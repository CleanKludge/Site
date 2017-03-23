using System;
using System.Collections.Generic;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Articles.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanKludge.Data.File.Articles
{
    public class ArticleSummaryRecord : IArticleSummaryDto
    {
        [JsonProperty("identifier")]
        [JsonConverter(typeof(ArticleIdentifierConverter))]
        public ArticleIdentifier Identifier { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("location")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Location Location { get; set; }
    }
}