using Microsoft.Extensions.Caching.Memory;
using NewsProject.Models;
using System.Xml.Linq;

namespace NewsProject.Repository
{
    public class RSSRepository : IRSSRepository
    {
        private IConfiguration _config;

        public RSSRepository(IConfiguration config)
        {
            _config = config;

        }
        public async Task<List<NewsItem>> GetNewsFromRssFeed()
        {
            var rssUrl = _config.GetValue<string>("AppSettings:RssUrl");
            using (var httpClient = new HttpClient())
            {
                var rssFeed = await httpClient.GetStringAsync(rssUrl);
                var xDoc = XDocument.Parse(rssFeed);
                var newsList = MapNews(xDoc);
                return newsList;
            }
        }

        private List<NewsItem> MapNews(XDocument xDoc)
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
        //only for development because the environment was not open to internet
        public async Task<List<NewsItem>> GetNewsFromFile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "NewsRss.xml");

            if (!System.IO.File.Exists(filePath))
            {
                throw new Exception("xml file does not exist");
            }

            var xDoc = XDocument.Load(filePath);
            var newsList = MapNews(xDoc);
            return newsList;
        }
    }
}
