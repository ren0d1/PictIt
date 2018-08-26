namespace PictIt.Areas.Picture.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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

        public string Extension { get; set; }

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
                string emotion = string.Empty;

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
                                            Extension = picture.Extension,
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
                                                          Face = faces.First(),
                                                          Extension = MimeTypeMap.GetExtension(file.ContentType)
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

                                                    // Try custom CNN
                                                    try
                                                    {
                                                        byte[] bytes;
                                                        using (var memoryStream = new MemoryStream())
                                                        {
                                                            stream = file.OpenReadStream();
                                                            stream.CopyTo(memoryStream);
                                                            bytes = memoryStream.ToArray();
                                                        }

                                                        string base64 = Convert.ToBase64String(bytes);
                                                        var imgRequest = new { image = base64 };
                                                        var resultFromCustomCNN = await client.PostAsync("http://localhost:5002/predict", new StringContent(JsonConvert.SerializeObject(imgRequest), Encoding.UTF8, "application/json"));
                                                        if (resultFromCustomCNN.IsSuccessStatusCode)
                                                        {
                                                            var people = await resultFromCustomCNN.Content.ReadAsAsync<List<string>>();
                                                            decimal highestConfidence = 0;
                                                            string matchingPerson = string.Empty;

                                                            foreach (var person in people)
                                                            {
                                                                string personName = person.Substring(0, person.IndexOf(':') - 1);
                                                                decimal personConfidence = Convert.ToDecimal(person.Substring(person.IndexOf(':') + 2, person.Length - person.IndexOf(':') - 2));
                                                                if (personConfidence > highestConfidence)
                                                                {
                                                                    highestConfidence = personConfidence;
                                                                    matchingPerson = personName;
                                                                }
                                                            }

                                                            Guid matchingUserId;

                                                            if (matchingPerson.Contains("Alexandre"))
                                                                matchingUserId = Guid.Parse("f3711199-3eaa-44a2-2f30-08d60b5b7cbf");
                                                            else if (matchingPerson.Contains("Danielle"))
                                                                matchingUserId = Guid.Parse("e02ca51d-9357-4f79-a337-08d60b93a736");
                                                            else if (matchingPerson.Contains("Joachim"))
                                                                matchingUserId = Guid.Parse("f20f6dd8-7432-4510-4df1-08d60b9cd4dd");
                                                            else if (matchingPerson.Contains("Gian-luca"))
                                                                matchingUserId = Guid.Parse("20c9e863-b3f7-415f-4df2-08d60b9cd4dd");
                                                            else
                                                                matchingUserId = Guid.Parse("81919b28-63ee-449c-8851-08d60ad2c6e6");

                                                            var extraPicture = new Picture
                                                            {
                                                                UserId = matchingUserId,
                                                                Face = faces.First(),
                                                                Extension = MimeTypeMap.GetExtension(file.ContentType)
                                                            };

                                                            bool extraPictureInsertionSucceeded = await _pictureRepository.Insert(extraPicture);

                                                            if (extraPictureInsertionSucceeded)
                                                            {
                                                                await _pictureRepository.Save();

                                                                var extraSearch = new Search
                                                                {
                                                                    UserId = user.Id,
                                                                    PictureId = extraPicture.Id,
                                                                    Date = DateTime.Now
                                                                };

                                                                bool extraSearchInsertionSucceeded = await _searchRepository.Insert(extraSearch);
                                                                if (extraSearchInsertionSucceeded)
                                                                {
                                                                    await _searchRepository.Save();
                                                                    var extraFileName = picture.Id + MimeTypeMap.GetExtension(file.ContentType);
                                                                    System.Console.WriteLine(extraFileName);
                                                                    var extraCloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(extraFileName);
                                                                    await extraCloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        Console.WriteLine(e.Message);
                                                    }
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
