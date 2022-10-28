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
    [Route("{action=Index}")]
    [RoutePrefix("Accrue")]
    public class AccrueController : BaseController
    {
        #region routes
        [Authorize]
        [Route("SettingAutoConfirm")]
        public ActionResult SettingAutoConfirm()
        {
            string actionTokenView = "b582b840-1c1a-4b6b-9707-8e8519bbe7f2";
            string actionTokenProcess = "eef99205-88af-419d-9338-dd17d5ca66e7";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        [Authorize]
        [Route("ListDataAccrue")]
        public ActionResult ListDataAccrue()
        {
            string actionTokenView = "2299a4d2-f448-49b9-a23c-8ed49859a193";
            string actionTokenProcess = "bd459585-f68f-48fa-a42a-9b64d8450788";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        [Authorize]
        [Route("FinanceConfirmation")]
        public ActionResult FinanceConfirmation()
        {
            string actionTokenView = "39120ba0-6f10-4734-bfbc-b677b6cfe2b1";
            string actionTokenProcess = "02e7d7ed-58d4-44b8-950a-430ca9440ed7";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        [Authorize]
        [Route("UserConfirmation")]
        public ActionResult UserConfirmation()
        {
            string actionTokenView = "50d350a6-fab0-4e7d-99bd-c3c361a41bed";
            string actionTokenProcess = "3551f186-9bbd-4ad9-b0ef-eb7147214c5b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.DocPath = Helper.Helper.TbigSysDoc();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        [Authorize]
        [Route("FinalConfirmation")]
        public ActionResult FinalConfirmation()
        {
            string actionTokenView = "799026a2-cbbd-4183-a9a7-be2d39457a69";
            string actionTokenProcess = "cb4d2a33-dc13-45b4-ba62-f9f8d02e476f";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.DocPath = Helper.Helper.TbigSysDoc();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        [Authorize]
        [Route("UploadAccrue")]
        public ActionResult UploadAccrue()
        {
            string actionTokenView = "C94D0740-3254-42C2-BE62-C8AC03B6A9A8";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionToken);
                    //ViewBag.DocPath = Helper.Helper.TbigSysDoc();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Route("downloadTemplateUpdateAccrue")]
        public ActionResult downloadTemplateUpdateAccrue()
        {
            string fileName = "Template Upload Accrue Update.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/templates/" + fileName));
            return File(bytes, contentType, fileName);
        }

        #endregion


        #region Export to Excel
        [Route("ListDataAccrue/Export")]
        public void GetListDataAccrueExport()
        {
            vwAccrueList post = new vwAccrueList();
            post.SONumber = Request.QueryString["SONumber"];
            post.SiteName = Request.QueryString["SiteName"];
            if (!string.IsNullOrWhiteSpace(Request.QueryString["EndDatePeriod"]))
                post.EndDatePeriod = Convert.ToDateTime(Request.QueryString["EndDatePeriod"]);
            post.CustomerID = Request.QueryString["CustomerID"];
            post.SOW = Request.QueryString["SOW"];
            post.CompanyID = Request.QueryString["CompanyID"];
            if (!string.IsNullOrWhiteSpace(Request.QueryString["RegionID"]))
                post.RegionID = int.Parse(Request.QueryString["RegionID"]);
            post.StatusAccrue = Request.QueryString["StatusAccrue"];
            ListDataAccrueService client = new ListDataAccrueService();
            int intTotalRecord = client.GetDataAccrueToListCount("", post);

            List<dwhAccrueListData> list = new List<dwhAccrueListData>();

            list = client.GetDataAccrueToList("", post, "", 0, 0);

            //Convert to DataTable
            string[] fieldList = new string[] {
                "SONumber",
                "SiteID",
                "SiteName",
                "SiteIDOpr",
                "StatusMasterlist",
                "CompanyMList",
                "CompanyInv",
                "Customer",
                "RFIDate",
                "SLDDate",
                "BAPSDate",
                "StartDateBAPS",
                "EndDateBAPS",
                "TenantType","BaseLeasePrice", "ServicePrice", "AmountTotal", "Currency","Accrue","Unearned","AccruePlusUnearned","Mio","Month", "StartDateAccrue","EndDateAccrue",
                "D", "OD", "ODCategory","Region", "Type", "Department","Status","Notes","Target","RootCause","PICADetail"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                i.SONumber,
                SiteID = i.SiteID,
                i.SiteName,
                SiteIDOpr = i.CustomerSiteID,
                StatusMasterlist = i.StatusMasterList,
                CompanyMList = i.CompanyID,
                CompanyInv = i.CompanyInvoiceID,
                Customer = i.CustomerName,
                RFIDate = i.RFIDate,
                SLDDate = i.SldDate,
                BAPSDate = i.BAPSDate,
                StartDateBAPS = i.StartBapsDate,
                EndDateBAPS = i.EndBapsDate,
                TenantType = i.TenantType,
                BaseLeasePrice = i.BaseLeasePrice,
                ServicePrice = i.ServicePrice,
                AmountTotal = i.AmountTotal,

                Currency = i.Currency,
                Accrue = i.Accrue,
                Unearned = i.Unearned,
                AccruePlusUnearned = i.AccrueUnearned,
                Mio = i.MioAccrue,

                Month = i.Month,
                StartDateAccrue = i.StartDateAccrue,
                EndDateAccrue = i.EndDateAccrue,
                D = i.D,
                OD = i.OD,
                ODCategory = i.ODCATEGORY,
                Region = i.RegionName,
                Type = i.Type,
                Department = i.SOW,
                Status = i.StatusAccrue,
                Notes = i.Remarks,
                Target = i.TargetUser,
                RootCause = i.RootCause,
                PICADetail = i.PICADetail
            }), fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ListDataAccrue", table);
        }

        [Route("FinanceConfirm/Export")]
        public void GetFinanceConfirmAccrueExport()
        {
            vwtrxDataAccrue post = new vwtrxDataAccrue();


            post.SONumber = Request.QueryString["SONumber"];
            if (!string.IsNullOrWhiteSpace(Request.QueryString["AccrueStatusID"]))
                post.AccrueStatusID = int.Parse(Request.QueryString["AccrueStatusID"]);

            string monthDate = Request.QueryString["monthDate"];
            string week = Request.QueryString["Week"];
            post.CustomerID = Request.QueryString["CustomerID"];
            post.CompanyID = Request.QueryString["CompanyID"];

            post.SOW = Request.QueryString["SOW"];

            FinanceConfirmationService client = new FinanceConfirmationService();
            //int intTotalRecord = client.GetFinanceConfirmToListCount("", post,"","");

            List<vwtrxDataAccrue> list = new List<vwtrxDataAccrue>();

            list = client.GetFinanceConfirmToList("", post, monthDate, week, "", 0, 5000000);

            //Convert to DataTable
            string[] fieldList = new string[] {
                "SONumber",
                "SiteID",
                "SiteName",
                "SiteIDOpr",
                "StatusMasterlist",
                "CompanyMList",
                "CompanyInv",
                "Customer",
                "RFIDate",
                "SLDDate",
                "BAPSDate",
                "StartDateBAPS",
                "EndDateBAPS",
                "TenantType","BaseLeasePrice", "ServicePrice", "AmountTotal", "Currency","Mio","Month", "StartDateAccrue","EndDateAccrue",
                "D", "OD", "ODCategory","Region", "Type", "Department","Status","Remarks"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                i.SONumber,
                SiteID = i.SiteID,
                i.SiteName,
                SiteIDOpr = i.SiteIDOpr,
                StatusMasterlist = i.StatusMasterList,
                CompanyMList = i.CompanyID,
                CompanyInv = i.CompanyInvID,
                Customer = i.CustomerName,
                RFIDate = i.RFIDate,
                SLDDate = i.SldDate,
                BAPSDate = i.BAPSDate,
                StartDateBAPS = i.StartDateBAPS,
                EndDateBAPS = i.EndDateBAPS,
                TenantType = i.TenantType,
                BaseLeasePrice = i.BaseLeasePrice,
                ServicePrice = i.ServicePrice,
                AmountTotal = i.TotalAmount,
                Currency = i.Currency,
                Mio = i.MioAccrue,

                Month = i.Month,
                StartDateAccrue = i.StartDateAccrue,
                EndDateAccrue = i.EndDateAccrue,
                D = i.D,
                OD = i.OD,
                ODCategory = i.ODCategory,
                Region = i.RegionName,
                Type = i.Type,
                Department = i.SOW,
                Status = i.StatusAccrue,
                Remarks = i.Remarks
            }), fieldList);
            table.Load(reader);
            //Export to Excel
            ExportToExcelHelper.Export("FinanceConfirm", table);
        }

        [Route("SettingAutoConfirm/Export")]
        public void SettingAutoConfirmExport()
        {
            vwmstAccrueSettingAutoConfirm post = new vwmstAccrueSettingAutoConfirm();



            if (!string.IsNullOrWhiteSpace(Request.QueryString["AccrueStatusID"]))
                post.AccrueStatusID = int.Parse(Request.QueryString["AccrueStatusID"]);

            string monthDate = Request.QueryString["Date"];
            string week = Request.QueryString["Week"];

            SettingAutoConfirmService client = new SettingAutoConfirmService();
            //int intTotalRecord = client.GetFinanceConfirmToListCount("", post,"","");

            List<vwmstAccrueSettingAutoConfirm> list = new List<vwmstAccrueSettingAutoConfirm>();

            list = client.GetSettingAutoConfirmToList("", post, monthDate, week, "", 0, 5000000);

            //Convert to DataTable
            string[] fieldList = new string[] {
                "Periode",
                "Activity",
                "AutoConfirmDate"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                Periode = i.PeriodText,
                Activity = i.AccrueStatus,
                i.AutoConfirmDate
            }), fieldList);
            table.Load(reader);
            //Export to Excel
            ExportToExcelHelper.Export("SettingAutoConfirm", table);
        }

        [Route("UserConfirm/Export")]
        public void GetUserConfirmAccrueExport()
        {
            vwtrxDataAccrue post = new vwtrxDataAccrue();

            post.SONumber = Request.QueryString["SONumber"];
            if (!string.IsNullOrWhiteSpace(Request.QueryString["AccrueStatusID"]))
                post.AccrueStatusID = int.Parse(Request.QueryString["AccrueStatusID"]);

            string monthDate = Request.QueryString["monthDate"];
            string week = Request.QueryString["Week"];
            post.CustomerID = Request.QueryString["CustomerID"];
            post.CompanyID = Request.QueryString["CompanyID"];

            post.SOW = Request.QueryString["SOW"];

            UserConfirmationService client = new UserConfirmationService();

            List<vwtrxDataAccrue> list = new List<vwtrxDataAccrue>();
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            list = client.GetUserConfirmToList(userCredential.UserID, post, monthDate, week, "", 0, 5000000);

            //Convert to DataTable
            string[] fieldList = new string[] {
                "SONumber",
                "SiteID",
                "SiteName",
                "SiteIDOpr",
                "StatusMasterlist",
                "CompanyMList",
                "CompanyInv",
                "RFIDate",
                "SLDDate",
                "BAPSDate",
                "StartDateBAPS",
                "EndDateBAPS",
                "TenantType","BaseLeasePrice", "ServicePrice", "AmountTotal", "Currency","Mio","Month", "StartDateAccrue","EndDateAccrue",
                "D", "OD", "ODCategory","Region", "Type", "Department","Status","Remarks"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                i.SONumber,
                SiteID = i.SiteID,
                i.SiteName,
                SiteIDOpr = i.SiteIDOpr,
                StatusMasterlist = i.StatusMasterList,
                CompanyMList = i.CompanyID,
                CompanyInv = i.CompanyInvID,
                RFIDate = i.RFIDate,
                SLDDate = i.SldDate,
                BAPSDate = i.BAPSDate,
                StartDateBAPS = i.StartDateBAPS,
                EndDateBAPS = i.EndDateBAPS,
                TenantType = i.TenantType,
                BaseLeasePrice = i.BaseLeasePrice,
                ServicePrice = i.ServicePrice,
                AmountTotal = i.TotalAmount,
                Currency = i.Currency,
                Mio = i.MioAccrue,
                Month = i.Month,
                StartDateAccrue = i.StartDateAccrue,
                EndDateAccrue = i.EndDateAccrue,
                D = i.D,
                OD = i.OD,
                ODCategory = i.ODCategory,
                Region = i.RegionName,
                Type = i.Type,
                Department = i.SOW,
                Status = i.StatusAccrue,
                Remarks = i.Remarks
            }), fieldList);
            table.Load(reader);
            //Export to Excel
            ExportToExcelHelper.Export("UserConfirm", table);
        }

        [Route("FinalConfirmSummary/Export")]
        public void FinalConfirmSummaryExport()
        {
            vwAccrueFinalConfirm post = new vwAccrueFinalConfirm();
            string dateParam = Request.QueryString["Date"];
            string weekParam = Request.QueryString["Week"];


            FinalConfirmationService client = new FinalConfirmationService();

            List<vwAccrueFinalConfirm> list = new List<vwAccrueFinalConfirm>();
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            list = client.GetFinalConfirmToList(userCredential.UserID, post, dateParam, weekParam, "", 0, 5000000);

            //Convert to DataTable
            string[] fieldList = new string[] {
                "Division",
                "NEW_CURR_NoOfSite",
                "NEW_CURR_Amount",
                "NEW_OD_NoOfSite",
                "NEW_OD_Amount",
                "REN_CURR_NoOfSite",
                "REN_CURR_Amount",
                "REN_OD_NoOfSite",
                "REN_OD_Amount",
                "Total_NoOfSite",
                "Total_Amount",
                "Percent"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                Division = i.SOW,
                NEW_CURR_NoOfSite = i.NEWCURRNoOfSite,
                NEW_CURR_Amount = i.NEWCURRTotalAmount,
                NEW_OD_NoOfSite = i.NEWODNoOfSite,
                NEW_OD_Amount = i.NEWODTotalAmount,
                REN_CURR_NoOfSite = i.RECCURRNoOfSite,
                REN_CURR_Amount = i.RECCURRTotalAmount,
                REN_OD_NoOfSite = i.RECODNoOfSite,
                REN_OD_Amount = i.RECODTotalAmount,
                Total_NoOfSite = i.TotalNoOfSite,
                Total_Amount = i.TotalAmount,
                Percent = i.Percen
            }), fieldList);
            table.Load(reader);
            var sumNEW_CURR_NoOfSite = table.Compute("Sum(NEW_CURR_NoOfSite)", string.Empty);
            var sumNEW_CURR_Amount = table.Compute("Sum(NEW_CURR_Amount)", string.Empty);
            var sumNEW_OD_NoOfSite = table.Compute("Sum(NEW_OD_NoOfSite)", string.Empty);
            var sumNEW_OD_Amount = table.Compute("Sum(NEW_OD_Amount)", string.Empty);
            var sumREN_CURR_NoOfSite = table.Compute("Sum(REN_CURR_NoOfSite)", string.Empty);
            var sumREN_CURR_Amount = table.Compute("Sum(REN_CURR_Amount)", string.Empty);
            var sumREN_OD_NoOfSite = table.Compute("Sum(REN_OD_NoOfSite)", string.Empty);
            var sumREN_OD_Amount = table.Compute("Sum(REN_OD_Amount)", string.Empty);
            var sumTotal_NoOfSite = table.Compute("Sum(Total_NoOfSite)", string.Empty);
            var sumTotal_Amount = table.Compute("Sum(Total_Amount)", string.Empty);
            var sumPercent = table.Compute("Sum(Percent)", string.Empty);
            if (table.Rows.Count > 0)
                table.Rows.Add("Grand Total", sumNEW_CURR_NoOfSite, sumNEW_CURR_Amount, sumNEW_OD_NoOfSite, sumNEW_OD_Amount, sumREN_CURR_NoOfSite, sumREN_CURR_Amount, sumREN_OD_NoOfSite, sumREN_OD_Amount, sumTotal_NoOfSite, sumTotal_Amount, sumPercent);
            //else
            //    table.Rows.Add("Grand Total", "", "", "", "", "", "", "", "", "", "", "");

            List<vwtrxDataAccrue> listDetail = new List<vwtrxDataAccrue>();
            vwtrxDataAccrue postDetail = new vwtrxDataAccrue();
            postDetail.AccrueStatusID = 6;
            listDetail = client.GetUserConfirmedToListExcel(userCredential.UserID, postDetail, dateParam, weekParam, "");
            //Convert to DataTable
            string[] fieldListDetail = new string[] {
                "SONumber",
                "SiteID",
                "SiteName",
                "SiteIDOpr",
                "StatusMasterlist",
                "CompanyMList",
                "CompanyInv",
                "RFIDate",
                "SLDDate",
                "BAPSDate",
                "StartDateBAPS",
                "EndDateBAPS",
                "TenantType","BaseLeasePrice", "ServicePrice", "AmountTotal", "Currency","Mio","Month", "StartDateAccrue","EndDateAccrue",
                "D", "OD", "ODCategory","Region", "Type", "Department","Status","Remarks"
            };
            DataTable tableDetail = new DataTable();
            var readerDetail = FastMember.ObjectReader.Create(listDetail.Select(i => new
            {
                i.SONumber,
                SiteID = i.SiteID,
                i.SiteName,
                SiteIDOpr = i.SiteIDOpr,
                StatusMasterlist = i.StatusMasterList,
                CompanyMList = i.CompanyID,
                CompanyInv = i.CompanyInvID,
                RFIDate = i.RFIDate,
                SLDDate = i.SldDate,
                BAPSDate = i.BAPSDate,
                StartDateBAPS = i.StartDateBAPS,
                EndDateBAPS = i.EndDateBAPS,
                TenantType = i.TenantType,
                BaseLeasePrice = i.BaseLeasePrice,
                ServicePrice = i.ServicePrice,
                AmountTotal = i.TotalAmount,
                Currency = i.Currency,
                Mio = i.MioAccrue,
                Month = i.Month,
                StartDateAccrue = i.StartDateAccrue,
                EndDateAccrue = i.EndDateAccrue,
                D = i.D,
                OD = i.OD,
                ODCategory = i.ODCategory,
                Region = i.RegionName,
                Type = i.Type,
                Department = i.SOW,
                Status = i.StatusAccrue,
                Remarks = i.Remarks
            }), fieldListDetail);
            tableDetail.Load(readerDetail);
            //Export to Excel

            try
            {
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(table, "Accrue Department");
                wbook.Worksheets.Add(tableDetail, "Detail");

                System.Web.HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=FinalConfirmed.xlsx");

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch (Exception ex)
            {

            }

            //Export to Excel
            //ExportToExcelHelper.Export("FinalConfirmSummary", table);
        }

        [Route("FinalConfirmedUserSummary/Export")]
        public void FinalConfirmedUserSummary()
        {
            string dateParam = Request.QueryString["Date"];
            string weekParam = Request.QueryString["Week"];

            FinalConfirmationService client = new FinalConfirmationService();
            List<vwtrxDataAccrue> list = new List<vwtrxDataAccrue>();
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            DataTable table = new DataTable();
            table = client.GetUserConfirmedFinalToDataTable(userCredential.UserID, dateParam, weekParam, "", 0, 5000000); //report summary user vs user

            vwtrxDataAccrue post = new vwtrxDataAccrue();
            list = client.GetUserConfirmedToListExcel(userCredential.UserID, post, dateParam, weekParam, "");

            //Convert to DataTable
            string[] fieldList = new string[] {
                "SONumber",
                "SiteID",
                "SiteName",
                "SiteIDOpr",
                "StatusMasterlist",
                "CompanyMList",
                "CompanyInv",
                "RFIDate",
                "SLDDate",
                "BAPSDate",
                "StartDateBAPS",
                "EndDateBAPS",
                "TenantType","BaseLeasePrice", "ServicePrice", "AmountTotal", "Currency","Mio","Month", "StartDateAccrue","EndDateAccrue",
                "D", "OD", "ODCategory","Region", "Type", "Department","Status","Remarks"
            };
            DataTable tableDetail = new DataTable();
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                i.SONumber,
                SiteID = i.SiteID,
                i.SiteName,
                SiteIDOpr = i.SiteIDOpr,
                StatusMasterlist = i.StatusMasterList,
                CompanyMList = i.CompanyID,
                CompanyInv = i.CompanyInvID,
                RFIDate = i.RFIDate,
                SLDDate = i.SldDate,
                BAPSDate = i.BAPSDate,
                StartDateBAPS = i.StartDateBAPS,
                EndDateBAPS = i.EndDateBAPS,
                TenantType = i.TenantType,
                BaseLeasePrice = i.BaseLeasePrice,
                ServicePrice = i.ServicePrice,
                AmountTotal = i.TotalAmount,
                Currency = i.Currency,
                Mio = i.MioAccrue,
                Month = i.Month,
                StartDateAccrue = i.StartDateAccrue,
                EndDateAccrue = i.EndDateAccrue,
                D = i.D,
                OD = i.OD,
                ODCategory = i.ODCategory,
                Region = i.RegionName,
                Type = i.Type,
                Department = i.SOW,
                Status = i.StatusAccrue,
                Remarks = i.Remarks
            }), fieldList);
            tableDetail.Load(reader);
            //Export to Excel

            try
            {
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(table, "ConfirmedUser");
                wbook.Worksheets.Add(tableDetail, "Detail");

                System.Web.HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=AccrueConfirmedUser.xlsx");

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch (Exception ex)
            {

            }



            //ExportToExcelHelper.Export("ConfirmedUser", table);
        }
        //test
        [Route("HistoryUploadAccrue/Export")]
        public void GetHistoryUploadAccrueExport()
        {
            vwtrxUploadDataAccrueStaging post = new vwtrxUploadDataAccrueStaging();

            string week = Request.QueryString["Week"];
            if (!string.IsNullOrWhiteSpace(Request.QueryString["AccrueStatusID"]))
                post.AccrueStatusID = int.Parse(Request.QueryString["AccrueStatusID"]);
            post.MonthYear = Request.QueryString["MonthYear"];
            post.SONumber = Request.QueryString["SONumber"];
            post.SiteID = Request.QueryString["SiteID"];

            UploadAccrueService client = new UploadAccrueService();

            List<vwtrxUploadDataAccrueStaging> list = new List<vwtrxUploadDataAccrueStaging>();

            list = client.GetHistoryUploadList(post, 0, 0);

            //Convert to DataTable
            string[] fieldList = new string[] {
                "SONumber",
                "SiteID",
                "SiteName",
                "SiteIDOpr",
                "SiteNameOpr",
                "CompanyID",
                "CompanyInvID",
                "CustomerID",
                "RFIDate",
                "SldDate",
                "RentalStart",
                "RentalEndNew",
                "TenantType",
                "Type","BaseLeasePrice", "ServicePrice", "BaseOnMasterListData", "BaseOnRevenueListingNew","StartDateAccrue","EndDateAccrue", "TotalAccrue","Month",
                "Year", "Week", "AccrueStatus"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                i.SONumber,
                SiteID = i.SiteID,
                i.SiteName,
                SiteIDOpr = i.SiteIDOpr,
                SiteNameOpr = i.SiteNameOpr,
                CompanyID = i.CompanyID,
                CompanyInvID = i.CompanyInvID,
                CustomerID = i.CustomerID,
                RFIDate = i.RFIDate,
                SldDate = i.SldDate,
                RentalStart = i.RentalStart,
                RentalEndNew = i.RentalEndNew,
                TenantType = i.TenantType,
                Type = i.TenantType,
                BaseLeasePrice = i.BaseLeasePrice,
                ServicePrice = i.ServicePrice,
                BaseOnMasterListData = i.BaseOnMasterListData,
                BaseOnRevenueListingNew = i.BaseOnRevenueListingNew,
                StartDateAccrue = i.StartDateAccrue,

                EndDateAccrue = i.EndDateAccrue,
                TotalAccrue = i.TotalAccrue,
                Month = i.Month,
                Year = i.Year,
                Week = i.Week,
                AccrueStatus = i.AccrueStatus
            }), fieldList);
            table.Load(reader);
            //Export to Excel
            ExportToExcelHelper.Export("HistoryUpload", table);
        }
        [Route("UploadAccrue/ExportDetailTable")]
        public void GetUploadAccrueExport()
        {
            vwtrxUploadDataAccrue post = new vwtrxUploadDataAccrue();

            post.SONumber = Request.QueryString["SONumber"];
            post.SiteID = Request.QueryString["SiteID"];
            post.Remarks = Request.QueryString["Remarks"];

            UploadAccrueService client = new UploadAccrueService();

            List<vwtrxUploadDataAccrue> list = new List<vwtrxUploadDataAccrue>();

            list = client.GetValidateAccrueList(post, 0, 0);

            //Convert to DataTable
            string[] fieldList = new string[] {
                "SONumber",
                "SiteID",
                "SiteName",
                "SiteIDOpr",
                "SiteNameOpr",
                "CompanyID",
                "CompanyInvID",
                "CustomerID",
                "RFIDate",
                "SldDate",
                "RentalStart",
                "RentalEndNew",
                "TenantType",
                "Type","BaseLeasePrice", "ServicePrice", "BaseOnMasterListData", "BaseOnRevenueListingNew","StartDateAccrue","EndDateAccrue", "TotalAccrue", "MonthYear",
                "Year", "Week", "Remarks"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                i.SONumber,
                SiteID = i.SiteID,
                i.SiteName,
                SiteIDOpr = i.SiteIDOpr,
                SiteNameOpr = i.SiteNameOpr,
                CompanyID = i.CompanyID,
                CompanyInvID = i.CompanyInvID,
                CustomerID = i.CustomerID,
                RFIDate = i.RFIDate,
                SldDate = i.SldDate,
                RentalStart = i.RentalStart,
                RentalEndNew = i.RentalEndNew,
                TenantType = i.TenantType,
                Type = i.TenantType,
                BaseLeasePrice = i.BaseLeasePrice,
                ServicePrice = i.ServicePrice,
                BaseOnMasterListData = i.BaseOnMasterListData,
                BaseOnRevenueListingNew = i.BaseOnRevenueListingNew,
                StartDateAccrue = i.StartDateAccrue,

                EndDateAccrue = i.EndDateAccrue,
                TotalAccrue = i.TotalAccrue,
                MonthYear = i.MonthYear,
                Year = i.Year,
                Week = i.Week,
                Remarks = i.Remarks
            }), fieldList);
            table.Load(reader);
            //Export to Excel
            ExportToExcelHelper.Export("UploadDataAccrue", table);
        }



        #endregion
    }
}
