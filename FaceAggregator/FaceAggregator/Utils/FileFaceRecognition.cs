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
    public class FileFaceRecognition : IFaceRecognition
    {

        private readonly string _subscriptionKey;

        public FileFaceRecognition(IConfiguration configuration)
        {
            this._subscriptionKey = configuration.GetValue("SubscriptionKey");
        }
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

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
