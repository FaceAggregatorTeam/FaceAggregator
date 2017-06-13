using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace FaceAggregator.Utils
{
    /// <summary>
    /// Class implementing IFaceSimilarity interface
    /// </summary>
    /// <seealso cref="FaceAggregator.Utils.IFaceSimilarity" />
    public class FaceSimilarity : IFaceSimilarity
    {
        /// <summary>
        /// The subscription key
        /// </summary>
        private readonly string _subscriptionKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaceSimilarity"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public FaceSimilarity(IConfiguration configuration)
        {
            _subscriptionKey = configuration.GetValue("SubscriptionKey");
        }
        /// <summary>
        /// Finds the similar face.
        /// </summary>
        /// <param name="facePatternId">The face pattern identifier.</param>
        /// <param name="images">The images.</param>
        /// <returns></returns>
        public async Task<IList<Image>> FindSimilar(string facePatternId, IList<Image> images)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            var uri = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/findsimilars?" + queryString;

            HttpResponseMessage response;
            var temp = new FaceRecognitionPatternDto();
            temp.faceId = facePatternId;
            temp.maxNumOfCandidatesReturned = 1000;
            temp.mode = "matchPerson";
            temp.faceIds = images.SelectMany(e => e.Faces).Select(e => e.FaceId).ToList();
            string json = JsonConvert.SerializeObject(temp);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await client.PostAsync(uri, content);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            IList<RecognitionResult> recognitionResults = JsonConvert.DeserializeObject<IList<RecognitionResult>>(responseContent);
            var recognizedFaces = recognitionResults.Select(e => e.faceId).ToList();
            return SelectRecognizedImages(images, recognizedFaces);
        }

        /// <summary>
        /// Selects the recognized images.
        /// </summary>
        /// <param name="imagesCollection">The images collection.</param>
        /// <param name="recognizedFacesIds">The recognized faces ids.</param>
        /// <returns></returns>
        private IList<Image> SelectRecognizedImages(IList<Image> imagesCollection, IList<string> recognizedFacesIds)
        {
            var imagesSource = imagesCollection.ToList();
            var result = new List<Image>();
            foreach (var faceId in recognizedFacesIds)
            {
                foreach (var image in imagesSource)
                {
                    if (image.Faces.Any(e => e.FaceId == faceId))
                    {
                        result.Add(image);
                        imagesSource.Remove(image);
                        break;
                    }
                }
            }
            return result;
        }
    }
}
