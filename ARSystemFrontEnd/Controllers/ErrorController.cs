using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("Error")]
    public class ErrorController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        {
            ActionResult result;

            object model = Request.Url.PathAndQuery;
            Response.StatusCode = 500;
            if (!Request.IsAjaxRequest())
                result = View(model);
            else
                result = View();

            return result;
        }

        [Route("NotFound")]
        public ActionResult NotFound()
        {
            ActionResult result;

            object model = Request.Url.PathAndQuery;
            Response.StatusCode = 404;
            if (!Request.IsAjaxRequest())
                result = View(model);
            else
                result = View("NotFound", model);

            return result;
        }
    }
}