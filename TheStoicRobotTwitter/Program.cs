using System;
using System.Net.Http;
using System.Threading.Tasks;
using TheStoicLibrary;

namespace TheStoicRobotTwitter
{
    class Program
    {          
        static async Task Main()
        {
            ConsoleLogging.PassMessage("Welcome to the TheStoicRobot", ConsoleColor.Yellow);

            StoicAPICaller caller = new StoicAPICaller(new HttpClient());
            var quote = caller.GetStoicQuote();

            ConsoleLogging.PassMessage($"Quote to be tweeted: {quote}", ConsoleColor.Blue);

            ConsoleLogging.PassMessage($"<{DateTime.Now}> - Getting Authorization", ConsoleColor.DarkRed);

            await QuoteManager.TweetAsync(quote, ConsoleLogging.PassMessage);

            ConsoleLogging.PassMessage("Success!", ConsoleColor.Green);
        }        
    }
}

