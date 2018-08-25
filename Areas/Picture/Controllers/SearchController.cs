namespace PictIt.Areas.Picture.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using IdentityServer4.Extensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    using PictIt.Models;
    using PictIt.Repositories;
    using PictIt.Services;

    public class SearchDisplay
    {
        public Guid PictureId { get; set; }

        public string UserName { get; set; }

        public string Gender { get; set; }

        public ushort Age { get; set; }

        public string Emotion { get; set; }
    }

    public class Match
    {
        public string FaceId { get; set; }

        public IEnumerable<Candidate> Candidates { get; set; }
    }

    public class Candidate
    {
        public string PersonId { get; set; }

        public decimal Confidence { get; set; }
    }

    [Area("Picture")]
    public class SearchController : AuthorizedApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly CloudStorageAccount _storageAccount;
        private readonly PictureRepository _pictureRepository;
        private readonly SearchRepository _searchRepository;

        public SearchController(UserManager<User> userManager, IConfiguration config, PictureRepository pictureRepository, SearchRepository searchRepository)
        {
            _userManager = userManager;
            _config = config;
            _storageAccount = CloudStorageAccount.Parse(config.GetConnectionString("BlobStorage"));
            _pictureRepository = pictureRepository;
            _searchRepository = searchRepository;
        }

        [HttpGet]
        public async Task<List<SearchDisplay>> GetUserSearches()
        {
            var searchesDisplay = new List<SearchDisplay>();

            string name = HttpContext.User.GetDisplayName();
            User user = null;

            if (name.Length > 0)
                user = await _userManager.FindByNameAsync(name);

            if (user == null)
            {
                string userId = HttpContext.User.GetSubjectId();
                user = await _userManager.FindByIdAsync(userId);
            }

            var searches = await _searchRepository.SearchFor(s => s.UserId == user.Id);

            foreach (var search in searches)
            {
                var picture = await _pictureRepository.GetById(search.PictureId);
                var pictureUser = await _userManager.FindByIdAsync(picture.UserId.ToString());
                string username = pictureUser.UserName;
                if (username.Length == 0)
                    username = pictureUser.FirstName + " " + pictureUser.LastName;

                decimal highestEmotion = 0;
                string emotion = "";

                if (picture.Face.FaceAttributes.Emotion.Anger > highestEmotion)
                {
                    highestEmotion = picture.Face.FaceAttributes.Emotion.Anger;
                    emotion = "anger";
                }

                if (picture.Face.FaceAttributes.Emotion.Contempt > highestEmotion)
                {
                    highestEmotion = picture.Face.FaceAttributes.Emotion.Contempt;
                    emotion = "contempt";
                }

                if (picture.Face.FaceAttributes.Emotion.Disgust > highestEmotion)
                {
                    highestEmotion = picture.Face.FaceAttributes.Emotion.Disgust;
                    emotion = "disgust";
                }

                if (picture.Face.FaceAttributes.Emotion.Fear > highestEmotion)
                {
                    highestEmotion = picture.Face.FaceAttributes.Emotion.Fear;
                    emotion = "fear";
                }

                if (picture.Face.FaceAttributes.Emotion.Happiness > highestEmotion)
                {
                    highestEmotion = picture.Face.FaceAttributes.Emotion.Happiness;
                    emotion = "happiness";
                }

                if (picture.Face.FaceAttributes.Emotion.Neutral > highestEmotion)
                {
                    highestEmotion = picture.Face.FaceAttributes.Emotion.Neutral;
                    emotion = "neutral";
                }

                if (picture.Face.FaceAttributes.Emotion.Sadness > highestEmotion)
                {
                    highestEmotion = picture.Face.FaceAttributes.Emotion.Sadness;
                    emotion = "sadness";
                }

                if (picture.Face.FaceAttributes.Emotion.Surprise > highestEmotion)
                {
                    highestEmotion = picture.Face.FaceAttributes.Emotion.Surprise;
                    emotion = "surprise";
                }

                var searchDisplay = new SearchDisplay
                                        {
                                            PictureId = picture.Id,
                                            UserName = username,
                                            Age = picture.Face.FaceAttributes.Age,
                                            Gender = picture.Face.FaceAttributes.Gender,
                                            Emotion = emotion
                                        };
                searchesDisplay.Add(searchDisplay);
            }

            return searchesDisplay;
        }

        [HttpPost]
        public async Task<IActionResult> SearchForUser(IFormFile file)
        {
            string name = HttpContext.User.GetDisplayName();
            User user = null;

            if (name.Length > 0)
                user = await _userManager.FindByNameAsync(name);

            if (user == null)
            {
                string userId = HttpContext.User.GetSubjectId();
                user = await _userManager.FindByIdAsync(userId);
            }

            // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
            var cloudBlobClient = _storageAccount.CreateCloudBlobClient();

            // Create a container called 'quickstartblobs' and append a GUID value to it to make the name unique. 
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("pictures");
            await cloudBlobContainer.CreateIfNotExistsAsync();

            // Set the permissions so the blobs are public. 
            var permissions = new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob };
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["FaceApi:Key"]);

                var stream = file.OpenReadStream();

                var resultPicture = await client.PostAsync("https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true&returnFaceAttributes=age,gender,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise", stream, new BinaryMediaTypeFormatter());

                if (resultPicture.IsSuccessStatusCode)
                {
                    try
                    {
                        var faces = await resultPicture.Content.ReadAsAsync<IEnumerable<Face>>();

                        var request = new
                                          {
                                              LargePersonGroupId = "pictit_users",
                                              FaceIds = new string[] { faces.First().FaceId },
                                              MaxNumOfCandidatesReturned = 1
                                          };

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var result = await client.PostAsync("https://westeurope.api.cognitive.microsoft.com/face/v1.0/identify", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
                        if (result.IsSuccessStatusCode)
                        {
                            var matches = await result.Content.ReadAsAsync<IEnumerable<Match>>();

                            if (matches.Any())
                            {
                                if (matches.First().Candidates.Any())
                                {
                                    var personId = Guid.Parse(matches.First().Candidates.First().PersonId);
                                    var matchingUser = await _userManager.Users?.SingleOrDefaultAsync(u => u.PersonId == personId);

                                    var picture = new Picture
                                                      {
                                                          UserId = matchingUser.Id, 
                                                          Face = faces.First()
                                                      };

                                    bool pictureInsertionSucceeded = await _pictureRepository.Insert(picture);

                                    if (pictureInsertionSucceeded)
                                    {
                                        await _pictureRepository.Save();

                                        var search = new Search
                                        {
                                            UserId = user.Id,
                                            PictureId = picture.Id,
                                            Date = DateTime.Now
                                        };

                                        bool searchInsertionSucceeded = await _searchRepository.Insert(search);

                                        if (searchInsertionSucceeded)
                                        {
                                            await _searchRepository.Save();

                                            var fileName = picture.Id + MimeTypeMap.GetExtension(file.ContentType);
                                            System.Console.WriteLine(fileName);
                                            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                                            await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

                                            stream = file.OpenReadStream();

                                            await client.PostAsync($"https://westeurope.api.cognitive.microsoft.com/face/v1.0/largepersongroups/pictit_users/persons/{user.PersonId}/persistedfaces", stream, new BinaryMediaTypeFormatter());

                                            var resultTrainingStatus = await client.GetAsync("https://westeurope.api.cognitive.microsoft.com/face/v1.0/largepersongroups/pictit_users/training");
                                            if (resultTrainingStatus.IsSuccessStatusCode)
                                            {
                                                string answer = await resultTrainingStatus.Content.ReadAsStringAsync();
                                                if (answer.IndexOf("running", StringComparison.Ordinal) == -1)
                                                {
                                                    await client.PostAsync("https://westeurope.api.cognitive.microsoft.com/face/v1.0/largepersongroups/pictit_users/train", null);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            return Ok();
        }
    }
}
