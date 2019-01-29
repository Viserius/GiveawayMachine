using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace Giveaway_Machine.Application.Gleam.GleamEntries
{
    class GleamTwitterTweet
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway, string identifier)
        {
            logger.Debug("Now trying to enter the giveaway by tweeting...");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            entryElement.Click();

            // Tweet
            waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector(".quoted-text .quoted-text__content"))));
            string Tweet = entryElement.FindElement(By.CssSelector(".quoted-text .quoted-text__content")).Text;
            User.GetAuthenticatedUser().PublishTweet(Tweet + " " + gleamGiveaway.url);

            // Submit
            entryElement.FindElement(By.PartialLinkText("Continue")).Click();
        }
    }
}