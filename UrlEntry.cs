namespace UrlShortenerAPI
{
    public class UrlEntry
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = null!;
        public string ShortCode { get; set; } = null!;
    }
}
