using NewsProject.Models;

namespace NewsProject.Services
{
    public interface INewsService
    {
        Task<List<NewsItem>> GetNews();
        Task<NewsItem> GetNewsDetailsById(int id);
        Task<List<NewsItem>> GetNewsTitles();
    }
}