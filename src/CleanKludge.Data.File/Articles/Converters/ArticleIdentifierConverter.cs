using System;
using CleanKludge.Core.Articles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CleanKludge.Data.File.Articles.Converters
{
    public class ArticleIdentifierConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var articleIdentifier = value as ArticleIdentifier;
            serializer.Serialize(writer, articleIdentifier?.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return token.Type != JTokenType.String ? ArticleIdentifier.None : ArticleIdentifier.From(token.Value<string>());
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ArticleIdentifier);
        }
    }
}