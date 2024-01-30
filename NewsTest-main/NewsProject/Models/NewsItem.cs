namespace NewsProject.Models
{
    public class NewsItem
    {
        public int Id { get; internal set; }
        public string Title { get; internal set; }
        public string Body { get; internal set; }
        public string? Link { get; internal set; }
        
    }
}
