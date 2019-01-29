using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using Tweetinvi;
using Tweetinvi.Models;
using Giveaway_Machine.Controller;

namespace Giveaway_Machine.API
{
    class Twitter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Facade facade;

        public Twitter(Facade facade)
        {
            logger.Info("Initializing Twitter API.");

            this.facade = facade;

            logger.Info("Loading Twitter Credentials...");
            // Set credentials
            Auth.ApplicationCredentials = new TwitterCredentials(
                ConfigurationManager.AppSettings.Get("TwitterConsumerKey"),
                ConfigurationManager.AppSettings.Get("TwitterConsumerSecret"),
                ConfigurationManager.AppSettings.Get("TwitterAccessToken"),
                ConfigurationManager.AppSettings.Get("TwitterAccessTokenSecret")
                );
            logger.Info("Twitter Credentials are now loaded.");
        }

        public bool twitterAuthenticated()
        {
            try
            {
                var user = User.GetAuthenticatedUser(Auth.ApplicationCredentials);
            } catch(Exception e)
            {
                logger.Error(e, "Could not authenticate to twitter!");
                return false;
            }
            return true;
        }
    }
}
