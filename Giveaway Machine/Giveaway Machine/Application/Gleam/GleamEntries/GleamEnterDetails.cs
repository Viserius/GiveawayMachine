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
    class GleamEnterDetails
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            if (entryElement.FindElements(By.CssSelector("input[required=\"required\"]")).Count > 0 
                && entryElement.FindElements(By.CssSelector(".center.details-header")).Count > 0
                && entryElement.FindElement(By.CssSelector("input[required=\"required\"]")).Displayed
                && entryElement.FindElement(By.CssSelector(".center.details-header")).Displayed
                && entryElement.FindElement(By.CssSelector(".form-wrapper label.checkbox")).Enabled)
            {
                logger.Info("Filling in personal information before entering...");
                WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                // Click the button
                waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector(".form-wrapper label.checkbox"))));
                entryElement.FindElement(By.CssSelector(".form-wrapper label.checkbox")).Click();

                try
                {
                    entryElement.FindElement(By.CssSelector("input[age-format=\"MDY\"]")).SendKeys("02071998");
                } catch (NoSuchElementException e)
                {
                    logger.Debug("Enter form does not include date.");
                }

                // complete
                waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector("button.btn.btn-primary.ng-scope"))));
                entryElement.FindElement(By.CssSelector("button.btn.btn-primary.ng-scope")).Click();

                // Wait before continuing
                try
                {
                    while(entryElement.FindElement(By.CssSelector(".contestant.compact-box")).Displayed) { Thread.Sleep(1 * 1000); }
                } catch (Exception e)
                {
                    logger.Info("Succesfully entered information... Now continuing...");
                }
            }
        }
    }
}
