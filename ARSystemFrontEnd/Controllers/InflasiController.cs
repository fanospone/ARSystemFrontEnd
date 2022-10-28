using System;
using System.Web.Mvc;
using System.Web;
using ARSystemFrontEnd.Providers;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using ARSystemFrontEnd.Models;
using System.Linq;
using FastMember;
using ARSystemFrontEnd.Helper;
using ARSystem.Service;
using ARSystem.Service.RevenueSystem;
using ARSystem.Domain.Models.ViewModels.RevenueSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using System.IO;

namespace ARSystemFrontEnd.Controllers
{
    //[Route("{action=Index}")]
    [RoutePrefix("Inflasi")]
    public class InflasiController : BaseController
    {
        private readonly InflasiService _IS;

        public InflasiController()
        {
            _IS = new InflasiService();
        }
        #region Routes
        [Authorize]
        [Route("SummaryInflasi")]
        public ActionResult SummaryInflasi()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            string actionToken = "322e6502-2eb3-4dc1-9277-539b71a41352";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionToken))
                {
                    ViewBag.UserFullNameLogin = UserManager.User.UserFullName;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View("~/Views/RevenueSystem/Inflasi.cshtml");
        }
        [Authorize]
        [Route("SummaryKurs")]
        public ActionResult SummaryKurs()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            string actionToken = "ae6f9432-4dc0-43da-9c25-b9bca05ab35b";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionToken))
                {
                    ViewBag.UserFullNameLogin = UserManager.User.UserFullName;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View("~/Views/RevenueSystem/Kurs.cshtml");
        }
        #endregion

        #region Export
        [Route("GridIR/Export")]
        public void GridIRExport(int periodYear)
        {
            GridInflasiParam param = new GridInflasiParam() { Year = periodYear };

            if (periodYear != 0)
            {
                param.Year = periodYear;
            }
            //Call Service
            List<mstRevSysInflationRate> IRDataList = _IS.GetInflationGridList(UserManager.User.UserToken, param);
            string[] fieldList = new string[] {
                        "Year",
                        "Percentage",
                        "FileName"
                    };
            //Convert to DataTable
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(IRDataList, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("InflationRateData", table);
        }
        [Route("GridSI/Export")]
        public void GridSIExport(string Sonumb, string SiteID, string SiteName, string SiteIDOpr, string SiteNameOpr, string CustInv, string ComInv, string Reg)
        {
            GridInflasiParam param = new GridInflasiParam();

            if (!string.IsNullOrEmpty(Sonumb))
            {
                param.Sonumb = Sonumb;
            }
            if (!string.IsNullOrEmpty(SiteID))
            {
                param.SiteID = SiteID;
            }
            if (!string.IsNullOrEmpty(SiteName))
            {
                param.SiteName = SiteName;
            }
            if (!string.IsNullOrEmpty(SiteIDOpr))
            {
                param.SiteIDOpr = SiteIDOpr;
            }
            if (!string.IsNullOrEmpty(SiteNameOpr))
            {
                param.SiteNameOpr = SiteNameOpr;
            }
            if (!string.IsNullOrEmpty(CustInv))
            {
                param.Customer = CustInv;
            }
            if (!string.IsNullOrEmpty(ComInv))
            {
                param.Company = ComInv;
            }
            if (!string.IsNullOrEmpty(Reg))
            {
                param.Regional = Reg;
            }
            //Call Service
            List<vwRevSysSonumbInflasiList> IRDataList = _IS.GetSonumbInflasiGridList(UserManager.User.UserToken, param);
            string[] fieldList = new string[] {
                        "Sonumb",
                        "SiteID",
                        "SiteName",
                        "OperatorID",
                        "SiteNameOpr",
                        "CustomerInvoice",
                        "CompanyInvoice",
                        "RegionalName",
                        "AmountRental",
                        "AmountService",
                        "AmountInflation",
                        "InflationRate",
                        "Status"
                    };
            //Convert to DataTable
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(IRDataList, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("SonumbInflationData", table);
        }
        [Route("GridKurs/Export")]
        public void GridKursExport(DateTime? StartDate, DateTime? EndDate)
        {
            GridInflasiParam param = new GridInflasiParam();

            if (StartDate != null)
            {
                param.StartDate = StartDate;
            }
            if (EndDate != null)
            {
                param.EndDate = EndDate;
            }
            //Call Service
            List<mstRevSysKurs> IRDataList = _IS.GetKursGridList(UserManager.User.UserToken, param);
            string[] fieldList = new string[] {
                        "StartDate",
                        "EndDate",
                        "Kurs",
                        "FileName"
                    };
            //Convert to DataTable
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(IRDataList, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("KursIDRData", table);
        }
        #endregion

        #region Download
        [Route("DownloadAttachmentIR")]
        public ActionResult DownloadInput(string FilePath, string ContentType, string OriginalFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    FilePath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString()) + OriginalFileName;
                }
                else
                {
                    FilePath = $@"{FilePath}";
                }
                //var attach = _trxService.GetAttactmentInput(UserManager.User.UserToken, trxID, squenceNumber);
                string contentType = ContentType;

                //return File(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, OriginalFileName);
                return File(FilePath, contentType, OriginalFileName);
            }
            catch
            {
                return HttpNotFound();
            }
        }
        #endregion
    }
}