using System.Collections.Generic;
using CleanKludge.Core.Articles;
using CleanKludge.Core.Sections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanKludge.Data.File.Articles
{
    public class SectionRecord
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SectionType Type { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, string> Properties { get; set; }

        public static SectionRecord From(ISectionDto dto)
        {
            return new SectionRecord
            {
                Type = dto.Type,
                Properties = dto.Properties,
                Content = dto.Content
            };
        }

        public ISectionDto ToCoreObject()
        {
            return new SectionDto
            {
                Content = Content,
                Properties = Properties,
                Type = Type
            };
        }
    }
}