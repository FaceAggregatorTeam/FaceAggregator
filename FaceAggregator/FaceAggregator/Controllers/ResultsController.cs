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
    /// <summary>
    /// Controller managing results view
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ResultsController : Controller
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
        /// Initializes a new instance of the <see cref="ResultsController"/> class.
        /// </summary>
        /// <param name="imagesService">The images service.</param>
        /// <param name="accountService">The account service.</param>
        public ResultsController(IImagesService imagesService, IAccountService accountService)
        {
            _imagesService = imagesService;
            _accountService = accountService;
        }

        /// <summary>
        /// Returns main view with all result phoros
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes all photos.
        /// </summary>
        /// <returns></returns>
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