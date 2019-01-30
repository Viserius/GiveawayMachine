using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace Giveaway_Machine.Application.Gleam.GleamEntries
{
    class GleamYoutubeSubscribe
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            logger.Info("Now trying to enter the giveaway by subscribing on youtube...");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            entryElement.Click();

            // Check if additional data must be entered
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);

            // Subscribe
            waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector(".btn.btn-info.btn-large"))));
            entryElement.FindElement(By.CssSelector(".btn.btn-info.btn-large")).Click();
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.First());

            // Submit
            waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector(".btn.btn-primary"))));
            entryElement.FindElement(By.CssSelector(".btn.btn-primary")).Click();
        }
    }
}
