using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Application.Gleam.GleamEntries
{
    class GleamFacebookViewPost
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);
            logger.Info("Now trying to enter the giveaway by watching a Facebook Post...");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            entryElement.Click();

            // Check if additional data must be entered
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);

            // Watch the video
            waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector("a.btn.btn-primary"))));
            entryElement.FindElement(By.CssSelector("a.btn.btn-primary")).Click();
        }
    }
}
