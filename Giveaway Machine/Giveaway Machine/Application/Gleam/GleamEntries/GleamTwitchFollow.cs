using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Application.Gleam.GleamEntries
{
    class GleamTwitchFollow
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            logger.Info("Now trying to enter the giveaway by following on twitch...");
            entryElement.Click();

            // Check if additional data must be entered
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);

        }
    }
}
