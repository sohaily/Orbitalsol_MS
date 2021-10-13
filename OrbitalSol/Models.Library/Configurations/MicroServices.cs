using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Library.Configurations
{
    public class MicroServices
    {
        [JsonProperty("ApplicationUrl")] public Uri ApplicationUrl { get; set; }

        [JsonProperty("Authority")] public Uri Authority { get; set; }

        [JsonProperty("Cms")] public Uri Cms { get; set; }

        [JsonProperty("EmailApi")] public Uri EmailApi { get; set; }

        [JsonProperty("ItemsApi")] public Uri ItemsApi { get; set; }

        [JsonProperty("Migration")] public Uri Migration { get; set; }

        [JsonProperty("PdfApi")] public Uri PdfApi { get; set; }

        [JsonProperty("ReverseProxy")] public Uri ReverseProxy { get; set; }

        [JsonProperty("TranslationApi")] public Uri TranslationApi { get; set; }

        [JsonProperty("TranslationWebApp")] public Uri TranslationWebApp { get; set; }

        [JsonProperty("WordPress")] public Uri WordPress { get; set; }

        [JsonProperty("NotificationApi")] public Uri NotificationApi { get; set; }

        [JsonProperty("HttpsAllowed")] public bool HttpsAllowed { get; set; }

        [JsonProperty("MarketApi")] public Uri MarketApi { get; set; }

        [JsonProperty("FeedApi")] public Uri FeedApi { get; set; }
    }
}
