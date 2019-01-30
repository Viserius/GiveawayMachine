using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Tweetinvi;

namespace Giveaway_Machine.Application.Gleam.GleamEntries
{
    class GleamTwitterFollow
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            logger.Info("Now trying to enter the giveaway by following...");

            // Follow the user
            string toFollowUser = Regex.Match(entryElement.FindElement(By.CssSelector(".user-links")).Text, @"(@[a-zA-Z0-9-_]+)").Value;
            User.GetAuthenticatedUser().FollowUser(toFollowUser);

            // Click the open-button
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            entryElement.Click();

            // Check if additional data must be entered
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);

            // Submit
            /*try
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector("button.btn.btn-primary"))));
            } catch (NoSuchElementException e)
            {
                if (entryElement.GetAttribute("class").Contains("completed-entry-method"))
                    return;
                throw e;
            }
            entryElement.FindElement(By.CssSelector("button.btn.btn-primary")).Click();*/

        }
    }
}
