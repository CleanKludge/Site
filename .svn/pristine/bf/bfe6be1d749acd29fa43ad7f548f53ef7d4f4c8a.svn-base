using System;
using System.Collections.Generic;
using System.Linq;
using CleanKludge.Core.Articles;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanKludge.Data.File.Articles
{
    public class ArticleRecord
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

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

        [JsonProperty("sections")]
        public List<SectionRecord> Sections { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("location")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Location Location { get; set; }

        public static ArticleRecord From(IArticleDto dto)
        {
            return new ArticleRecord
            {
                Author = dto.Author,
                Created = dto.Created,
                Enabled = dto.Enabled,
                Description = dto.Description,
                Identifier = dto.Identifier.ToString(),
                Location = dto.Location,
                Tags = dto.Tags,
                Title = dto.Title,
                Sections = dto.Sections.Select(SectionRecord.From).ToList()
            };
        }

        public IArticleDto ToCoreObject()
        {
            return new ArticleDto
            {
                Author = Author,
                Created = Created,
                Description = Description,
                Identifier = ArticleIdentifier.From(Identifier),
                Location = Location,
                Enabled = Enabled,
                Tags = Tags,
                Title = Title,
                Sections = Sections.Select(x => x.ToCoreObject()).ToList()
            };
        }
    }
}