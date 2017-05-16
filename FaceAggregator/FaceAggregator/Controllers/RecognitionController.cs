using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FaceAggregator.Services;
using FaceAggregator.Utils;

namespace FaceAggregator.Controllers
{
    [Authorize]
    public class RecognitionController : Controller
    {
        private IRecognitionService _recognitionService;
        private IImagesService _imagesService;
        public RecognitionController(IRecognitionService recognitionService, IImagesService imagesService)
        {
            _recognitionService = recognitionService;
            _imagesService = imagesService;
        }
        
        [HttpPost]
        public async Task<ActionResult> StartRecognition()
        {
            var patternImageUri = _imagesService.GetAllPhotos("patternImageContainer").Result.FirstOrDefault();
            var patternImage =await _recognitionService.DetectFaces(patternImageUri.AbsoluteUri);
            var allImagesUri = await _imagesService.GetAllPhotos("photosContainer");
            var allImagesAddresses = allImagesUri.Select(e => e.AbsoluteUri);
            var allDetectedImages =await DetectionForImages(allImagesAddresses);
            var foundImages = await _recognitionService.FindSimilar(patternImage.Faces.FirstOrDefault().FaceId, allDetectedImages);
            MoveToResultContainer(foundImages);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void MoveToResultContainer(IList<Image> foundImages)
        {
            
        }

        public async Task<List<Image>> DetectionForImages(IEnumerable<string> images)
        {
            var result = new List<Image>();
            foreach (var filePath in images)
            {
                var image = await _recognitionService.DetectFaces(filePath);
                result.Add(image);
            }
            return result;
        }
    }
}