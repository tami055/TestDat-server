using Microsoft.Extensions.Caching.Memory;
using NewsProject.Models;
using System.Xml.Linq;

namespace NewsProject.Repository
{
    public class RSSRepository : IRSSRepository
    {
        private const string RssFeedUrl = "http://news.google.com/news?pz=1&cf=all&ned=en_il&hl=en&output=rss";

        public async Task<List<NewsItem>> GetNewsFromRssFeed()
        {
            using (var httpClient = new HttpClient())
            {
                var rssFeed = await httpClient.GetStringAsync(RssFeedUrl);
                var xDoc = XDocument.Parse(rssFeed);
                var newsList = MapNews(xDoc);
                return newsList;
            }
        }

        private static List<NewsItem> MapNews(XDocument xDoc)
        {
            var counter = 0;
            var newsList = xDoc.Descendants("item").Select(item => new NewsItem
            {
                Id = counter++,
                Title = item.Element("title")?.Value,
                Body = item.Element("description")?.Value,
                Link = item.Element("link")?.Value
            }).ToList();
            return newsList;
        }

        public async Task<List<NewsItem>> GetNewsFromFile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "NewsRss.xml");

            if (!System.IO.File.Exists(filePath))
            {
                // Handle the case where the XML file doesn't exist
                return new List<NewsItem>();
            }

            var xDoc = XDocument.Load(filePath);
            var newsList = MapNews(xDoc);
            return newsList;
        }
    }
}
