using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using Microsoft.Reporting.WebForms;
using ClosedXML.Excel;
using ARSystem.Domain.Models;
using ARSystem.Service;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("ManualBook")]
    public class ManualBookController : BaseController
    {
        [Authorize]
        [Route("Download")]
        public ActionResult ManualBook()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            return View();
        }

        [Authorize]
        [Route("Download/Document/{intProjectID}")]
        public ActionResult DownloadManualBook(int intProjectID)
        {
            //int UniqueID = Convert.ToInt32(Request.QueryString["UniqueID"]);

            vwProjectManualBook attachment = new vwProjectManualBook();
            attachment.ProjectID = intProjectID;

            attachment = ManualBookService.GetSingleManualBook(attachment);
            string filename = attachment.FileName;
            string filepath = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocumentTeamMatePath() + attachment.FilePath);
            string contentType = attachment.ContentType;

            return File(filepath, contentType, filename);
        }
    }
}
