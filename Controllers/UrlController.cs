using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UrlShortenerAPI.Model;
namespace UrlShortenerAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class UrlController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UrlController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("shorten")]
        public IActionResult Shorten([FromBody] UrlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.OriginalUrl))
                return BadRequest("URL is required");

            if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out var uriResult)
                || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                return BadRequest("Invalid URL format");
            }

            string code;
            do
            {
                code = GenerateShortCode();
            }
            while (_db.Urls.Any(u => u.ShortCode == code));

            var entry = new UrlEntry
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = code
            };

            _db.Urls.Add(entry);
            _db.SaveChanges();

            var shortUrl = $"{Request.Scheme}://{Request.Host}/{code}";

            return Ok(new { shortUrl });
        }

        [HttpGet("{code}")]
        public IActionResult RedirectToUrl(string code)
        {
            var found = _db.Urls.FirstOrDefault(u => u.ShortCode == code);

            if (found == null)
                return NotFound();

            return Redirect(found.OriginalUrl);
        }

        private static string GenerateShortCode(int length = 6)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
