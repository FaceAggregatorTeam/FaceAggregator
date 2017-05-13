using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaceAggregator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome page, used to log in";
            return View();
        }
        [Authorize]
        public ActionResult Menu()
        {
            ViewBag.Message = "Here is app main menu";
            return View();
        }
        [Authorize]
        public ActionResult Upload()
        {
            ViewBag.Message = "Here we upload out photos.";
            return View();
        }
        [Authorize]
        public ActionResult Person()
        {
            ViewBag.Message = "Here we upload template person photo.";

            return View();
        }
        [Authorize]
        public ActionResult Results()
        {
            ViewBag.Message = "Here we see results.";

            return View();
        }
    }
}