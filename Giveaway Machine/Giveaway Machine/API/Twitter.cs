using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using Tweetinvi;
using Tweetinvi.Models;

namespace Giveaway_Machine.API
{
    class Twitter
    {
        public Twitter()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("Initializing Twitter API.");

            Console.WriteLine("Setting Twitter Credentials.");
            // Set credentials
            Auth.ApplicationCredentials = new TwitterCredentials(
                ConfigurationManager.AppSettings.Get("TwitterConsumerKey"),
                ConfigurationManager.AppSettings.Get("CONSUMER_SECRET"),
                ConfigurationManager.AppSettings.Get("ACCESS_TOKEN"),
                ConfigurationManager.AppSettings.Get("ACCESS_TOKEN_SECRET")
            );
            Console.WriteLine("Twitter Credentials are now set.");

        }
    }
}
