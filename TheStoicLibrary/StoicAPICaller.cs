using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TheStoicLibrary
{
    public class StoicAPICaller
    {
        private readonly HttpClient _client;
        const string stoicQuotesURL = "https://api.themotivate365.com/stoic-quote";

        public StoicAPICaller(HttpClient client)
        {
            _client = client;
        }

        public string GetStoicQuote()
        {
            var stoicResponse = _client.GetStringAsync(stoicQuotesURL).Result;
            var stoicData = JObject.Parse(stoicResponse).GetValue("data").ToString();
            var stoicQuote = JObject.Parse(stoicData).GetValue("quote").ToString();
            var stoicAuthor = JObject.Parse(stoicData).GetValue("author").ToString();
            string tweet = $"\"{stoicQuote}\"\n-{stoicAuthor}";
            return tweet;
        }
    }
}
