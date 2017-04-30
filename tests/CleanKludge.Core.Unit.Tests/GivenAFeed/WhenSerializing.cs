using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CleanKludge.Api.Responses.Feed;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace CleanKludge.Core.Unit.Tests.GivenAFeed
{
    [TestClass]
    public class WhenSerializing
    {
        [TestMethod]
        public void Test()
        {
            var feed = new Feed
            {
                Version = "2.0",
                Channel = new Channel
                {

                    Description = "description",
                    Copyright = "copyright",
                    Link = "link",
                    Title = "title",
                    Items = new List<FeedItem>
                    {
                        new FeedItem
                        {
                            Link = "link1",
                            Description = "description1",
                            Title = "titl1",
                            Author = "author1",
                            PublishDate = "publishDate1"
                        },
                        new FeedItem
                        {
                            Link = "link2",
                            Description = "description2",
                            Title = "titl2",
                            Author = "author2",
                            PublishDate = "publishDate2"
                        }
                    }
                }
            };

            var xmlSerializer = new XmlSerializer(typeof(Feed));
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, feed);
                    Logger.LogMessage(stringWriter.ToString());
                }
            }
        }
    }
}