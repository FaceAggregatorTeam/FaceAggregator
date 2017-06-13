using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaceAggregator.Controllers
{
    /// <summary>
    /// Main view controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// Not logged in action
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Main view when logged in
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult LoggedInIndex()
        {
            return View("Index");
        }

    }
}