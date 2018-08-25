namespace PictIt.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    using PictIt.Models;
    using PictIt.Repositories;
    using PictIt.Services;

    public class Person
    {
        public string PersonId { get; set; }
    }

    [Area("User")]
    public class PictureController : AnonymousApiController
    {
        private readonly IConfiguration _config;
        private readonly PictureRepository _pictureRepository;
        private readonly CloudStorageAccount _storageAccount;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PictureController> _logger;

        public PictureController(IConfiguration config, PictureRepository pictureRepository, UserManager<User> userManager, ILogger<PictureController> logger)
        {
            _config = config;
            _pictureRepository = pictureRepository;
            _storageAccount = CloudStorageAccount.Parse(config.GetConnectionString("BlobStorage"));
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadPictures(IFormFileCollection files, [FromForm] string userId)
        {
            if (files.Count == 0)
                return Content("file not selected");

            // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
            var cloudBlobClient = _storageAccount.CreateCloudBlobClient();

            // Create a container called 'quickstartblobs' and append a GUID value to it to make the name unique. 
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("pictures");
            await cloudBlobContainer.CreateIfNotExistsAsync();

            // Set the permissions so the blobs are public. 
            var permissions = new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob };
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            var user = await _userManager.FindByIdAsync(userId);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config["FaceApi:Key"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var person = new { Name = user.Email };

                var result = await client.PostAsync("https://westeurope.api.cognitive.microsoft.com/face/v1.0/largepersongroups/pictit_users/persons", new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json"));

                if (result.IsSuccessStatusCode)
                {
                    try
                    {
                        var createdPerson = await result.Content.ReadAsAsync<Person>();

                        user.PersonId = Guid.Parse(createdPerson.PersonId);
                        await _userManager.UpdateAsync(user);

                        foreach (var file in files)
                        {
                            var stream = file.OpenReadStream();

                            var resultPicture = await client.PostAsync("https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true&returnFaceAttributes=age,gender,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise", stream, new BinaryMediaTypeFormatter());

                            if (resultPicture.IsSuccessStatusCode)
                            {
                                try
                                {
                                    var faces = await resultPicture.Content.ReadAsAsync<IEnumerable<Face>>();

                                    // Get a reference to the blob address, then upload the file to the blob.
                                    // Use the value of fileName for the blob name.
                                    var picture = new Picture
                                    {
                                        UserId = Guid.Parse(userId),
                                        Face = faces.First()
                                    };

                                    bool succeeded = await _pictureRepository.Insert(picture);

                                    if (!succeeded) continue;

                                    var fileName = picture.Id + MimeTypeMap.GetExtension(file.ContentType);
                                    System.Console.WriteLine(fileName);
                                    var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                                    await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

                                    stream = file.OpenReadStream();

                                    await client.PostAsync($"https://westeurope.api.cognitive.microsoft.com/face/v1.0/largepersongroups/pictit_users/persons/{user.PersonId}/persistedfaces", stream, new BinaryMediaTypeFormatter());
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }

                        await _pictureRepository.Save();

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
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            return Ok();
        }
    }
}
