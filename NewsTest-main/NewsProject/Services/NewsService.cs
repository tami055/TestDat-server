using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using NewsProject.Models;
using NewsProject.Repository;
using System.Xml.Linq;

namespace NewsProject.Services
{
    public class NewsService : INewsService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IRSSRepository _rssRepository;
        private readonly IConfiguration _config;

        public NewsService(IMemoryCache memoryCache, IRSSRepository rssRepository, IConfiguration config)
        {
            _memoryCache = memoryCache;
            _rssRepository = rssRepository;
            _config = config;
        }

        
        public async Task<List<NewsItem>> GetNewsTitles()
        {
            var newsList = await GetNews();
            return newsList.Select((item, index) => new NewsItem
            {
                Id = item.Id,
                Title = item.Title,
            }).ToList();
        }

        public async Task<NewsItem> GetNewsDetailsById(int id)
        {
            var newsList = await GetNews();
            var selectedNews = newsList.SingleOrDefault(x => x.Id == id);

            if (selectedNews == null)
            {
                throw new KeyNotFoundException($"news id not exists in news collection");
            }

            return selectedNews;
        }
        public async Task<List<NewsItem>> GetNews()
        {
            var news = _memoryCache.Get<List<NewsItem>>("NewsItems");

            if (news is not null) return news;

            if (!_config.GetValue<bool>("AppSettings:BlockedEnviornment"))
            {
                news = await _rssRepository.GetNewsFromRssFeed();
            }
            else
            {
                news = await _rssRepository.GetNewsFromFile();
            }
            _memoryCache.Set("NewsItems", news, TimeSpan.FromHours(1));
            return news;

        }
    }
}
