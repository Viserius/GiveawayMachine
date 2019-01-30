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
    class GleamYoutubeView
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            logger.Info("Now trying to enter the giveaway by watching a video...");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            entryElement.Click();

            // Check if additional data must be entered
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);

            // Watch the video
            IWebElement clickButton = entryElement.FindElement(By.CssSelector("button.btn.btn-primary"));
            while (!clickButton.GetAttribute("class").Contains("ng-hide"))
            { Thread.Sleep(5000); }

            // Finalize
            entryElement.FindElement(By.CssSelector("button.btn.btn-primary:nth-child(2)")).Click();
            logger.Debug("Finished watching the video");
        }
    }
}
