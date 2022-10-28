using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using System.IO;
using System.Web;
using System.Collections.Specialized;

namespace ARSystemFrontEnd.ControllerApis.RevenueAssurance
{
    [RoutePrefix("api/CustomReport")]
    public class ApiCustomReportController : ApiController
    {
        TemplateService client = new TemplateService();

        [HttpPost, Route("GetReport")] //Kepake
        public IHttpActionResult GetReport()
        {
            try
            {
                var list = client.GetReport().OrderBy(o => o.CustomerID).ToList();

                return Ok(new { data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SaveReport")] //Kepake
        public IHttpActionResult SaveReport(ARSystem.Domain.Models.mstRAGeneratorPDF post)
        {
            try
            {
                var rst = client.SaveReport(post);

                return Ok(new { data = rst });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("UploadFile")]
        public IHttpActionResult UploadFile()
        {
            try
            {
                NameValueCollection nvc = HttpContext.Current.Request.Form;

                HttpPostedFile LogoRight = HttpContext.Current.Request.Files["LogoRight"];
                HttpPostedFile LogoLeft = HttpContext.Current.Request.Files["LogoLeft"];

                string LogoRights = LogoRight == null ? "" : MapModel(LogoRight);
                string LogoLefts = LogoLeft == null ? "" : MapModel(LogoLeft);

                return Ok(new { LogoRight = LogoRights, LogoLeft = LogoLefts });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string MapModel(HttpPostedFile postedFile)
        {
            string BasePath = "~\\Content\\Images\\";
            string filePath = BasePath;

            if (postedFile != null)
            {
                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);


                string path = System.Web.HttpContext.Current.Server.MapPath(filePath);
                filePath = path + "ImgTemplate" + fileTimeStamp;
                BasePath = BasePath + "ImgTemplate" + fileTimeStamp;


                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(filePath);
            }
            return BasePath;
        }
    }
}