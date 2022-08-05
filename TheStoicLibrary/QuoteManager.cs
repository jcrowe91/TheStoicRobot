using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;


namespace TheStoicLibrary
{
    public class QuoteManager
    {
        public delegate void LogToConsole(string message, ConsoleColor consoleColor = ConsoleColor.White);


            
        private static string GetAPIKey()
        {
            IConfiguration builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return builder["apiKey"];
                
        }

        private static string GetAPISecret()
        {
            IConfiguration builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return builder["apiSecret"];

        }

        private static string GetUsername()
        {
            IConfiguration builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return builder["twitterUsername"];

        }

        private static string GetTwitterPass()
        {
            IConfiguration builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return builder["twitterPass"];

        }

        public static async Task TweetAsync(string quote, LogToConsole consoleLog)
        {
            string apiKey = GetAPIKey();
            string apiSecret = GetAPISecret();
            string userName = GetUsername();
            string twitterPass = GetTwitterPass();
            var driver = new ChromeDriver();

            // Create a client
            var appClient = new TwitterClient(apiKey, apiSecret);

            // Start the authentication process
            var authenticationRequest = await appClient.Auth.RequestAuthenticationUrlAsync();
            var authCode = GetAuthCode(driver, authenticationRequest, userName, twitterPass);

            // Ask to enter the pin code given by Twitter
            consoleLog("Please enter the code and press enter.");
            var pinCode = authCode.Text;

            // With this pin code it is now possible to get the credentials back from Twitter
            var userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(pinCode, authenticationRequest);
                        
            var userClient = new TwitterClient(userCredentials);
            var user = await userClient.Users.GetAuthenticatedUserAsync();

            consoleLog("Congratulations, you have authenticated the user: " + user, ConsoleColor.Green);
            Console.ResetColor();

            consoleLog($"<{DateTime.Now}> - Bot Tweeting", ConsoleColor.DarkRed);

            var tweet = await userClient.Tweets.PublishTweetAsync($"{quote}");

            consoleLog($"Success! Tweeted {tweet}", ConsoleColor.Green);
            
            driver.Close();
            driver.Quit();
        }

        public static IWebElement GetAuthCode(ChromeDriver driver, Tweetinvi.Models.IAuthenticationRequest authenticationRequest, string userName, string twitterPass)
        {
            driver.Manage().Window.Minimize();

            driver.Navigate().GoToUrl(authenticationRequest.AuthorizationURL);

            IWebElement userNameElement = driver.FindElement(By.Id("username_or_email"));
            userNameElement.SendKeys(userName);

            IWebElement passwordElement = driver.FindElement(By.Id("password"));
            passwordElement.SendKeys(twitterPass);

            IWebElement authButton = driver.FindElement(By.Id("allow"));
            authButton.Submit();

            IWebElement authCode = driver.FindElement(By.CssSelector("#oauth_pin > p > kbd > code"));

            return authCode;
        }
    }
    #region DEPRECATED CODE
    /*  FOLLOWING CODE OBSOLETE AS OF 6/3/2022
     
     //public static Quote RandomeQuote()
        //{
        //    Random rng = new();
        //    List<Quote> quotes = new List<Quote>();

        //    string filePath = $@"{Directory.GetCurrentDirectory()}\quotes.csv";
        //    List<string> lines = File.ReadAllLines(filePath).ToList();

        //    foreach (var line in lines)
        //    {
        //        string[] entries = line.Split(',');

        //        Quote newQuote = new Quote()
        //        {
        //            QuoteContent = entries[0],
        //            Author = entries[1],
        //        };

        //        quotes.Add(newQuote);
        //    }

        //    var randomQuote = quotes[rng.Next(quotes.Count())];
        //    return randomQuote;
        //}

    public static Quote RandomeQuote()
    {
        Random rng = new();
        List<Quote> quotes = new List<Quote>();

        string filePath = $@"{Directory.GetCurrentDirectory()}\quotes.csv";
        List<string> lines = File.ReadAllLines(filePath).ToList();

        foreach (var line in lines)
        {
            string[] entries = line.Split(',');

            Quote newQuote = new Quote()
            {
                QuoteContent = entries[0],
                Author = entries[1],
            };

            quotes.Add(newQuote);
        }

        var randomQuote = quotes[rng.Next(quotes.Count())];
        return randomQuote;
    }

    */
    #endregion
}
