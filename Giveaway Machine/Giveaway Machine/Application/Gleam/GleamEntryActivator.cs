using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Giveaway_Machine.Application.Gleam.GleamEntries;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Giveaway_Machine.Application.Gleam
{
    class GleamEntryActivator
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        internal bool doEachAction(IWebDriver driver, GleamGiveaway gleamGiveaway)
        {
            logger.Debug("Starting doing each action for the current Gleam Giveaway...");

            GleamEnterDetails.activate(driver, driver.FindElement(By.TagName("body")), gleamGiveaway);

            // Get the elements
            ReadOnlyCollection<IWebElement> entryElements = driver.FindElements(By.CssSelector(".entry-method"));
            bool hasDoneOneEntry = false;
            foreach(IWebElement el in entryElements)
            {
                if(el.GetAttribute("class").Contains("completed-entry-method"))
                {
                    continue;
                }
                try
                {
                    if(completeAction(driver, el, gleamGiveaway, entryElements.IndexOf(el) + 1))
                        hasDoneOneEntry = true;
                } catch (Exception e)
                {
                    logger.Error(e, "Failed to complete Entry " + (entryElements.IndexOf(el)+1) + " for giveaway: " + gleamGiveaway.url);
                }
            }
            return hasDoneOneEntry;
        }

        private bool completeAction(IWebDriver driver, IWebElement el, GleamGiveaway gleamGiveaway, int number)
        {
            WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(el));
            } catch (Exception e)
            {
                logger.Info(e, "Invisible action. Skipping...");
                return false;
            }

            IWebElement entryTextElement = el.FindElement(By.CssSelector(".text.user-links"));
            string entryText = entryTextElement.Text;
            string identifier = el.GetAttribute("id");

            // Skip hidden entries
            if (entryText.Length < 3)
                return false;

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
            else if (el.FindElements(By.ClassName("discord-border")).Count > 0 && entryText.ToLower().Contains("join"))
            {
                GleamDiscordJoin.activate(driver, el, gleamGiveaway);
            }
            else if (el.FindElements(By.ClassName("twitter-border")).Count > 0 && entryText.ToLower().Contains("#"))
            {
                GleamTwitterHashtag.activate(driver, el, gleamGiveaway);
            }
            else if (entryText.ToLower().Contains("sub") && el.FindElements(By.ClassName("youtube-border")).Count > 0)
            {
                GleamYoutubeSubscribe.activate(driver, el, gleamGiveaway);
            }
            else if(el.FindElements(By.ClassName("custom-border")).Count > 0 && el.FindElements(By.ClassName("fa-external-link-square")).Count > 0) {
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
            else if (entryText.ToLower().Contains("enter using") && el.FindElements(By.ClassName("youtube-border")).Count > 0)
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
            else if (el.FindElements(By.ClassName("custom-border")).Count > 0 && el.FindElements(By.ClassName("fa-star")).Count > 0)
            {
                GleamDailyBonus.activate(driver, el, gleamGiveaway);
            }
            else if (el.FindElements(By.ClassName("twitchtv-border")).Count > 0 && entryText.ToLower().Contains("follow"))
            {
                GleamTwitchFollow.activate(driver, el, gleamGiveaway);
            }
            else if (el.FindElements(By.ClassName("twitchtv-border")).Count > 0 && entryText.ToLower().Contains("enter using twitch"))
            {
                GleamDailyBonus.activate(driver, el, gleamGiveaway);
            }
            else if (el.FindElements(By.ClassName("googleplus-border")).Count > 0 && entryText.ToLower().Contains("visit"))
            {
                GleamCustomURL.activate(driver, el, gleamGiveaway);
            }
            else if (el.FindElements(By.ClassName("linkedin-border")).Count > 0 && entryText.ToLower().Contains("follow"))
            {
                GleamLinkedInFollow.activate(driver, el, gleamGiveaway);
            }else if (entryText.Equals("Refer Friends For Extra Entries"))
            {
                logger.Debug("Skipping Friend Referral Entry");
                return false;
            }
            else if(el.FindElements(By.ClassName("youtube-border")).Count > 0 && entryText.ToLower().Contains("visit"))
            {
                GleamCustomURL.activate(driver, el, gleamGiveaway);
            } else if(el.FindElements(By.ClassName("twitchtv-border")).Count > 0 && entryText.ToLower().Contains("bonus for twitch subscribers"))
            {
                logger.Debug("Skipping Twitch Subscribers Entry");
                return false;
            }

            // default case for custom border
            else if (el.FindElements(By.ClassName("custom-border")).Count > 0)
            {
                GleamCustomURL.activate(driver, el, gleamGiveaway);
            }
            else
            {
                logger.Warn("Unhandled Gleam Entry: " + entryText + " for the giveaway: " + gleamGiveaway.url);
                return false;
            }
            return true;
        }
    }
}
