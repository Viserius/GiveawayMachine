using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models.Entities;
using Tweetinvi.Streaming;

namespace Giveaway_Machine.Application.Gleam
{
    class GleamFetcher
    {
        private IFilteredStream stream;
        private GleamProcessor GleamProcessor;
        private Facade facade;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public GleamFetcher(Facade f)
        {
            facade = f;
            GleamProcessor = new GleamProcessor(facade);
        }

        public void startFetching()
        {
            logger.Info("Starting to fetch giveaways for Gleam on Twitter...");

            stream = Stream.CreateFilteredStream();
            stream.AddTrack("gleam io");
            stream.MatchingTweetReceived += (sender, args) =>
            {
                logger.Info(args.Tweet);
                var urls = args.Tweet.Urls;
                foreach (IUrlEntity url in urls)
                {
                    GleamProcessor.Process(url.ExpandedURL, 5);
                }
            };

            stream.StreamStopped += (sender, args) =>
            {
                logger.Info(args.Exception, "Twitter Stream stopped with message:" + args.DisconnectMessage);
                logger.Info("Retrying to connect");
                stream.StartStreamMatchingAllConditions();
            };

            stream.StartStreamMatchingAllConditions();
        }

        internal void Stop()
        {
            logger.Info("Stopping the stream of incoming Gleam Giveaways...");
            if(stream != null)
                stream.StopStream();
            GleamProcessor.SaveProcessedURLs();
        }

        public GleamProcessor getProcessor()
        {
            return GleamProcessor;
        }
    }
}
