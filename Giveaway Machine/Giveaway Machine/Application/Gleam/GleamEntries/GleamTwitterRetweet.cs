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
    class GleamTwitterRetweet
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal static void activate(IWebDriver driver, IWebElement entryElement, GleamGiveaway gleamGiveaway)
        {
            logger.Info("Now trying to enter the giveaway by Retweeting...");
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            entryElement.Click();

            // Check if additional data must be entered
            GleamEnterDetails.activate(driver, entryElement, gleamGiveaway);

            // Tweet
            long TweetIdentifier = long.Parse(entryElement.FindElement(By.CssSelector("twitter-widget")).GetAttribute("data-tweet-id"));
            Tweet.PublishRetweet(TweetIdentifier);

            // Submit
            //waiter.Until(ExpectedConditions.ElementToBeClickable(entryElement.FindElement(By.CssSelector(".btn.btn-primary"))));
            //entryElement.FindElement(By.CssSelector(".btn.btn-primary")).Click();
        }
    }
}
