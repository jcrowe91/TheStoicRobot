using System.Collections.Generic;

namespace TheStoicLibrary
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Root
    {
        public int id { get; set; }
        public string body { get; set; }
        public string author { get; set; }
        public string quotesource { get; set; }
        public List<string> keywords { get; set; }
        public string document_with_weights { get; set; }
    }
}
