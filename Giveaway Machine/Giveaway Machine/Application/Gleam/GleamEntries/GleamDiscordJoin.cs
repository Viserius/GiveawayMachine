using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;

namespace Giveaway_Machine.Application.Gleam.GleamEntries
{
    class GleamDiscordJoin
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            logger.Info("Now trying to enter the giveaway by joining the Discord...");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            entryElement.Click();

            // Check if additional data must be entered
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);

            // Join the discord
            Thread.Sleep(1000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector(".btn.btn-info.btn-large"))));
            entryElement.FindElement(By.CssSelector(".btn.btn-info.btn-large")).Click();
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            // Submit
            driver.FindElement(By.CssSelector("button")).Click();
            Thread.Sleep(2000);

            // Switch to original window
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.First());

            // complete
            entryElement.FindElement(By.CssSelector("button.btn.btn-primary")).Click();
        }
    }
}
