using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tweetinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Auth;

namespace TheStoicRobotTwitter
{
    class Program
    {          
        static async Task Main()
        {
            var randomQuote = QuoteManager.RandomeQuote();    

            ConsoleLogging.PassMessage("TheStoicRobot");
            ConsoleLogging.PassMessage($"<{DateTime.Now}> - Getting Authorization");

            await QuoteManager.TweetAsync(randomQuote);           
        }        
    }
}

