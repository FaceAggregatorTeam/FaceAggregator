using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace FaceAggregator.Utils
{
    /// <summary>
    /// Class for file face recognition
    /// </summary>
    /// <seealso cref="FaceAggregator.Utils.IFaceRecognition" />
    public class FileFaceRecognition : IFaceRecognition
    {

        /// <summary>
        /// The subscription key
        /// </summary>
        private readonly string _subscriptionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFaceRecognition"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public FileFaceRecognition(IConfiguration configuration)
        {
            this._subscriptionKey = configuration.GetValue("SubscriptionKey");
        }
        /// <summary>
        /// Detects the faces.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public async Task<Image> DetectFaces(string imagePath)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["returnFaceId"] = "true";
            string uri = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            HttpResponseMessage response;
            string responseContent;

            byte[] byteData = GetImageAsByteArray(imagePath);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }
            IList<Face> faces = JsonConvert.DeserializeObject<IList<Face>>(responseContent);
            return new Image() { Faces = faces, Id = new Guid(), Path = imagePath };
        }

        /// <summary>
        /// Gets the image as byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <returns></returns>
        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
