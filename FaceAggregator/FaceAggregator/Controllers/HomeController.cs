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
            return View();
        }
        [Authorize]
        public ActionResult Menu()
        {
            return View();
        }
        [Authorize]
        public ActionResult Results()
        {
            return View();
        }
    }
}