using Newtonsoft.Json;
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
        const string stoicQuotesURLAlt = "https://stoic-server.herokuapp.com/random";

        public StoicAPICaller(HttpClient client)
        {
            _client = client;
        }

        public string GetStoicQuote()
        {
            var stoicResponse = _client.GetStringAsync(stoicQuotesURL).Result;
            

            var stoicQuote = JObject.Parse(stoicResponse).GetValue("quote").ToString();
            var stoicAuthor = JObject.Parse(stoicResponse).GetValue("author").ToString();
            string tweet = $"\"{stoicQuote}\"\n-{stoicAuthor}";
            return tweet;
        }

        public string GetStoicQuoteAlternative()
        {
            var stoicResponse = _client.GetStringAsync(stoicQuotesURLAlt).Result;
            Root deserializedRespose = JsonConvert.DeserializeObject<List<Root>>(stoicResponse).FirstOrDefault();
            var tweet = $"\"{deserializedRespose.body}\"\n-{deserializedRespose.author}";
            return tweet;
        }
    }
}
