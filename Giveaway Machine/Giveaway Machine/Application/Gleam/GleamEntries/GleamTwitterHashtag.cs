using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi;

namespace Giveaway_Machine.Application.Gleam.GleamEntries
{
    class GleamTwitterHashtag
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            logger.Info("Now trying to enter the giveaway by tweeting with Hashtag...");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            entryElement.Click();

            // Tweet
            string hashtag = Regex.Match(entryElement.FindElement(By.CssSelector(".user-links")).Text, @"(#[a-zA-Z0-9-_]+)").Value;
            User.GetAuthenticatedUser().PublishTweet(hashtag);

            // Submit
            waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.PartialLinkText("Already tweeted"))));
            entryElement.FindElement(By.PartialLinkText("Already tweeted")).Click();

            // Check if additional data must be entered
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);
        }
    }
}
