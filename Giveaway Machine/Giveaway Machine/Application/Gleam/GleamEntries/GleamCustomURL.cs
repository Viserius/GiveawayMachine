using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Giveaway_Machine.Application.Gleam.GleamEntries
{
    class GleamCustomURL
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            logger.Info("Now trying to enter the giveaway by visiting a URL...");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            entryElement.Click();

            // Check if additional data must be entered
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);

            // Click the button
            waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector(".btn.btn-info.btn-large"))));
            entryElement.FindElement(By.CssSelector(".btn.btn-info.btn-large")).Click();

            // Close the new window
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            Thread.Sleep(20000);
            driver.Close();

            // Switch to original window
            driver.SwitchTo().Window(driver.WindowHandles.First());

            // complete
            waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector("a.btn.btn-primary"))));
            entryElement.FindElement(By.CssSelector("a.btn.btn-primary")).Click();
        }
    }
}
