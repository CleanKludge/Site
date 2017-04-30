using System.Collections.Generic;
using System.Xml.Serialization;

namespace CleanKludge.Api.Responses.Feed
{
    [XmlRoot("rss", Namespace = "")]
    public class Feed
    {
        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement("channel")]
        public Channel Channel { get; set; }

        public Feed()
        {
            Version = "2.0";
        }
    }

    public class Channel
    {
        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("copyright")]
        public string Copyright { get; set; }

        [XmlElement("item")]
        public List<FeedItem> Items { get; set; }
    }

    public class FeedItem
    {
        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("pubDate")]
        public string PublishDate { get; set; }
    }
}