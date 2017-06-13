using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using FaceAggregator.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FaceAggregator.Controllers
{
    /// <summary>
    /// Controller managing image upload process
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class UploadController : Controller
    {
        /// <summary>
        /// The images service
        /// </summary>
        private readonly IImagesService _imagesService;
        /// <summary>
        /// The account service
        /// </summary>
        private readonly IAccountService _accountService;
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadController"/> class.
        /// </summary>
        /// <param name="imagesService">The images service.</param>
        /// <param name="accountService">The account service.</param>
        public UploadController(IImagesService imagesService, IAccountService accountService)
        {
            _imagesService = imagesService;
            _accountService = accountService;
        }

        /// <summary>
        /// Uploads the photos.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> UploadPhotos()
        {
            try
            {
                ICollection<Uri> allPhotos = await _imagesService.GetAllPhotos(_accountService.GetContainerNamePhotos(ClaimsPrincipal.Current));
                return View(allPhotos);
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        /// <summary>
        /// Uploads the face.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> UploadFace()
        {
            try
            {
                ICollection<Uri> facePhotos = await _imagesService.GetAllPhotos(_accountService.GetContainerNameFace(ClaimsPrincipal.Current));
                return View(facePhotos.FirstOrDefault());
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        /// <summary>
        /// Uploads the face asynchronously.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UploadAsyncFace()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                string containerName = _accountService.GetContainerNameFace(ClaimsPrincipal.Current);
                await _imagesService.DeleteAllImages(containerName);
                await _imagesService.UploadAsync(files, containerName);

                return RedirectToAction("UploadFace");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        /// <summary>
        /// Uploads the photos asynchronously.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UploadAsyncPhotos()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                await _imagesService.UploadAsync(files, _accountService.GetContainerNamePhotos(ClaimsPrincipal.Current));
                
                return RedirectToAction("UploadPhotos");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        /// <summary>
        /// Deletes the image.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DeleteImage(string name)
        {
            try
            {
                Uri uri = new Uri(name);
                string filename = Path.GetFileName(uri.LocalPath);
                await _imagesService.DeleteImage(filename, _accountService.GetContainerNamePhotos(ClaimsPrincipal.Current));
                return RedirectToAction("UploadPhotos");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        /// <summary>
        /// Deletes all images.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DeleteAll()
        {
            try
            {
                await _imagesService.DeleteAllImages(_accountService.GetContainerNamePhotos(ClaimsPrincipal.Current));

                return RedirectToAction("UploadPhotos");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

    }
}