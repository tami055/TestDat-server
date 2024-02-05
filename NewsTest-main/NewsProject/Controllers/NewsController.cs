using Microsoft.AspNetCore.Mvc;
using NewsProject.Models;
using NewsProject.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

[ApiController]
[Route("api/")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet("News")]
    public async Task<IActionResult> GetNewsTitles()
    {
        var newsList = await _newsService.GetNewsTitles();
        return Ok(newsList);
    }

    [HttpGet("News/{id}")]
    public async Task<IActionResult> GetNewsDetailsById(int id)
    {
        var newsDetails = await _newsService.GetNewsDetailsById(id);

        return Ok(newsDetails);
    }

    [HttpGet("AllNews")]
    public async Task<IActionResult> GetNews()
    {
        var newsList = await _newsService.GetNews();
        return Ok(newsList);
    }
}
