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
using ARSystem.Domain.Models;
using ARSystem.Service;
using static ARSystem.Service.Constants;
using ARSystem.Service.RASystem.DashboardRA.DashboardTSEL;
using OfficeOpenXml;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("DashboardTSEL")]
    public class DashboardTSELController : BaseController
    {
        private readonly DashboardTSELService summary;
        private readonly InputTargetService _inputTargetService;
        
        public DashboardTSELController()
        {
            summary = new DashboardTSELService();
            _inputTargetService = new InputTargetService();
        }

        #region Routes
        [Authorize]
        [Route("Setting")]
        public ActionResult Setting()
        {
            string actionTokenView = "313F5FEB-C397-4E63-987A-24DAF57589B7";
            string actionTokenTabTsel = "dc93b316-84d2-48ce-be57-6c3fcd07864b";
            string actionTokenTabNonTsel = "10e3495f-817b-4e5e-93f8-01ccbf13a65c";
            string actionTokenTabNewBaps = "1f37b842-60c7-4fd1-bf07-2623109d8f1a";
            string actionTokenTabNewProduct = "235d799a-77d1-4bd5-b63f-6d803f786657";

            var allowToInputFrom = _inputTargetService.GetConstants("InputTarget.StartInputDate");
            var allowToInputTill = _inputTargetService.GetConstants("InputTarget.EndInputDate");

            int _allowToInputFrom;
            int _allowToInputTo;
            ViewBag.IsAllowToInputTarget = true;
            ViewBag.IsAllowToInputTargetMessage = String.Empty;
            if(Int32.TryParse(allowToInputFrom, out _allowToInputFrom) && Int32.TryParse(allowToInputTill, out _allowToInputTo))
            {
                if(Convert.ToInt32(_allowToInputFrom) > Convert.ToInt32(_allowToInputTo))
                {
                    if (DateTime.Today.Day >= Convert.ToInt32(_allowToInputFrom) || DateTime.Today.Day <= Convert.ToInt32(_allowToInputTo))
                    {
                        ViewBag.IsAllowToInputTarget = true;
                    }
                    else
                    {
                        ViewBag.IsAllowToInputTarget = false;
                        ViewBag.IsAllowToInputTargetMessage = String.Format("Cannot Input Target, you will be able to input target within date {0} till date {1} at the next month", _allowToInputFrom, _allowToInputTo);
                    }
                }
                else
                {
                    if(DateTime.Today.Day >= Convert.ToInt32(_allowToInputFrom) && DateTime.Today.Day <= Convert.ToInt32(_allowToInputTo))
                    {
                        ViewBag.IsAllowToInputTarget = true;
                    }
                    else
                    {
                        ViewBag.IsAllowToInputTarget = false;
                        ViewBag.IsAllowToInputTargetMessage = String.Format("Cannot Input Target, you will be able to input target within date {0} till date {1} at the next month", _allowToInputFrom, _allowToInputTo);
                    }
                }
            }

            ViewBag.IsTselAccessible = false;
            ViewBag.IsNonTselAccessible = false;
            ViewBag.IsNewBapsAccessible = false;
            ViewBag.IsNewProductAccessible = false;
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }

                if(client.CheckUserAccess(UserManager.User.UserToken, actionTokenTabTsel))
                {
                    ViewBag.IsTselAccessible = true;
                }
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenTabNonTsel))
                {
                    ViewBag.IsNonTselAccessible = true;
                }
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenTabNewBaps))
                {
                    ViewBag.IsNewBapsAccessible = true;
                }
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenTabNewProduct))
                {
                    ViewBag.IsNewProductAccessible = true;
                }
            }


            ViewBag.DeptCodeTsel = RADepartmentCodeTabEnum.TSEL;
            ViewBag.DeptCodeNonTsel = RADepartmentCodeTabEnum.NonTSEL;
            ViewBag.DeptCodeNewBaps = RADepartmentCodeTabEnum.NewBaps;
            ViewBag.DeptCodeNewProduct = RADepartmentCodeTabEnum.NewProduct;


            return View();
        }

        [Authorize]
        [Route("UpdateInputTarget")]
        public ActionResult UpdateInputTarget(string targetID)
        {

            return PartialView("_FormInputTarget");
        }

        [Authorize]
        [Route("DashboardOverDue")]
        public ActionResult DashboardOverDue()
        {
            string actionTokenView = "D577F723-42F5-4D30-83EB-E0DB38B3BBBD";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("DashboardLeadTime")]
        public ActionResult DashboardLeadTime()
        {
            string actionTokenView = "82A9605E-A729-4D7E-A4DE-7544BC0A4171";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("Dashboard")]
        public ActionResult Dashboard()
        {
            string actionTokenView = "BA1CFB4A-D4C8-4C87-A5B3-F00A6158C2E8";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("DashboardOutstanding")]
        public ActionResult DashboardOutstanding()
        {
            string actionTokenView = "3E0473A8-2BAF-4B0E-897B-A5FA3CDBEDDA";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        #endregion
        // GET: DashboardTSEL

        #region Export
        [Route("ListDashboardTSEL/Export")]
        public void GetDashboardTSELToExport()
        {

            //Parameter
            vwRABapsSite post = new vwRABapsSite();
            post.CompanyInvoiceId = Request.QueryString["strCompanyInvoice"];
            post.CustomerID = Request.QueryString["strCustomerInvoice"];
            post.SectionProductId = Request.QueryString["strSection"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strSection"]);
            post.SowProductId = Request.QueryString["strSOW"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strSOW"]);
            post.YearBill = Request.QueryString["strBillingYear"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strBillingYear"]);
            post.RegionID = Request.QueryString["strRegional"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strRegional"]);
            post.ProvinceID = Request.QueryString["strProvince"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strProvince"]);
            post.ProductID = Request.QueryString["strTenantType"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strTenantType"]);
            post.TargetBaps = Request.QueryString["BapsType"];
            post.TargetPower = Request.QueryString["PowerType"];
            post.TargetPower = post.TargetPower == "" ? null : post.TargetPower;

            post.SONumber = Request.QueryString["schSONumber"];
            post.SiteID = Request.QueryString["schSiteID"];
            post.SiteName = Request.QueryString["schSiteName"];
            post.CustomerSiteID = Request.QueryString["schCustomerSiteID"];
            post.CustomerSiteName = Request.QueryString["schCustomerSiteName"];
            post.RegionName = Request.QueryString["schRegionName"];

            if (Request.QueryString["StartInvoiceDate"] != String.Empty)
            {
                var date = Convert.ToDateTime(Request.QueryString["StartInvoiceDate"]);
                if (date != DateTime.MinValue)
                {
                    post.StartInvoiceDate = date;
                }
            }
            if (Request.QueryString["EndInvoiceDate"] != String.Empty)
            {
                var date = Convert.ToDateTime(Request.QueryString["EndInvoiceDate"]);
                if (date != DateTime.MinValue)
                {
                    post.EndInvoiceDate = date;
                }
            }


            //Call Service
            List<vwRABapsSite> listDashboardTSEL = new List<vwRABapsSite>();

            var client = new DashboardTSELService();
            int intTotalRecord = client.GetTrxDashbordTSELDataCount(post);
            int intBatch = intTotalRecord / 50;
            List<vwRABapsSite> listDataPostedDashboardTSEL = new List<vwRABapsSite>();


            listDataPostedDashboardTSEL = client.GetTrxDashbordTSELDataToList(post, 0, 0, "");


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "SONumber", "SiteID", "SiteName", "CustomerSiteID", "CustomerSiteName", "CustomerID", "RegionName", "StartInvoiceDate", "EndInvoiceDate", "AmountIDR" };
            var reader = FastMember.ObjectReader.Create(listDataPostedDashboardTSEL.Select(i => new
            {
                i.SONumber,
                i.SiteID,
                i.SiteName,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.CustomerID,
                i.RegionName,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.AmountIDR
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ListDashboardTSEL", table);

        }


        [Route("ListDashboardNonTSEL/Export")]
        public void GetDashboardNonTSELToExport()
        {

            //Parameter
            vwRABapsSite post = new vwRABapsSite();
            post.CompanyInvoiceId = Request.QueryString["strCompanyInvoice"];
            post.CustomerID = Request.QueryString["strCustomerInvoice"];
            post.SectionProductId = Request.QueryString["strSection"] == String.Empty ? 0 : Convert.ToInt32(Request.QueryString["strSection"]);
            post.SowProductId = Request.QueryString["strSOW"] == String.Empty ? 0 : Convert.ToInt32(Request.QueryString["strSOW"]);
            post.YearBill = Request.QueryString["strBillingYear"] == String.Empty ? 0 : Convert.ToInt32(Request.QueryString["strBillingYear"]);
            post.RegionID = Request.QueryString["strRegional"] == String.Empty ? 0 : Convert.ToInt32(Request.QueryString["strRegional"]);
            post.ProvinceID = Request.QueryString["strProvince"] == String.Empty ? 0 : Convert.ToInt32(Request.QueryString["strProvince"]);
            post.ProductID = Request.QueryString["strTenantType"] == String.Empty ? 0 : Convert.ToInt32(Request.QueryString["strTenantType"]);
            post.TargetBaps = Request.QueryString["BapsType"];
            post.TargetPower = Request.QueryString["PowerType"];
            post.TargetPower = post.TargetPower == "" ? null : post.TargetPower;

            post.DepartmentType = RADepartmentTypeEnum.NonTSEL;

            post.SONumber = Request.QueryString["schSONumber"];
            post.SiteID = Request.QueryString["schSiteID"];
            post.SiteName = Request.QueryString["schSiteName"];
            post.CustomerSiteID = Request.QueryString["schCustomerSiteID"];
            post.CustomerSiteName = Request.QueryString["schCustomerSiteName"];
            post.RegionName = Request.QueryString["schRegionName"];

            if (Request.QueryString["StartInvoiceDate"] != String.Empty)
            {
                var date = Convert.ToDateTime(Request.QueryString["StartInvoiceDate"]);
                if (date != DateTime.MinValue)
                {
                    post.StartInvoiceDate = date;
                }
            }
            if (Request.QueryString["EndInvoiceDate"] != String.Empty)
            {
                var date = Convert.ToDateTime(Request.QueryString["EndInvoiceDate"]);
                if (date != DateTime.MinValue)
                {
                    post.EndInvoiceDate = date;
                }
            }

            //Call Service
            List<vwRABapsSite> listDashboardTSEL = new List<vwRABapsSite>();

            var client = new InputTargetService();
            int intTotalRecord = client.GetTrxTargetNonTSELDataCount(post);
            int intBatch = intTotalRecord / 50;
            List<vwRABapsSite> listDataPostedDashboardTSEL = new List<vwRABapsSite>();


            listDataPostedDashboardTSEL = client.GetTrxDashbordNonTSELDataToList(post, 0, 0, "");


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "SONumber", "SiteID", "SiteName", "CustomerSiteID", "CustomerSiteName", "CustomerID", "RegionName", "StartInvoiceDate", "EndInvoiceDate", "AmountIDR" };
            var reader = FastMember.ObjectReader.Create(listDataPostedDashboardTSEL.Select(i => new
            {
                i.SONumber,
                i.SiteID,
                i.SiteName,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.CustomerID,
                i.RegionName,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.AmountIDR
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ListDashboardNonTSEL", table);

        }

        [Route("ListDashboardNewBaps/Export")]
        public void GetDashboardNewBapsToExport()
        {

            //Parameter
            vwRABapsSite post = new vwRABapsSite();
            post.CompanyInvoiceId = Request.QueryString["strCompanyInvoice"];
            post.CustomerID = Request.QueryString["strCustomerInvoice"];
            post.SectionProductId = Request.QueryString["strSection"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strSection"]);
            post.RegionID = Request.QueryString["strRegional"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strRegional"]);
            post.ProvinceID = Request.QueryString["strProvince"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strProvince"]);
            post.ProductID = Request.QueryString["strTenantType"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strTenantType"]);
            post.TargetBaps = Request.QueryString["BapsType"];
            post.TargetPower = Request.QueryString["PowerType"];
            post.TargetPower = post.TargetPower == "" ? null : post.TargetPower;
            post.SONumber = Request.QueryString["schSONumber"];
            post.SiteID = Request.QueryString["schSiteID"];
            post.SiteName = Request.QueryString["schSiteName"];
            post.CustomerSiteID = Request.QueryString["schCustomerSiteID"];
            post.CustomerSiteName = Request.QueryString["schCustomerSiteName"];
            post.RegionName = Request.QueryString["schRegionName"];

            if (Request.QueryString["StartInvoiceDate"] != String.Empty)
            {
                var date = Convert.ToDateTime(Request.QueryString["StartInvoiceDate"]);
                if (date != DateTime.MinValue)
                {
                    post.StartInvoiceDate = date;
                }
            }
            if (Request.QueryString["EndInvoiceDate"] != String.Empty)
            {
                var date = Convert.ToDateTime(Request.QueryString["EndInvoiceDate"]);
                if (date != DateTime.MinValue)
                {
                    post.EndInvoiceDate = date;
                }
            }



            //Call Service
            List<vwRABapsSite> listDashboardTSEL = new List<vwRABapsSite>();

            var client = new InputTargetService();
            int intTotalRecord = client.GetTrxTargetNewBapsDataCount(post);
            int intBatch = intTotalRecord / 50;
            List<vwRABapsSite> listDataPosted = new List<vwRABapsSite>();


            listDataPosted = client.GetTrxTargetNewBapsToList(post, 0, 0, "");


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "SONumber", "SiteID", "SiteName", "CustomerSiteID", "CustomerSiteName", "CustomerID", "CompanyInvoiceId", "RegionName", "StartInvoiceDate", "EndInvoiceDate", "AmountIDR" };
            var reader = FastMember.ObjectReader.Create(listDataPosted.Select(i => new
            {
                i.SONumber,
                i.SiteID,
                i.SiteName,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.CustomerID,
                i.CompanyInvoiceId,
                i.RegionName,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.AmountIDR
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ListDashboardNewBaps", table);

        }


        [Route("ListDashboardProduct/Export")]
        public void GetDashboardProductToExport()
        {

            //Parameter
            vwRABapsSite post = new vwRABapsSite();
            post.CompanyInvoiceId = Request.QueryString["strCompanyInvoice"];
            post.CustomerID = Request.QueryString["strCustomerInvoice"];
            post.SectionProductId = Request.QueryString["strSection"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strSection"]);
            post.SowProductId = Request.QueryString["strSOW"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strSOW"]);
            post.YearBill = Request.QueryString["strBillingYear"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strBillingYear"]);
            post.RegionID = Request.QueryString["strRegional"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strRegional"]);
            post.ProvinceID = Request.QueryString["strProvince"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strProvince"]);
            post.ProductID = Request.QueryString["strTenantType"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strTenantType"]);
            post.TargetBaps = Request.QueryString["BapsType"];
            post.TargetPower = Request.QueryString["PowerType"];
            post.TargetPower = post.TargetPower == "" ? null : post.TargetPower;

            post.SONumber = Request.QueryString["schSONumber"];
            post.SiteID = Request.QueryString["schSiteID"];
            post.SiteName = Request.QueryString["schSiteName"];
            post.CustomerSiteID = Request.QueryString["schCustomerSiteID"];
            post.CustomerSiteName = Request.QueryString["schCustomerSiteName"];
            post.RegionName = Request.QueryString["schRegionName"];

            if (Request.QueryString["StartInvoiceDate"] != String.Empty)
            {
                var date = Convert.ToDateTime(Request.QueryString["StartInvoiceDate"]);
                if (date != DateTime.MinValue)
                {
                    post.StartInvoiceDate = date;
                }
            }
            if (Request.QueryString["EndInvoiceDate"] != String.Empty)
            {
                var date = Convert.ToDateTime(Request.QueryString["EndInvoiceDate"]);
                if (date != DateTime.MinValue)
                {
                    post.EndInvoiceDate = date;
                }
            }


            //Call Service
            List<vwRABapsSite> listDashboardTSEL = new List<vwRABapsSite>();

            var client = new InputTargetService();
            int intTotalRecord = client.GetTrxTargetNewProductDataCount(post);
            int intBatch = intTotalRecord / 50;
            List<vwRABapsSite> listDataPostedDashboardTSEL = new List<vwRABapsSite>();

            listDataPostedDashboardTSEL = client.GetTrxDashbordNewProductDataToList(post, 0, 0, "");

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "SONumber", "SiteID", "SiteName", "CustomerSiteID", "CustomerSiteName", "CustomerID", "CompanyInvoiceId", "RegionName", "StartInvoiceDate", "EndInvoiceDate", "AmountIDR" };
            var reader = FastMember.ObjectReader.Create(listDataPostedDashboardTSEL.Select(i => new
            {
                i.SONumber,
                i.SiteID,
                i.SiteName,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.CustomerID,
                i.CompanyInvoiceId,
                i.RegionName,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.AmountIDR

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ListDashboardNewProduct", table);
        }

        [Route("ListDashboardTargetHistory/Export")]
        public void GetDashboardTargetHistoryToExport()
        {

            //Parameter
            vwRABapsSite post = new vwRABapsSite();
            post.CompanyInvoiceId = Request.QueryString["strCompanyInvoice"];
            post.CustomerID = Request.QueryString["strCustomerInvoice"];
            post.SectionProductId = Request.QueryString["strSection"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strSection"]);
            post.RegionID = Request.QueryString["strRegional"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strRegional"]);
            post.ProvinceID = Request.QueryString["strProvince"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strProvince"]);
            post.ProductID = Request.QueryString["strTenantType"] == "" ? 0 : Convert.ToInt32(Request.QueryString["strTenantType"]);
            post.ReconcileType = Request.QueryString["BapsType"];
            post.PowerType = Request.QueryString["PowerType"];
            post.TargetPower = post.TargetPower == "" ? null : post.TargetPower;
            post.SONumber = Request.QueryString["schSONumber"];
            post.SiteID = Request.QueryString["schSiteID"];
            post.SiteName = Request.QueryString["schSiteName"];
            post.CustomerSiteID = Request.QueryString["schCustomerSiteID"];
            post.CustomerSiteName = Request.QueryString["schCustomerSiteName"];
            post.RegionName = Request.QueryString["schRegionName"];
            post.TargetYear = Request.QueryString["strYearTargetHistory"] == "" || Request.QueryString["strYearTargetHistory"] == "All" ? 0 : 
                Convert.ToInt32(Request.QueryString["strYearTargetHistory"]);    
            post.TargetMonth = Request.QueryString["strMonthTargetHistory"] == "" || Request.QueryString["strMonthTargetHistory"] == "All" ? 0 : 
                Convert.ToInt32(Request.QueryString["strMonthTargetHistory"]);
            post.DepartmentType = Request.QueryString["DepartmentCode"];
            List<string> strMultipleSONumber = null;
            var tes = Request.QueryString["strSONumberMultiple"] == "" ? Request.QueryString["strSONumberMultiple"] : null;
            //Call Service
            List<vwRABapsSite> listDashboardTSEL = new List<vwRABapsSite>();

            var client = new InputTargetService();
            int intTotalRecord = summary.GetHistoryRecurringListCount(post, strMultipleSONumber);
            int intBatch = intTotalRecord / 50;
            List<vwRABapsSite> listDataPosted = new List<vwRABapsSite>();


            listDataPosted = summary.GetHistoryRecurringToList(post, strMultipleSONumber, 0, 0);


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "SONumber", "SiteID", "SiteName", "CustomerSiteID", "CustomerSiteName", "CustomerID", "CompanyInvoiceId", "RegionName", "StartInvoiceDate", "EndInvoiceDate", "AmountIDR", "AmountUSD", "DepartmentType" };
            var reader = FastMember.ObjectReader.Create(listDataPosted.Select(i => new
            {
                i.SONumber,
                i.SiteID,
                i.SiteName,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.CustomerID,
                i.CompanyInvoiceId,
                i.RegionName,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.AmountIDR,
                i.AmountUSD,
                i.DepartmentType
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ListDashboardTargetHistory", table);

        }


        [Route("ExportDetailTargetSite")]
        public void ExportDetailTargetSite()
        {
            PostDashboardTSEL post = new PostDashboardTSEL();
            //Parameter
            post.YearBill = Request.QueryString["strYearBill"].ToString();
            post.MonthBill = Request.QueryString["strMonthBill"].ToString();
            post.BapsType = Request.QueryString["strBapsType"].ToString();
            post.Targets = Request.QueryString["strTargets"].ToString();
            post.MonthBillName = Request.QueryString["strMonthBillName"].ToString();
            post.PowerType = Request.QueryString["strPowerType"].ToString();
            post.SecName = Request.QueryString["strSecName"].ToString();
            post.SOWName = Request.QueryString["strSOWName"].ToString();

            string strWhereClause = " 1=1 ";

            switch (post.MonthBillName)
            {
                case "Jan":
                    post.MonthBillName = "January";
                    break;
                case "Feb":
                    post.MonthBillName = "February";
                    break;
                case "Mar":
                    post.MonthBillName = "March";
                    break;
                case "Apr":
                    post.MonthBillName = "April";
                    break;
                case "May":
                    post.MonthBillName = "May";
                    break;
                case "Jun":
                    post.MonthBillName = "June";
                    break;
                case "Jul":
                    post.MonthBillName = "July";
                    break;
                case "Aug":
                    post.MonthBillName = "August";
                    break;
                case "Sep":
                    post.MonthBillName = "September";
                    break;
                case "Oct":
                    post.MonthBillName = "October";
                    break;
                case "Nov":
                    post.MonthBillName = "November";
                    break;
                case "Dec":
                    post.MonthBillName = "December";
                    break;
                default:
                    post.MonthBillName = post.MonthBillName;
                    break;
            }

            if (!string.IsNullOrEmpty(post.YearBill))
                strWhereClause += " AND YearTarget = " + post.YearBill;

            if (!string.IsNullOrEmpty(post.MonthBill))
                strWhereClause += " AND MonthBill = " + post.MonthBill;

            if (!string.IsNullOrEmpty(post.BapsType))
                strWhereClause += " AND mstBapsTypeId = " + post.BapsType;

            if (!string.IsNullOrEmpty(post.MonthBillName))
                strWhereClause += " AND MonthBillName = '" + post.MonthBillName + "' ";

            if (!string.IsNullOrEmpty(post.PowerType))
                strWhereClause += " AND PowerTypeCode = '" + post.PowerType + "' ";

            if (!string.IsNullOrEmpty(post.Targets))
                strWhereClause += " AND Targets = " + post.Targets;

            if (!string.IsNullOrEmpty(post.SecName) && !string.IsNullOrWhiteSpace(post.SecName))
                strWhereClause += " AND SectionName = '" + post.SecName + "' ";

            if (!string.IsNullOrEmpty(post.SOWName) && !string.IsNullOrWhiteSpace(post.SOWName))
                strWhereClause += " AND SowName = '" + post.SOWName + "' ";

            var dataresult = summary.GetDetailTargetSite(strWhereClause);

            string[] fieldList = new string[] {
                        "RowIndex",
                        "SONumber",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CustomerID",
                        "RegionalName",
                        "ProvinceName",
                        "ResidenceName",
                        "PoNumber",
                        "MLANumber",
                        "StartBapsDate",
                        "EndBapsDate",
                        "Term",
                        "BapsType",
                        "CustomerInvoice",
                        "CompanyInvoice",
                        "Company",
                        "StipSiro",
                        "Currency",
                        "StartInvoiceDate",
                        "EndInvoiceDate",
                        "BaseLeasePrice",
                        "ServicePrice",
                        "DeductionAmount",
                        "TotalAmount"
                    };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataresult, fieldList);
            table.Load(reader);

            table.Columns["RowIndex"].ColumnName = "No";
            //table.Columns["SONumber"].ColumnName = "SO Number";
            //table.Columns["SiteID"].ColumnName = "Site ID";
            //table.Columns["SiteName"].ColumnName = "Site Name";
            //table.Columns["RegionalName"].ColumnName = "Regional";
            //table.Columns["StipCategory"].ColumnName = "Stip Category";
            //table.Columns["TotalCost"].ColumnName = "Total Amount Cost";
            //table.Columns["TotalRevenue"].ColumnName = "Total Amount Revenue";

            ExportToExcelHelper.Export("TargetList", table);

        }

        [Route("ExportDetailSite")]
        public void ExportDetailSite()
        {
            PostDashboardTSEL post = new PostDashboardTSEL();
            //Parameter
            post.YearBill = Request.QueryString["strYearBill"].ToString();
            post.MonthBill = Request.QueryString["strMonthBill"].ToString();
            post.BapsType = Request.QueryString["strBapsType"].ToString();
            post.Targets = Request.QueryString["strTargets"].ToString();
            post.MonthBillName = Request.QueryString["strMonthBillName"].ToString();
            post.PowerType = Request.QueryString["strPowerType"].ToString();
            post.SecName = Request.QueryString["strSecName"].ToString();
            post.SOWName = Request.QueryString["strSOWName"].ToString();

            string strWhereClause = " 1=1 ";

            switch (post.MonthBillName)
            {
                case "Jan":
                    post.MonthBillName = "January";
                    break;
                case "Feb":
                    post.MonthBillName = "February";
                    break;
                case "Mar":
                    post.MonthBillName = "March";
                    break;
                case "Apr":
                    post.MonthBillName = "April";
                    break;
                case "May":
                    post.MonthBillName = "May";
                    break;
                case "Jun":
                    post.MonthBillName = "June";
                    break;
                case "Jul":
                    post.MonthBillName = "July";
                    break;
                case "Aug":
                    post.MonthBillName = "August";
                    break;
                case "Sep":
                    post.MonthBillName = "September";
                    break;
                case "Oct":
                    post.MonthBillName = "October";
                    break;
                case "Nov":
                    post.MonthBillName = "November";
                    break;
                case "Dec":
                    post.MonthBillName = "December";
                    break;
                default:
                    post.MonthBillName = post.MonthBillName;
                    break;
            }

            if (!string.IsNullOrEmpty(post.YearBill))
                strWhereClause += " AND YearBill = " + post.YearBill;

            if (!string.IsNullOrEmpty(post.MonthBill))
                strWhereClause += " AND MonthBill = " + post.MonthBill;

            if (!string.IsNullOrEmpty(post.BapsType))
                strWhereClause += " AND mstBapsTypeId = " + post.BapsType;

            if (!string.IsNullOrEmpty(post.MonthBillName))
                strWhereClause += " AND MonthBillName = '" + post.MonthBillName + "' ";

            if (!string.IsNullOrEmpty(post.PowerType))
                strWhereClause += " AND PowerTypeCode = '" + post.PowerType + "' ";

            if (!string.IsNullOrEmpty(post.Targets))
                strWhereClause += " AND Targets = " + post.Targets;

            if (!string.IsNullOrEmpty(post.SecName))
                strWhereClause += " AND SectionName = '" + post.SecName + "' ";

            if (!string.IsNullOrEmpty(post.SOWName))
                strWhereClause += " AND SowName = '" + post.SOWName + "' ";

            var dataresult = summary.GetDetailSite(strWhereClause);

            string[] fieldList = new string[] {
                        "RowIndex",
                        "SONumber",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CustomerID",
                        "RegionalName",
                        "ProvinceName",
                        "ResidenceName",
                        "PoNumber",
                        "MLANumber",
                        "StartBapsDate",
                        "EndBapsDate",
                        "Term",
                        "BapsType",
                        "CustomerInvoice",
                        "CompanyInvoice",
                        "Company",
                        "StipSiro",
                        "Currency",
                        "StartInvoiceDate",
                        "EndInvoiceDate",
                        "BaseLeasePrice",
                        "ServicePrice",
                        "DeductionAmount",
                        "TotalAmount"
                    };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataresult, fieldList);
            table.Load(reader);

            table.Columns["RowIndex"].ColumnName = "No";
            //table.Columns["SONumber"].ColumnName = "SO Number";
            //table.Columns["SiteID"].ColumnName = "Site ID";
            //table.Columns["SiteName"].ColumnName = "Site Name";
            //table.Columns["RegionalName"].ColumnName = "Regional";
            //table.Columns["StipCategory"].ColumnName = "Stip Category";
            //table.Columns["TotalCost"].ColumnName = "Total Amount Cost";
            //table.Columns["TotalRevenue"].ColumnName = "Total Amount Revenue";

            ExportToExcelHelper.Export("AchievementList", table);

        }

        [Route("ExportDetailSiteOverdue")]
        public void ExportDetailSiteOverdue()
        {
            PostDashboardTSEL post = new PostDashboardTSEL();
            //Parameter
            post.YearBill = Request.QueryString["strYearBill"].ToString();
            post.SecName = Request.QueryString["strSectionName"].ToString();
            post.SOWName = Request.QueryString["strSowName"].ToString();
            post.IsOverdue = Request.QueryString["strIsOverdue"].ToString();

            string strWhereClause = " 1=1 ";

            if (!string.IsNullOrEmpty(post.YearBill))
                strWhereClause += " AND YearBill = " + post.YearBill;

            if (!string.IsNullOrEmpty(post.SecName))
                strWhereClause += " AND SectionName = '" + post.SecName + "'";

            if (!string.IsNullOrEmpty(post.SOWName))
                strWhereClause += " AND SowName = '" + post.SOWName + "'";
            if (!string.IsNullOrEmpty(post.IsOverdue))
            {
                if (post.IsOverdue == "Overdue")
                    strWhereClause += " AND LeadTime > " + 50;
                else
                    strWhereClause += " AND LeadTime <= " + 50;
            }
            else
                strWhereClause += " AND LeadTime > " + 50;

            List <vwRADetailSiteRecurring> dataresult = summary.GetDetailSite(strWhereClause);
            DataTable table = new DataTable();
            string[] fieldList = new string[] {
                        "RowIndex",
                        "SONumber",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CustomerID",
                        "RegionalName",
                        "ProvinceName",
                        "ResidenceName",
                        "PoNumber",
                        "MLANumber",
                        "StartBapsDate",
                        "EndBapsDate",
                        "Term",
                        "BapsType",
                        "PowerType",
                        "CustomerInvoice",
                        "CompanyInvoice",
                        "Company",
                        "StipSiro",
                        "Currency",
                        "StartInvoiceDate",
                        "EndInvoiceDate",
                        "BaseLeasePrice",
                        "ServicePrice",
                        "DeductionAmount",
                        "TotalAmount",
                        "RTIDone",
                        "BAPSConfirmDate"
                    };

            
            var reader = FastMember.ObjectReader.Create(dataresult.Select(i => new
            {
                i.RowIndex ,
                i.SONumber ,
                i.SiteID ,
                i.SiteName ,
                i.CustomerSiteID ,
                i.CustomerSiteName ,
                i.CustomerID ,
                i.RegionalName ,
                i.ProvinceName ,
                i.ResidenceName ,
                i.PoNumber ,
                i.MLANumber ,
                i.StartBapsDate ,
                i.EndBapsDate ,
                i.Term ,
                i.BapsType ,
                PowerType = i.PowerTypeCode ,
                i.CustomerInvoice ,
                i.CompanyInvoice ,
                i.Company ,
                i.StipSiro ,
                i.Currency ,
                i.StartInvoiceDate ,
                i.EndInvoiceDate ,
                i.BaseLeasePrice ,
                i.ServicePrice ,
                i.DeductionAmount ,
                i.TotalAmount ,
                RTIDone = i.RTIDate ,
                BAPSConfirmDate = i.DateConfirm 

            }), fieldList);
            //var reader = FastMember.ObjectReader.Create(dataresult, fieldList);

            table.Load(reader);

            table.Columns["RowIndex"].ColumnName = "No";

            ExportToExcelHelper.Export("OverdueList", table);

        }


        [Route("ExportDetailSiteOutstanding")]
        public void ExportDetailSiteOutstanding()
        {
            DashboardTSELOutstandingService client = new DashboardTSELOutstandingService();
            PostDashboardTSEL post = new PostDashboardTSEL();
            //Parameter
            post.type = Request.QueryString["strtype"].ToString();
            post.paramRow = Request.QueryString["strparamRow"].ToString();
            post.paramColumn = Request.QueryString["strparamColumn"].ToString();
            if (Request.QueryString["strCompanyID"] != "")
                post.CompanyID = Request.QueryString["strCompanyID"].ToString();
            if (Request.QueryString["strRegionalID"] != "")
                post.RegionalID = Convert.ToInt32(Request.QueryString["strRegionalID"].ToString());
            if (Request.QueryString["strSTIPID"] != "")
                post.STIPID = Convert.ToInt32(Request.QueryString["strSTIPID"].ToString());
            if (Request.QueryString["strProductID"] != "")
                post.ProductID = Convert.ToInt32(Request.QueryString["strProductID"].ToString());
            if (Request.QueryString["strSOWID"] != "")
                post.SOWID = Convert.ToInt32(Request.QueryString["strSOWID"].ToString());
            if (Request.QueryString["strSectionID"] != "")
                post.SectionID = Convert.ToInt32(Request.QueryString["strSectionID"].ToString());
            if (Request.QueryString["strRFIDate"] != "")
                post.RFIDate = Convert.ToInt32(Request.QueryString["strRFIDate"].ToString());
            if (Request.QueryString["strSTIPDate"] != "")
                post.STIPDate = Convert.ToInt32(Request.QueryString["strSTIPDate"].ToString());

            string strWhereClause = " 1=1 ";

            if (post.paramRow != "")
            {
                if (post.type == "GroupBySection")
                    strWhereClause += " AND SectionName = '" + post.paramRow + "'";
                else if (post.type == "GroupBySOW")
                    strWhereClause += " AND SowName = '" + post.paramRow + "'";
                else if (post.type == "GroupByProduct")
                    strWhereClause += " AND ProductName = '" + post.paramRow + "'";
                else if (post.type == "GroupBySTIPCategory")
                    strWhereClause += " AND StipCategory = '" + post.paramRow + "'";
                else if (post.type == "GroupByRegional")
                    strWhereClause += " AND RegionName = '" + post.paramRow + "'";
                else if (post.type == "GroupByCompany")
                    strWhereClause += " AND CompanyInvoiceName = '" + post.paramRow + "'";
            }
            if (post.paramColumn != "")
            {
                strWhereClause += " AND YearInvoiceCategory = '" + post.paramColumn + "'";
            }
            if (post.STIPDate != null)
                strWhereClause += " AND YEAR(StipDate) = '" + post.STIPDate.ToString() + "'";
            if (post.RFIDate != null)
                strWhereClause += " AND YEAR(RFIDate) = '" + post.RFIDate.ToString() + "'";
            if (post.SectionID != null)
                strWhereClause += " AND SectionProductId = " + post.SectionID;
            if (post.SOWID != null)
                strWhereClause += " AND SowProductId = " + post.SOWID;
            if (post.ProductID != null)
                strWhereClause += " AND ProductID = " + post.ProductID;
            if (post.STIPID != null)
                strWhereClause += " AND STIPID = " + post.STIPID;
            if (post.RegionalID != null)
                strWhereClause += " AND RegionID = " + post.RegionalID;
            if (post.CompanyID != null)
                strWhereClause += " AND CompanyInvoice = '" + post.CompanyID + "'";


            var dataresult = client.GetDetailList(strWhereClause, 0, 0, "");

            string[] fieldList = new string[] {

                        "SoNumber",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CustomerID",
                        "RegionName",
                        "ProvinceName",
                        "ResidenceName",
                        "RFIDate",
                        "FirstBAPSDone",
                        "StipCategory",
                        "PONumber",
                        "MLANumber",
                        "StartBapsDate",
                        "EndBapsDate",
                        "Term",
                        "BapsType",
                        "CustomerInvoice",
                        "CompanyInvoice",
                        "Company",
                        "StipSiro",
                        "Currency",
                        "StartInvoiceDate",
                        "EndInvoiceDate",
                        "BaseLeasePrice",
                        "ServicePrice",
                        "DeductionAmount",
                        "TotalAmount"
                    };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataresult, fieldList);
            table.Load(reader);

            ExportToExcelHelper.Export("OutstandingDetailList", table);

        }
        #endregion

        #region UPLOAD TEMPALTE
        public ActionResult DownloadUploadInputTargetTemplate()
        {
            byte[] downloadBytes;
            using (var package = CreateUploadInputTargetTemlpate())
            {
                downloadBytes = package.GetAsByteArray();
            }
            string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(downloadBytes, XlsxContentType, "TBG - Upload Input Target.xlsx");
        }
        [NonAction]
        private ExcelPackage CreateUploadInputTargetTemlpate()
        {
            var mstBapsType = new List<ARSystemService.mstBapsType>();
            using (var client = new ARSystemService.ImstDataSourceServiceClient())
            {
                mstBapsType = client.GetBapsTypeToList(UserManager.User.UserToken, "")
                    .Where(m => m.mstBapsTypeId == 2 ||
                           m.mstBapsTypeId == 3 ||
                           m.mstBapsTypeId == 5).ToList();
            }
            var mstPowerType = new List<ARSystemService.mstBapsPowerType>();
            using (var client = new ARSystemService.ImstDataSourceServiceClient())
            {
                mstPowerType = client.GetBapsPowerTypeToList(UserManager.User.UserToken, "PowerType ASC").ToList();
            }

            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Input Target Inflasi Upload Template";
            package.Workbook.Properties.Author = "Tower Bersama Group";
            package.Workbook.Properties.Subject = "Data Import Template";
            package.Workbook.Properties.Keywords = "TBG - Upload Input Target";

            var worksheet = package.Workbook.Worksheets.Add("Upload Input Target");

            // First: add the header
            worksheet.Cells[1, 1].Value = "SoNumber";
            worksheet.Column(1).Style.Numberformat.Format = "@";
            worksheet.Cells[1, 1].AddComment("Please remind to input as a text format, some value can be changed while use numeric format.","ArSystem");
            worksheet.Cells[1, 2].Value = "Start Invoice Date";
            worksheet.Cells[1, 2].AddComment("Please remind to input using 'M/D/YYYY' date format", "ArSystem");
            worksheet.Cells[1, 3].Value = "End Invoice Date";
            worksheet.Cells[1, 3].AddComment("Please remind to input using 'M/D/YYYY' date format", "ArSystem");
            worksheet.Cells[1, 4].Value = "Amuont IDR";
            worksheet.Cells[1, 4].AddComment("Please remind to input as numeric", "ArSystem");
            worksheet.Cells[1, 5].Value = "Amuont USD";
            worksheet.Cells[1, 5].AddComment("Please remind to input as numeric", "ArSystem");

            worksheet.Cells[1, 6].Value = "Month Target";
            worksheet.Cells[1, 6].AddComment("Please remind to input as number of month (1- 12)", "ArSystem");

            worksheet.Cells[1, 7].Value = "Year Target";
            worksheet.Cells[1, 7].AddComment("Please remind to input as number of year", "ArSystem");
            worksheet.Cells[1, 8].Value = "BAPS Type";
            var bapsTypesName = String.Empty;
            foreach (var type in mstBapsType)
            {
                if(bapsTypesName != String.Empty)
                {
                    bapsTypesName += ", ";
                }
                bapsTypesName += type.BapsType;
            }
            worksheet.Cells[1, 8].AddComment(String.Format("the following data for baps type : {0}", bapsTypesName), "ArSystem");
            worksheet.Cells[1, 9].Value = "Power Type";
            var powerTypesName = String.Empty;
            foreach (var type in mstPowerType)
            {
                if (powerTypesName != String.Empty)
                {
                    powerTypesName += ", ";
                }
                powerTypesName += type.PowerType;
            }
            worksheet.Cells[1, 9].AddComment(String.Format("the following data for power type : {0}", powerTypesName), "ArSystem");

            worksheet.Cells["A1:ZZ1"].Style.Font.Bold = true;
            worksheet.Cells["A1:ZZ1"].AutoFitColumns();
            return package;
        }
        #endregion
    }

}