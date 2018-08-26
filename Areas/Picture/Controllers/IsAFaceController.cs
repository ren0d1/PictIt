namespace PictIt.Areas.Picture.Controllers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [Area("Picture")]
    public class IsAFaceController : AnonymousApiController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<IsAFaceController> _logger;

        public IsAFaceController(IConfiguration config, ILogger<IsAFaceController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpPost]
        public async Task<bool> PictureContainsOnlyOneFace(IFormFile file)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["FaceApi:Key"]);

                var stream = file.OpenReadStream();

                var result = await client.PostAsync("https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false", stream, new BinaryMediaTypeFormatter());

                if (!result.IsSuccessStatusCode)
                    return false;

                string content = await result.Content.ReadAsStringAsync();
                return content.IndexOf("faceId", StringComparison.Ordinal) != -1 && content.IndexOf("faceId", StringComparison.Ordinal) == content.LastIndexOf("faceId", StringComparison.Ordinal);
            }
        }
    }
}
