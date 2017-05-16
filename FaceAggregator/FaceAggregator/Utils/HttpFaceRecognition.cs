using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace FaceAggregator.Utils
{
    public class HttpFaceRecognition : IFaceRecognition
    {
        private readonly string _subscriptionKey;

        public HttpFaceRecognition(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public async Task<Image> DetectFaces(string imagePath)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["returnFaceId"] = "true";

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            var uri = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            HttpResponseMessage response;

            string json = "{\r\n\"url\":\"" + imagePath + "\"\r\n}";
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await client.PostAsync(uri, content);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            IList<Face> faces = JsonConvert.DeserializeObject<IList<Face>>(responseContent);
            return new Image() {Faces = faces, Id = new Guid(), Path = imagePath};
        }
    }
}
