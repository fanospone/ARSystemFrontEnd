using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        [Route("~/")]
        [Route("")]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }
    }
}