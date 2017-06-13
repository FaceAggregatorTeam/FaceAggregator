using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FaceAggregator.Services;
using FaceAggregator.Utils;

namespace FaceAggregator.Controllers
{
    /// <summary>
    /// Controller managing recognition process
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class RecognitionController : Controller
    {
        /// <summary>
        /// The recognition service
        /// </summary>
        private IRecognitionService _recognitionService;
        /// <summary>
        /// The images service
        /// </summary>
        private IImagesService _imagesService;
        /// <summary>
        /// The account service
        /// </summary>
        private IAccountService _accountService;
        /// <summary>
        /// Initializes a new instance of the <see cref="RecognitionController"/> class.
        /// </summary>
        /// <param name="recognitionService">The recognition service.</param>
        /// <param name="imagesService">The images service.</param>
        /// <param name="accountService">The account service.</param>
        public RecognitionController(IRecognitionService recognitionService, IImagesService imagesService, IAccountService accountService)
        {
            _recognitionService = recognitionService;
            _imagesService = imagesService;
            _accountService = accountService;
        }

        /// <summary>
        /// Starts the recognition.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> StartRecognition()
        {
            try
            {
                string containerName = _accountService.GetContainerNameResults(ClaimsPrincipal.Current);
                await _imagesService.DeleteAllImages(containerName);
                var patternImageUri = _imagesService.GetAllPhotos(_accountService.GetContainerNameFace(ClaimsPrincipal.Current)).Result.FirstOrDefault();
                var patternImage = await _recognitionService.DetectFaces(patternImageUri.AbsoluteUri);
                var allImagesUri = await _imagesService.GetAllPhotos(_accountService.GetContainerNamePhotos(ClaimsPrincipal.Current));
                var allImagesAddresses = allImagesUri.Select(e => e.AbsoluteUri);
                var allDetectedImages = await DetectionForImages(allImagesAddresses);
                var foundImages = await _recognitionService.FindSimilar(patternImage.Faces.FirstOrDefault().FaceId, allDetectedImages);
                await MoveToResultContainer(foundImages);
                return RedirectToAction("Index", "Results");
            }
            catch (Exception e)
            {
                return View("Error");
            }

        }

        /// <summary>
        /// Moves images to result container.
        /// </summary>
        /// <param name="foundImages">The found images.</param>
        /// <returns></returns>
        private async Task MoveToResultContainer(IList<Image> foundImages)
        {
            await _imagesService.UploadAsyncFromUri(foundImages,
                _accountService.GetContainerNameResults(ClaimsPrincipal.Current));
        }

        /// <summary>
        /// Detects faces for images.
        /// </summary>
        /// <param name="images">The images.</param>
        /// <returns></returns>
        private async Task<List<Image>> DetectionForImages(IEnumerable<string> images)
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