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
        private readonly IUploadService _uploadService;
        private readonly string _emailAddress;
        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
            var claim = ClaimsPrincipal.Current.Claims.FirstOrDefault(e => e.Type.Contains("emailaddress"));
            if (claim != null)
                _emailAddress = claim.Value;
        }

        public async Task<ActionResult> UploadPhotos()
        {
            try
            {
                ICollection<Uri> allPhotos = await _uploadService.GetAllPhotos(GetContainerNamePhotos(_emailAddress));
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
                ICollection<Uri> facePhotos = await _uploadService.GetAllPhotos(GetContainerNameFace(_emailAddress));
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
                await _uploadService.DeleteAllImages(GetContainerNameFace(_emailAddress));
                await _uploadService.UploadAsync(files, GetContainerNameFace(_emailAddress));

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
                await _uploadService.UploadAsync(files, GetContainerNamePhotos(_emailAddress));
                
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
                await _uploadService.DeleteImage(filename, GetContainerNamePhotos(_emailAddress));
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
                await _uploadService.DeleteAllImages(GetContainerNamePhotos(_emailAddress));

                return RedirectToAction("UploadPhotos");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        private string GetContainerNamePhotos(string emailAddress)
        {
            string result = emailAddress.Replace('.', '-').Replace('@', '-') + "-photos";
            return result.ToLower();
        }

        private string GetContainerNameFace(string emailAddress)
        {
            string result = emailAddress.Replace('.', '-').Replace('@', '-') + "-face";
            return result.ToLower();
        }

    }
}