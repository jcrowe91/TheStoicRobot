using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;


namespace TheStoicRobotTwitter
{
    public class QuoteManager
    {
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

        public static async Task TweetAsync(Quote randomQuote)
        {
            string apiKey = GetAPIKey();
            string apiSecret = GetAPISecret();
            // Create a client
            var appClient = new TwitterClient(apiKey, apiSecret);

            // Start the authentication process
            var authenticationRequest = await appClient.Auth.RequestAuthenticationUrlAsync();

            // Go to the URL so that Twitter authenticates the user and gives a PIN code.
            Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL)
            {
                UseShellExecute = true
            });

            // Ask to enter the pin code given by Twitter
            ConsoleLogging.PassMessage("Please enter the code and press enter.");
            var pinCode = Console.ReadLine();

            // With this pin code it is now possible to get the credentials back from Twitter
            var userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(pinCode, authenticationRequest);
                        
            var userClient = new TwitterClient(userCredentials);
            var user = await userClient.Users.GetAuthenticatedUserAsync();

            ConsoleLogging.GreenConsole();
            ConsoleLogging.PassMessage("Congratulation you have authenticated the user: " + user);
            Console.ResetColor();

            ConsoleLogging.PassMessage($"<{DateTime.Now}> - Bot Tweeting");

            var tweet = await userClient.Tweets.PublishTweetAsync($"\"{randomQuote.QuoteContent}\" \n-{randomQuote.Author}");

            ConsoleLogging.GreenConsole();
            ConsoleLogging.PassMessage($"Success! Tweeted {tweet}");
            Console.ResetColor();
        }
    }
}
