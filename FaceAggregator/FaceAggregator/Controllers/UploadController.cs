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
    [Authorize]
    public class UploadController : Controller
    {
        private readonly IImagesService _imagesService;
        private readonly IAccountService _accountService;
        public UploadController(IImagesService imagesService, IAccountService accountService)
        {
            _imagesService = imagesService;
            _accountService = accountService;
        }

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