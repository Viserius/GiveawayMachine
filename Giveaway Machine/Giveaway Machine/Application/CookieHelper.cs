using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Giveaway_Machine.Application
{
    class CookieHelper
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static Cookie Convert(Cookie c)
        {
            return new Cookie(
                c.Name,
                c.Value,
                c.Domain,
                c.Path,
                c.Expiry);
        }

        public static void LoadCookiesIfPossible(string URL, string serviceName, IWebDriver driver, bool needsRefresh)
        {
            FileStream fs;
            driver.Navigate().GoToUrl(URL);
            if (File.Exists(serviceName + "Cookies.dat"))
            {
                logger.Info("Loading previous " + serviceName + "cookies from file...");
                fs = new FileStream(serviceName + "Cookies.dat", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                IReadOnlyCollection<Cookie> cookies = (IReadOnlyCollection<Cookie>)formatter.Deserialize(fs);
                foreach (Cookie c in cookies)
                {
                    driver.Manage().Cookies.AddCookie(c);
                }
                fs.Close();
                logger.Info("Successfully loaded the Cookies for " + serviceName + "!");
                if(needsRefresh)
                    driver.Navigate().Refresh();
                return;
            }
            logger.Info("No cookies could be loaded for the " + serviceName + " website.");

        }

        public static void SaveCookies(string ServiceName, IWebDriver driver)
        {
            logger.Info("Saving cookies for the " + ServiceName + " Website...");
            FileStream fs = new FileStream(ServiceName + "Cookies.dat", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                IEnumerable<Cookie> cookies = driver.Manage().Cookies.AllCookies.Select(c => CookieHelper.Convert(c)).ToList();
                formatter.Serialize(fs, cookies);
            }
            catch (SerializationException e)
            {
                logger.Fatal(e, "Failed to serialize the " + ServiceName + " Cookies.");
                Console.Error.WriteLine();
                throw;
            }
            finally
            {
                fs.Close();
            }
            logger.Info("Successfully saved " + ServiceName + " Cookies to file!");
        }
    }
}
