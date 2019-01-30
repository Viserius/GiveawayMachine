# GiveawayMachine
GiveawayMachine is a C# application, that crawls for Gleam.IO giveaways, and automatically enters them.

**Disclaimer: Use at your own risk. I'm unaware of Gleam-accounts getting banned, but it might happen.**

# Current Features
* Use the Twitter (Streaming) API (with the Tweetinvi library) to find Gleam.IO giveaways
* Use Selenium (Browser Driver Library) to navigate the giveaway-pages algorithmicly
* Logging
* Automatically login into Gleam, Discord and Twitter
* Automatically enter existing giveaways

    * Tweeting
    * Retweeting
    * Following on Twitter
    * Tweeting Hashtag
    * Following on Twitch (Make sure the account is connected)
    * YouTube Subscribe
    * YouTube Watch Video
    * View FaceBook Post
    * View Instagram Post
    * LinkedIn Follow (requires no account)
    * Enter additional details such as Date of Birth if necessary
    * Join Discord Server
    * Claim Daily Bonus
    * Visit a URL
* Daily revisit the giveaways you already processed, if there are daily entries
* Saving and Loading Cookies, to remember logged-in status, instead of logging in every time
* Console-application with integrated command system

# Installation
1. Clone the repository
2. Apply for a developers account at developer.twitter.com
3. Create a Twitter-App
4. Generate Consumer Keys and Access Tokens for that Twitter App
5. Copy App.config.example to App.Config and enter your information under appSettings (credentials are only used locally)
6. Download a Chrome Driver that's compatible with your version of chrome, and place it in the project folder. (chromedriver.chromium.org)

# Running
Compile using Visual Studio, and run from the debug folder

# Contributing
Feel free to contribute, open issues or open Pull Requests.