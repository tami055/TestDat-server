using NewsProject.Models;

namespace NewsProject.Repository
{
    public interface IRSSRepository
    {
        Task<List<NewsItem>> GetNewsFromRssFeed();
        Task<List<NewsItem>> GetNewsFromFile();
    }
}