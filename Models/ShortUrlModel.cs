namespace UrlShorteningService.Models
{
    public class ShortUrlModel
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? ShortUrl { get; set; }
    }
}
