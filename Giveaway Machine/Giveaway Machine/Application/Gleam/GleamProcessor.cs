using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace Giveaway_Machine.Application.Gleam
{
    class GleamProcessor
    {
        private Facade facade;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static Regex rx = new Regex(@"\b^(https:\/\/gleam.io\/\w+\/[a-zA-Z0-9-]+)");
        private Dictionary<string, GleamGiveaway> processedGiveaways;
        private GleamEntryActivator gleamEntryActivator = new GleamEntryActivator();
        IWebDriver driver;

        public GleamProcessor(Facade f)
        {
            facade = f;
            LoadProcessedURLs();

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("no-sandbox");
            chromeOptions.AddArgument("-no-sandbox");
            driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), chromeOptions);

            CookieHelper.LoadCookiesIfPossible("https://gleam.io/404", "Gleam", driver, false);
            discordLogin();

            processOverdueDailyGiveaways();
        }

        private void processOverdueDailyGiveaways()
        {
            foreach(KeyValuePair<string, GleamGiveaway> kv in processedGiveaways.Where(c => c.Value != null && c.Value.hasDailyEntry).Where(c => c.Value != null && c.Value.lastEntry < DateTime.Today))
            {
                logger.Info("Loading Already Entered giveaway with daily entries...");
                Process(kv.Key, 1);
            }
        }

        private void LoadProcessedURLs()
        {
            logger.Info("Loading Past Gleam Giveaways from file...");
            FileStream fs;
            try
            {
                fs = new FileStream("GleamGiveaways.dat", FileMode.Open);
            } catch (FileNotFoundException e)
            {
                logger.Warn(e, "Could not load Gleam Giveaways... File is missing!");
                processedGiveaways = new Dictionary<string, GleamGiveaway>();
                return;
            }
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the URLs from the file and 
                // assign the reference to the local variable.
                processedGiveaways = (Dictionary<string,GleamGiveaway>)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                logger.Fatal(e, "Failed to load GleamGiveaways...");
                throw;
            }
            finally
            {
                fs.Close();
            }
            logger.Info("Successfully loaded {0} Gleam Giveaways from file! Hooray!", processedGiveaways.Count);
        }

        public static bool IsValidURL(string URL)
        {
            return rx.IsMatch(URL);
        }

        internal void Process(string expandedURL, int timeoutMinutes)
        {
            logger.Info("Now processing Gleam Giveaway with URL: " + expandedURL);
            if (processedGiveaways.ContainsKey(expandedURL))
            {
                logger.Info("Already entered... Skipping this one...");
                return;
            }

            // Go to the Giveaway
            driver.Navigate().GoToUrl(expandedURL);
            if (!checkIfValidURL()) return;
            LoginIfNecessary();

            // For each action, call the activator
            GleamGiveaway gleamGiveaway = loadGiveAwayObject(expandedURL);
            bool succeed = gleamEntryActivator.doEachAction(driver, gleamGiveaway);
            if(succeed) gleamEntryActivator.doEachAction(driver, gleamGiveaway);

            if (succeed && hasDailyEntries())
                gleamGiveaway.hasDailyEntry = true;
            gleamGiveaway.lastEntry = DateTime.Now;

            // Add to the list
            if (!processedGiveaways.ContainsKey(gleamGiveaway.url))
                processedGiveaways.Add(gleamGiveaway.url, gleamGiveaway);
            // Save out of precaution
            SaveProcessedURLs();

            // Sleep before doing the next one to prevent detection...
            if (succeed)
            {
                logger.Info("Gleam Processor goes to sleep, to prevent detection...");
                Thread.Sleep(timeoutMinutes * 60 * 1000);
            }
        }

        private bool checkIfValidURL()
        {
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                waiter.Until(ExpectedConditions.ElementExists(By.CssSelector(".campaign.competition")));

                waiter.Until(ExpectedConditions.ElementExists(By.CssSelector(".purple-square span span")));
                if (driver.FindElement(By.CssSelector(".purple-square span span")).Text.Contains("Ended"))
                    return false;

            } catch (Exception e)
            {
                logger.Info(e, "This is not a valid URL.");
                processedGiveaways.Add(driver.Url, null);
                return false;
            }
            return true;
        }

        private GleamGiveaway loadGiveAwayObject(string URL)
        {
            logger.Debug("Creating Giveaway object by using the data from the website...");
            GleamGiveaway gleamGiveaway = new GleamGiveaway();
            gleamGiveaway.created = DateTime.Now;
            gleamGiveaway.url = URL;
            gleamGiveaway.hasDailyEntry = false;
            gleamGiveaway.lastEntry = DateTime.Now.AddDays(-1);
            return gleamGiveaway;
        }

        private bool LoginIfNecessary()
        {
            // Check if already logged in
            if (driver.PageSource.Contains("Mark S."))
            {
                logger.Debug("De browser was al ingelogd!");
                return true;
            }

            logger.Info("Gleam is nog niet ingelogd met Twitter... Nu aan het inloggen...");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Check if the page successfully loads:
            bool openedLoginWindow = false;
            while(!openedLoginWindow)
            {
                openedLoginWindow = SwitchToTwitterLoginPage();
            }

            logger.Debug("Entering information");
            driver.FindElement(By.Id("username_or_email")).SendKeys(ConfigurationManager.AppSettings.Get("TwitterMail"));
            driver.FindElement(By.Id("password")).SendKeys(ConfigurationManager.AppSettings.Get("TwitterPassword"));
            driver.FindElement(By.Id("remember")).Click();
            driver.FindElement(By.Id("allow")).Click();

            logger.Debug("Switching back to Gleam");
            driver.SwitchTo().Window(driver.WindowHandles.First());

            // Check if actually logged in...
            waiter.Until(e => e.FindElement(By.CssSelector(".text:nth-child(2) .small-bar--text > .ng-binding")));
            string loggedInText = driver.FindElement(By.CssSelector(".text:nth-child(2) .small-bar--text > .ng-binding")).Text;
            if (loggedInText.Contains("Entering as"))
            {
                logger.Info("Successfully logged in into Twitter!");
                CookieHelper.SaveCookies("Gleam", driver);
                return true;
            } else
            {
                logger.Error("Failed to login into Twitter! After logging in, the text says: " + loggedInText);
                return false;
            }
        }

        private bool SwitchToTwitterLoginPage()
        {
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            logger.Debug("Clicking on Twitter login icon...");
            driver.FindElement(By.CssSelector(".text:nth-child(2) .ng-scope:nth-child(5) .fa")).Click();

            logger.Debug("Switching to Twitter login page...");
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            try
            {
                waiter.Until(ExpectedConditions.ElementExists(By.Id("header")));
            } catch (WebDriverException e)
            {
                logger.Error(e, "Failed to open the twitter login screen...");
                driver.SwitchTo().Window(driver.WindowHandles.First());
                return false;
            }
            return true;
        }

        public void SaveProcessedURLs()
        {
            logger.Info("Attempting to save the list of processed Gleam Giveaways...");
            FileStream fs = new FileStream("GleamGiveaways.dat", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, processedGiveaways);
            }
            catch (SerializationException e)
            {
                logger.Fatal(e, "Failed to serialize the Gleam Giveaways.");
                throw;
            }
            finally
            {
                fs.Close();
            }
            logger.Info("Successfully saved {0} Gleam Giveaways to file!", processedGiveaways.Count);
        }

        public void discordLogin()
        {
            driver.Navigate().GoToUrl("https://discordapp.com/login");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            if (driver.FindElements(By.CssSelector("input[type=\"email\"]")).Count > 0)
            {
                logger.Info("Trying to log into Discord");
                waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[type=\"email\"]")));
                driver.FindElement(By.CssSelector("input[type=\"email\"]")).SendKeys(ConfigurationManager.AppSettings.Get("DiscordMail"));
                driver.FindElement(By.CssSelector("input[type=\"password\"]")).SendKeys(ConfigurationManager.AppSettings.Get("DiscordPassword"));
                driver.FindElement(By.CssSelector("button[type=\"submit\"]")).Click();
                Thread.Sleep(5000);
            }

        }

        public bool hasDailyEntries()
        {
            return (driver.FindElements(By.ClassName("fa-clock-o")).Count > 0) || (driver.PageSource.Contains("Daily Bonus Entry"));
        }
    }
}
