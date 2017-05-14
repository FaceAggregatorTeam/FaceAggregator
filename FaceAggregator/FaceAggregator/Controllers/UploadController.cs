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
        private string _emailAddress;
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
                ICollection<Uri> allBlobs = await _uploadService.GetAllBlobs(_emailAddress);
                return View(allBlobs);
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        public ActionResult UploadPerson()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadAsync()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                await _uploadService.UploadAsync(files, _emailAddress);
                
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
                await _uploadService.DeleteImage(filename, _emailAddress);
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
                await _uploadService.DeleteAllImages(_emailAddress);

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