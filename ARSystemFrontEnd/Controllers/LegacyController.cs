using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("Legacy")]
    public class LegacyController : BaseController
    {
        // GET: HomeV1
        [Authorize]
        [Route("")]
        public ActionResult Index()
        {
            var returnUrl = Request.QueryString["returnUrl"];
            var config = Request.QueryString["config"];
            ViewBag.LegacyUrl = ConfigurationManager.AppSettings[config].ToString();
            ViewBag.ReturnURL = returnUrl;
            return View();
        }
    }
}