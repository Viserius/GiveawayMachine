using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giveaway_Machine.Application.Gleam.GleamEntries;
using OpenQA.Selenium;

namespace Giveaway_Machine.Application.Gleam
{
    class GleamEntryActivator
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal void doEachAction(IWebDriver driver, GleamGiveaway gleamGiveaway)
        {
            logger.Debug("Starting doing each action for the current Gleam Giveaway...");

            // Get the elements
            ReadOnlyCollection<IWebElement> entryElements = driver.FindElements(By.CssSelector(".entry-method"));
            foreach(IWebElement el in entryElements)
            {
                if(el.GetAttribute("class").Contains("completed-entry-method"))
                {
                    continue;
                }
                try
                {
                    completeAction(driver, el, gleamGiveaway);
                } catch (Exception e)
                {
                    logger.Error(e, "Failed to complete Entry " + (entryElements.IndexOf(el)+1) + " for giveaway: " + gleamGiveaway.url);
                }
            }
        }

        private void completeAction(IWebDriver driver, IWebElement el, GleamGiveaway gleamGiveaway)
        {
            IWebElement entryTextElement = el.FindElement(By.CssSelector(".text.user-links"));
            string entryText = entryTextElement.Text;
            string identifier = el.GetAttribute("id");

            if(entryText.Contains("Follow") && entryText.Contains("on Twitter"))
            {
                GleamTwitterFollow.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.Contains("Tweet on Twitter"))
            {

                GleamTwitterTweet.activate(driver, el, gleamGiveaway, identifier);
            }
            else if(entryText.Contains("Retweet") && entryText.Contains("on Twitter"))
            {
                GleamTwitterRetweet.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.Contains("Join our Discord"))
            {
                GleamDiscordJoin.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.Contains("Tweet with hashtag"))
            {
                GleamTwitterHashtag.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.ToLower().Contains("sub") && el.FindElements(By.ClassName("youtube-border")).Count > 0)
            {
                GleamYoutubeSubscribe.activate(driver, el, gleamGiveaway);
            }
            else if(el.FindElements(By.ClassName("custom-border")).Count > 0) {
                GleamCustomURL.activate(driver, el, gleamGiveaway);
            }
            else if (el.FindElements(By.ClassName("email-border")).Count > 0)
            {
                GleamNewsletter.activate(driver, el, gleamGiveaway);
            }
            else if ((entryText.ToLower().Contains("watch") || entryText.ToLower().Contains("view")) && el.FindElements(By.ClassName("youtube-border")).Count > 0)
            {
                GleamYoutubeView.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.ToLower().Contains("visit") && el.FindElements(By.ClassName("instagram-border")).Count > 0)
            {
                GleamCustomURL.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.ToLower().Contains("visit") && el.FindElements(By.ClassName("facebook-border")).Count > 0)
            {
                GleamCustomURL.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.ToLower().Contains("view this") && entryText.ToLower().Contains("on facebook"))
            {
                GleamFacebookViewPost.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.ToLower().Contains("enter using twitter") && el.FindElements(By.ClassName("twitter-border")).Count > 0)
            {
                GleamCustomURL.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.ToLower().Contains("view this") && entryText.ToLower().Contains("on instagram"))
            {
                GleamFacebookViewPost.activate(driver, el, gleamGiveaway);
            }
            else
            {
                logger.Warn("Unhandled Gleam Entry: " + entryText + " for the giveaway: " + gleamGiveaway.url);
            } 
        }
    }
}
