using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FaceAggregator.Services;

namespace FaceAggregator.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IImagesService _imagesService;
        private readonly IAccountService _accountService;
        public ResultsController(IImagesService imagesService, IAccountService accountService)
        {
            _imagesService = imagesService;
            _accountService = accountService;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                ICollection<Uri> allPhotos = await _imagesService.GetAllPhotos(_accountService.GetContainerNameResults(ClaimsPrincipal.Current));
                return View(allPhotos);
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
                await _imagesService.DeleteAllImages(_accountService.GetContainerNameResults(ClaimsPrincipal.Current));

                return RedirectToAction("Index");
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