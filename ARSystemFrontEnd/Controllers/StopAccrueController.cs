using System;
using System.Web.Mvc;
using System.Web;
using ARSystemFrontEnd.Providers;
using System.IO;
using System.Data;
using System.Collections.Generic;
using ARSystemFrontEnd.Models;
using System.Linq;
using ARSystemFrontEnd.Helper;
using ARSystem.Domain.Models;
using ARSystem.Service;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Parsing;
using TBGFramework.HRData;

namespace ARSystemFrontEnd.Controllers
{

    [RoutePrefix("StopAccrue")]
    public class StopAccrueController : BaseController
    {
        [Authorize]
        [Route("Request")]
        public ActionResult RequestStopAccrue()
        {
            string actionTokenView = "B5C847DA-234E-415C-BC52-0523AEE0CC73";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    if (userCredential.ErrorType > 0)
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    var service = new StopAccrueService();
                    var RoleLabel = new mstStopAccrueRoleLabel();
                    RoleLabel = service.StopAccrueRoleLabel(userCredential.UserRoleID);
                    var hrData = new Employee();
                    var hrDataResult = hrData.GetDetailByUserID(userCredential.UserID);
                    ViewBag.StopAccrueUser = hrDataResult.NIP;
                    ViewBag.StopAccrueUserRoleLabel = RoleLabel == null ? "" : RoleLabel.RoleLabel;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("UpdateAmount")]
        public ActionResult UpdateAmount()
        {
            string actionTokenView = "B5C847DA-234E-415C-BC52-0523AEE0CC73";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    if (userCredential.ErrorType > 0)
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    var service = new StopAccrueService();
                    var RoleLabel = new mstStopAccrueRoleLabel();
                    RoleLabel = service.StopAccrueRoleLabel(userCredential.UserRoleID);

                    ViewBag.StopAccrueUser = userCredential.UserID;
                    ViewBag.StopAccrueUserRoleLabel = RoleLabel == null ? "" : RoleLabel.RoleLabel;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [HttpPost, Route("UploadFile")]
        public ActionResult UploadFile()
        {
            string actionTokenView = "B5C847DA-234E-415C-BC52-0523AEE0CC73";

            try
            {
                string result = "";
                using (var client = new SecureAccessService.UserServiceClient())
                {
                    if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                    {
                        vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                        if (userCredential.ErrorType > 0)
                        {
                            return RedirectToAction("Logout", "Login");
                        }

                        HttpPostedFileBase files = Request.Files[0];

                        FileInfo fi = new FileInfo(files.FileName);
                        if (fi.Extension != ".pdf" && fi.Extension != ".zip" && fi.Extension != ".rar")
                        {
                            result = "Please upload an PDF or ZIP or RAR File.!";
                        }
                        else if ((files.ContentLength / 1048576.0) > 2)
                        {
                            result = "Upload File Can`t bigger then 2048 bytes (2mb)..! ";
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(files.FileName);
                            string _path = Path.Combine(Server.MapPath(Helper.Helper.GetDocPath() + "StopAccrue/RequestDetail"));
                            //string _path = "\\StopAccrue\\" + Helper.Helper.GetFileTimeStamp(_FileName);    unremark jika naik ker prod
                            if (!Directory.Exists(_path))
                            {
                                Directory.CreateDirectory(_path);
                            }
							  _FileName = _FileName.Replace("&", "dan");
                            files.SaveAs(Path.Combine(_path, _FileName));
                        }
                    }
                    else
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                }
                return Json(result);
            }
            catch (Exception ex)
            {

                return Json("Error System.");
            }
        }

        [Authorize]
        [HttpGet, Route("downloadFile")]
        public FileResult DownloadFile()
        {

            try
            {
                string fileName = Request.QueryString["fileName"];
                string _path = Path.Combine(Server.MapPath(Helper.Helper.GetDocPath() + "StopAccrue/RequestDetail"));
                byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_path, fileName));
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception)
            {


                throw;
            }
           
        }

        [Authorize]
        [HttpGet, Route("downloadDocSubmit")]
        public FileResult DownloadDocSubmit()
        {

            try
            {
                string fileName = Request.QueryString["fileName"];
                string activity = Request.QueryString["activity"];
                string AppHeaderID = Request.QueryString["appHeaderID"];
                string RequestType = Request.QueryString["accrueType"];

                string _path = Path.Combine(Server.MapPath(Helper.Helper.GetDocPath() + "StopAccrue/RequestHeader"));

                if (activity.ToLower().Equals("finish"))
                {
                    StopAccrueService service = new StopAccrueService();
                    var logAct = new List<vwWfHeaderActivityLogs>();
                    logAct = service.GetListActivityLogs(int.Parse(AppHeaderID));

                    string divAsetReceiveDate = String.Format("{0:dd MMM yyyy}", logAct.Where(x => x.ActivityLabel == "SA_DOC_RECEIVE_DHAS").OrderByDescending(x => x.LogID).Select(x => x.LogTime).FirstOrDefault());
                    string divAccReceiveDate = String.Format("{0:dd MMM yyyy}", logAct.Where(x => x.ActivityLabel == "SA_DOC_RECEIVE_DHAC").OrderByDescending(x => x.LogID).Select(x => x.LogTime).FirstOrDefault());

                    PdfLoadedDocument loadedDocument = new PdfLoadedDocument(Path.Combine(_path, fileName));

                    PdfPageBase loadedPage = loadedDocument.Pages[0];

                    PdfGraphics graphics = loadedPage.Graphics;

                    //set the font

                    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                    // watermark text.

                    PdfGraphicsState state = graphics.Save();

                    graphics.SetTransparency(0.25f);

                    graphics.RotateTransform(-30);

                    if (RequestType.Equals("STOP"))
                    {
                        graphics.DrawString("Ditindak Lanjuti", font, PdfPens.Blue, PdfBrushes.Blue, new PointF(-250, 500));
                        graphics.DrawString(divAsetReceiveDate, font, PdfPens.Blue, PdfBrushes.Blue, new PointF(-255, 525));
                    }


                    graphics.DrawString("Ditindak Lanjuti", font, PdfPens.Blue, PdfBrushes.Blue, new PointF(40, 670));
                    graphics.DrawString(divAccReceiveDate, font, PdfPens.Blue, PdfBrushes.Blue, new PointF(30, 695));


                    //Save and close the document.

                    string[] files = fileName.Split('.');
                    fileName = files[0] + "_Approved.pdf";
                    loadedDocument.Save(Path.Combine(_path, fileName));

                    loadedDocument.Close(true);
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_path, fileName));
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Authorize]
        [HttpGet, Route("downloadFileHeader")]
        public FileResult DownloadFileHeader()
        {

            try
            {
                string fileName = Request.QueryString["fileName"];
                string _path = Path.Combine(Server.MapPath(Helper.Helper.GetDocPath() + "StopAccrue/RequestHeader"));
                byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_path, fileName));
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Authorize]
        [HttpGet, Route("exportSonumbList")]
        public void ExportSonumbList(PostStopAccrueSonumbList post)
        {
            DataTable table = new DataTable();
            try
            {
                var data = new List<vwStopAccrueSonumbList>();
                var param = new vwStopAccrueSonumbList();
                param.RegionID = post.RegionID;
                param.CustomerID = post.CustomerID;
                param.CompanyID = post.CompanyID;
                param.ProductID = post.ProductID;
                param.RFIDone = post.RFIDone;
                param.SiteID = post.SiteID;
                param.SiteName = post.SiteName;
                param.SONumber = post.SONumber;

                var service = new StopAccrueService();
                data = service.GetSonumbList(param);

                string[] ColumsShow = new string[] { "SONumber", "SiteID", "SiteName", "RFIDate", "CustomerID", "CompanyID", "TenantType", "RegionName", "CustomerSiteID", "CustomerSiteName" };
                var reader = FastMember.ObjectReader.Create(data.Select(i => new
                {
                    i.SONumber,
                    i.SiteID,
                    i.SiteName,
                    RFIDate = String.Format("{0:dd-MMM-yyyy}", i.RFIDone),
                    i.CustomerID,
                    i.CompanyID,
                    TenantType = i.Product,
                    i.RegionName,
                    i.CustomerSiteID,
                    i.CustomerSiteName
                }), ColumsShow);
                table.Load(reader);

                ExportToExcelHelper.Export("SonumbList", table);
            }
            catch (Exception ex)
            {

            }

        }


        [Authorize]
        [HttpGet, Route("exportRequestHeader")]
        public void ExportRequestHeader(PostStopAccrueHeader post)
        {
            DataTable table = new DataTable();
            try
            {

                var service = new StopAccrueService();
                var data = new List<vwStopAccrueHeader>();
                var param = new vwStopAccrueHeader();
                param.RequestTypeID = post.RequestTypeID;
                param.ActivityID = post.ActivityID;
                param.ActivityOwner = post.ActivityOwner;
                param.ActivityOwnerName = post.ActivityOwnerName;
                param.InitiatorName = post.InitiatorName;
                param.Initiator = post.Initiator;
                param.StartEffectiveDate = post.StartEffectiveDate;
                param.EndEffectiveDate = post.EndEffectiveDate;
                param.CreatedDate = post.CreatedDate;
                param.RequestNumber = post.RequestNumber;
                param.UserRole = post.UserRole;

                data = service.GetHeaderRequestList(param);
                string[] ColumsShow = new string[] { "RequestNumber", "Initiator", "ActivityOwner", "CreatedDate", "StartEffectiveDate", "EndEffectiveDate", "Activity", "SubmissionType" };
                var reader = FastMember.ObjectReader.Create(data.Select(i => new
                {
                    i.RequestNumber,
                    Initiator = i.InitiatorName,
                    ActivityOwner = i.ActivityOwnerName,
                    CreatedDate = String.Format("{0:dd-MMM-yyyy}", i.CreatedDate),
                    StartEffectiveDate = String.Format("{0:dd-MMM-yyyy}", i.StartEffectiveDate),
                    EndEffectiveDate = String.Format("{0:dd-MMM-yyyy}", i.EndEffectiveDate),
                    Activity = i.ActivityName,
                    SubmissionType = i.AccrueType

                }), ColumsShow);
                table.Load(reader);

                ExportToExcelHelper.Export("RequestStopAccrueHeader", table);
            }
            catch (Exception ex)
            {

            }

        }

        [Authorize]
        [HttpGet, Route("exportRequestDetail")]
        public void ExportRequestDetail()
        {
            DataTable table = new DataTable();
            try
            {
                string ID = Request.QueryString["HeaderID"];
                string IsReHold = Request.QueryString["IsReHold"];
                string RequestNumber = Request.QueryString["RequestNumber"];
                RequestNumber = RequestNumber.Replace("/", "_");
                var service = new StopAccrueService();
                var data = new List<vwStopAccrueDetail>();
                data = service.GetDetailRequestList(int.Parse(ID));
                if (IsReHold.ToLower().Equals("true"))
                {
                    string[] ColumsShow = new string[] { "SONumber", "ReActive", "SiteID", "SiteName", "Company", "Customer", "TenantType", "RFIDate", "CategoryCase", "DetailCase", "EffectiveDate", "RevenueAmount", "CapexAmount", "Attachment", "Remarks" };
                    var reader = FastMember.ObjectReader.Create(data.Select(i => new
                    {
                        i.SONumber,
                        ReActive = (i.IsHold == true ? "No" : "Yes"),
                        i.SiteID,
                        i.SiteName,
                        i.Company,
                        i.Customer,
                        TenantType = i.Product,
                        RFIDate = String.Format("{0:dd/MM/yyyy}", i.RFIDone),
                        i.CategoryCase,
                        i.DetailCase,
                        EffectiveDate = String.Format("{0:dd/MM/yyyy}", i.EffectiveDate),
                        RevenueAmount = i.RevenueAmount,
                        CapexAmount = i.CapexAmount,
                        Attachment = i.FileName,
                        i.Remarks
                    }), ColumsShow);
                    table.Load(reader);
                }
                else
                {
                    string[] ColumsShow = new string[] { "SONumber", "SiteID", "SiteName", "Company", "Customer", "TenantType", "RFIDate", "CategoryCase", "DetailCase", "EffectiveDate", "RevenueAmount", "CapexAmount", "Attachment", "Remarks" };
                    var reader = FastMember.ObjectReader.Create(data.Select(i => new
                    {
                        i.SONumber,
                        i.SiteID,
                        i.SiteName,
                        i.Company,
                        i.Customer,
                        TenantType = i.Product,
                        RFIDate = String.Format("{0:dd/MM/yyyy}", i.RFIDone),
                        i.CategoryCase,
                        i.DetailCase,
                        EffectiveDate = String.Format("{0:dd/MM/yyyy}", i.EffectiveDate),
                        RevenueAmount = i.RevenueAmount,
                        CapexAmount = i.CapexAmount,
                        Attachment = i.FileName,
                        i.Remarks
                    }), ColumsShow);
                    table.Load(reader);
                }

                ExportToExcelHelper.Export(RequestNumber, table);
            }
            catch (Exception ex)
            {

            }

        }

        [Authorize]
        [HttpGet, Route("exportDashboardHeader")]
        public void ExportDashboardHeader(PostStopAccrueHeader post)
        {
            DataTable table = new DataTable();
            try
            {

                var service = new StopAccrueService();
                var data = new List<vwStopAccrueDashboardHeader>();
                var param = new vwStopAccrueDashboardHeader();
                param.RequestTypeID = post.RequestTypeID;
                param.RequestNumber = post.RequestNumber;
                param.CraetedDate = post.CreatedDate;
                param.CraetedDate2 = post.CreatedDate2;
                param.DepartName = post.DepartName;
                param.Activity = post.Activity;

                data = service.GetDashboardHeaderList(param, post.start, post.length);

                string[] ColumsShow = new string[] { "Number", "RequestNumber", "PIC", "ActivityStatus", "CountSoNumber", "SaldoAccrue_mio", "Capex_mio", "CraetedDate", "LastUpdate" };
                var reader = FastMember.ObjectReader.Create(data.Select(i => new
                {
                    Number = i.RowIndex,
                    i.RequestNumber,
                    PIC = i.DepartName,
                    ActivityStatus = i.Activity,
                    CountSoNumber = i.SoNumberCount,
                    SaldoAccrue_mio = i.SumRevenue,
                    Capex_mio = i.SumCapex,
                    i.CraetedDate,
                    LastUpdate = i.LastModifiedDate

                }), ColumsShow);
                table.Load(reader);

                ExportToExcelHelper.Export("MonitorngRequestStopAccrue", table);
            }
            catch (Exception ex)
            {

            }

        }

        [Authorize]
        [HttpGet, Route("exportDashboardDetail")]
        public void ExportDashboardDetaik()
        {
            DataTable table = new DataTable();
            try
            {
                string ID = Request.QueryString["HeaderID"];
                string IsReHold = Request.QueryString["IsReHold"];
                string RequestNumber = Request.QueryString["RequestNumber"];
                RequestNumber = RequestNumber.Replace("/", "_");
                var service = new StopAccrueService();
                var data = new List<vwStopAccrueDashboardDetail>();
                var param = new vwStopAccrueDashboardDetail();
                param.TrxStopAccrueHeaderID = int.Parse(ID);

                data = service.GetDashboardDetailList(param, 0, 0);
                var dataSum = new vwStopAccrueDashboardDetail();
                dataSum.RowIndex = "Total";
                dataSum.CapexAmount = data.Sum(x => x.CapexAmount);
                dataSum.RevenueAmount = data.Sum(x => x.RevenueAmount);
                dataSum.CompensationAmount = data.Sum(x => x.CompensationAmount);
                data.Add(dataSum);

                if (IsReHold.ToLower().Equals("false"))
                {
                    string[] ColumsShow = new string[] { "Number", "RequestNumber", "SubmissionType", "Company", "SONumber", "SiteName", "Customer", "TenantType", "RFIDate", "SaldoAccrue_mio", "Capex_mio", "CategoryCase", "DetailCase", "Compensation_mio", "PIC", "StartDate", "EndDate", "Initiator" };
                    var reader = FastMember.ObjectReader.Create(data.Select(i => new
                    {
                        Number = i.RowIndex,
                        i.RequestNumber,
                        SubmissionType = i.AccrueType,
                        i.Company,
                        i.SONumber,
                        i.SiteName,
                        i.Customer,
                        TenantType = i.Product,
                        RFIDate = String.Format("{0:dd/MM/yyyy}", i.RFIDone),
                        SaldoAccrue_mio = i.RevenueAmount,
                        Capex_mio = i.CapexAmount,
                        i.CategoryCase,
                        i.DetailCase,
                        Compensation_mio = i.CompensationAmount,
                        PIC = i.DepartName,
                        StartDate = String.Format("{0:dd/MM/yyyy}", i.StartEffectiveDate),
                        EndDate = String.Format("{0:dd/MM/yyyy}", i.EndEffectiveDate),
                        Initiator = i.Initiator,

                    }), ColumsShow);
                    table.Load(reader);


                }
                else
                {
                    string[] ColumsShow = new string[] { "Number", "RequestNumber", "SubmissionType", "Company", "SONumber", "SiteName", "Customer", "TenantType", "RFIDate", "SaldoAccrue_mio", "Capex_mio", "CategoryCase", "DetailCase", "Compensation_mio", "Status", "PIC", "StartDate", "EndDate", "Initiator" };
                    var reader = FastMember.ObjectReader.Create(data.Select(i => new
                    {
                        Number = i.RowIndex,
                        i.RequestNumber,
                        SubmissionType = i.AccrueType,
                        i.Company,
                        i.SONumber,
                        i.SiteName,
                        i.Customer,
                        TenantType = i.Product,
                        RFIDate = String.Format("{0:dd/MM/yyyy}", i.RFIDone),
                        SaldoAccrue_mio = i.RevenueAmount,
                        Capex_mio = i.CapexAmount,
                        i.CategoryCase,
                        i.DetailCase,
                        Compensation_mio = i.CompensationAmount,
                        Status = (i.IsHold == true ? "Hold" : "Active"),
                        PIC = i.DepartName,
                        StartDate = i.IsHold == false ? "" : String.Format("{0:dd/MM/yyyy}", i.StartEffectiveDate),
                        EndDate = i.IsHold == false ? "" : String.Format("{0:dd/MM/yyyy}", i.EndEffectiveDate),
                        Initiator = i.Initiator,

                    }), ColumsShow);
                    table.Load(reader);

                }
                ExportToExcelHelper.Export(RequestNumber, table);
            }
            catch (Exception ex)
            {

            }

        }

        [Authorize]
        [HttpGet, Route("downloadTemplateUpdateAmount")]
        public void DownloadTemplateUpdateAmount()
        {
            DataTable table = new DataTable();
            try
            {
                string ID = Request.QueryString["HeaderID"];
                string RequestNumber = Request.QueryString["RequestNumber"];
                RequestNumber = RequestNumber.Replace("/", "_");
                var service = new StopAccrueService();
                var data = new List<vwStopAccrueDetail>();
                data = service.GetDetailRequestList(int.Parse(ID));
                data = data.Where(x => x.IsHold == true).ToList();
                string[] ColumsShow = new string[] { "SONumber", "CapexAmount", "RevenueAmount" };
                var reader = FastMember.ObjectReader.Create(data.Select(i => new
                {
                    SONumber = i.SONumber,
                    CapexAmount = i.CapexAmount == 0 || i.CapexAmount == null ? 0 : i.CapexAmount,
                    RevenueAmount = i.RevenueAmount == 0 || i.RevenueAmount == null ? 0 : i.RevenueAmount
                }), ColumsShow);
                table.Load(reader);

                ExportToExcelHelper.Export(RequestNumber, table);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet, Route("exportRequestHeaderUpdateAmount")]
        public void GetRequestHeaderUpdateAmount(PostStopAccrueHeader post)
        {
            DataTable table = new DataTable();
            try
            {
                var service = new StopAccrueService();
                var data = new List<vwStopAccrueHeader>();

                var param = new vwStopAccrueHeader();
                param.RequestTypeID = post.RequestTypeID;
                param.ActivityOwnerName = post.ActivityOwnerName;
                param.InitiatorName = post.InitiatorName;
                param.StartEffectiveDate = post.StartEffectiveDate;
                param.CreatedDate = post.CreatedDate;
                param.RequestNumber = post.RequestNumber;
                param.Initiator = post.Initiator;
                param.UserRole = post.UserRole;

                data = service.GetRequestUpdateAmountList(param, 0, 0);

                string[] ColumsShow = new string[] { "Number", "RequestNumber", "Initiator", "ActivityOwner", "CreatedDate", "StartEffectiveDate", "EndEffectiveDate", "SubmissionType" };
                var reader = FastMember.ObjectReader.Create(data.Select(i => new
                {
                    Number = i.RowIndex,
                    i.RequestNumber,
                    i.Initiator,
                    i.ActivityOwner,
                    CreatedDate = String.Format("{0:dd/MM/yyyy}", i.CreatedDate),
                    StartEffectiveDate = String.Format("{0:dd/MM/yyyy}", i.StartEffectiveDate),
                    EndEffectiveDate = String.Format("{0:dd/MM/yyyy}", i.EndEffectiveDate),
                    SubmissionType = i.AccrueType,
                }), ColumsShow);
                table.Load(reader);

                ExportToExcelHelper.Export("RequestStopAccrue", table);

            }
            catch (Exception ex)
            {

            }
        }

        [Authorize]
        [Route("Dashboard")]
        public ActionResult Dashboard()
        {
            string actionTokenView = "B5C847DA-234E-415C-BC52-0523AEE0CC73";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    if (userCredential.ErrorType > 0)
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    var service = new StopAccrueService();
                    var RoleLabel = new mstStopAccrueRoleLabel();
                    RoleLabel = service.StopAccrueRoleLabel(userCredential.UserRoleID);

                    ViewBag.StopAccrueUser = userCredential.UserID;
                    ViewBag.StopAccrueUserRoleLabel = RoleLabel == null ? "" : RoleLabel.RoleLabel;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("printHeaderRequest")]
        public ActionResult PrintHeaderRequest(PostStopAccrueHeader post)
        {
            DataTable table = new DataTable();

            var service = new StopAccrueService();
            ViewBag.RequestNumber = post.RequestNumber;
            post.RequestNumber = post.RequestNumber.Replace("/", "_");

            string atachment = "";
            var StopAccrueDetail = new List<vwStopAccrueDetail>();
            var StopAccrueDetailPrint = new List<vwStopAccrueDetailPrint>();

            StopAccrueDetail = service.GetDetailRequestList(post.HeaderID);
            StopAccrueDetailPrint = service.GetStopAccrueDetailPrint(post.HeaderID);

            ViewBag.CountTenant = "(" + StopAccrueDetailPrint.Sum(x => x.countTenant) + ")";
            ViewBag.StartEffective = String.Format("{0:dd MMM yyyy}", post.StartEffectiveDate);
            ViewBag.CountMonth = "(" + ((Convert.ToDateTime(post.EndEffectiveDate).Month - Convert.ToDateTime(post.StartEffectiveDate).Month) + 12 * (Convert.ToDateTime(post.EndEffectiveDate).Year - Convert.ToDateTime(post.StartEffectiveDate).Year)) + ")";

            foreach (var item in StopAccrueDetailPrint)
            {
                atachment += "<tr>"
                                + "<td style='border:1px solid black;text-align:center'>" + item.countTenant + "</td>"
                                + "<td style='border:1px solid black'>" + item.CustomerName + "</td>"
                                + "<td style='border:1px solid black'>" + (item.InternalCase == 1 ? "<input type='checkbox' checked>" : "<input type='checkbox'>") + "</td>"
                                + "<td style='border:1px solid black'>" + (item.ExternalCase == 1 ? "<input type='checkbox' checked>" : "<input type='checkbox'>") + "</td>"
                                + "<td style='border:1px solid black'>" + item.DetailCase + "</td>"
                                //+ "<td style='border:1px solid black'>Rp." + String.Format("{0:#,###}", StopAccrueDetail.Sum(x => x.RevenueAmount)) + "</td>"
                                + "<td style='border:1px solid black'>Rp." + String.Format("{0:#,###}", item.TtlRevenue) + "</td>"
                                + "</tr>";
            }
            ViewBag.TotalSumAccrue = " Rp." + String.Format("{0:#,###}", StopAccrueDetail.Sum(x => x.RevenueAmount)) + "";

            var logAct = new List<vwWfHeaderActivityLogs>();
            logAct = service.GetListActivityLogs(post.AppHeaderID);

            ViewBag.StopAccrueChekbox = "<input type='checkbox'  />";
            ViewBag.HoldAccrueChekbox = "<input type='checkbox'  />";
            ViewBag.ReHolAccrueChekbox = "<input type='checkbox' />";

            if (post.RequestType.ToUpper() == "STOP")
            {
                ViewBag.StopAccrueChekbox = "<input type='checkbox' checked />";
                ViewBag.AccrueType = "STOP";
                ViewBag.RequestType2 = "Stop";
            }

            else if (post.RequestType.ToUpper() == "HOLD" && post.IsReHold == false)
            {
                ViewBag.HoldAccrueChekbox = "<input type='checkbox' checked />";
                ViewBag.AccrueType = "HOLD";
                ViewBag.RequestType2 = "Hold";
            }


            else if (post.RequestType.ToUpper() == "HOLD" && post.IsReHold == true)
            {
                ViewBag.ReHolAccrueChekbox = "<input type='checkbox' checked />";
                ViewBag.AccrueType = "ReHOLD";
                ViewBag.RequestType2 = "Hold";
            }

            ViewBag.RequestType = post.RequestType.ToUpper();


            string ApprUser = "";
            string ApprNoted = "";
            string ApprDate = "";
            string ApprJobTitle = "";
            var appr = new vwWfHeaderActivityLogs();

            appr = logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV").OrderByDescending(x => x.LogID).FirstOrDefault();
            if (appr != null)
            {
                ApprUser = appr.name;
                ApprNoted = appr.LogText;
                ApprDate = String.Format("{0:dd MMM yyyy}", appr.LogTime);
                ApprJobTitle = appr.PositionName;
            }
            else
            {
                ApprUser = "";
                ApprNoted = "";
                ApprDate = "";
                ApprJobTitle = "";
            }


            ViewBag.DivHeadApprUser = ApprUser; //logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV").OrderByDescending(x => x.LogID).Select(x => x.name).FirstOrDefault();
            ViewBag.DivHeadApprDate = ApprDate; //String.Format("{0:dd MMM yyyy}", logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV").OrderByDescending(x => x.LogID).Select(x => x.LogTime).FirstOrDefault());
            ViewBag.DivHeadNoted = ApprNoted; // logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV").OrderByDescending(x => x.LogID).Select(x => x.LogText).FirstOrDefault();
            ViewBag.DivHeadJobTitle = ApprJobTitle;

            appr = logAct.Where(x => x.ActivityLabel == "SA_DOC_RECEIVE_DHAS").OrderByDescending(x => x.LogID).FirstOrDefault();
            if (appr != null)
            {
                ApprUser = appr.name;
                ApprNoted = appr.LogText;
                ApprDate = String.Format("{0:dd MMM yyyy}", appr.LogTime);
                ApprJobTitle = appr.PositionName;
            }
            else
            {
                ApprUser = "";
                ApprNoted = "";
                ApprDate = "";
                ApprJobTitle = "";
            }


            if (post.RequestType.ToUpper() == "STOP")
            {
                ViewBag.DivAsetApprDate = "";// logAct.Where(x => x.ActivityLabel == "SA_DOC_RECEIVE_DHAS").OrderByDescending(x => x.LogID).Select(x => x.name).FirstOrDefault();
                ViewBag.DivAsetApprUser = "Mohamad Syarif Tahir";// post.RequestType.ToUpper() == "STOP" ? "Mohamad Syarif Tahir" : ""; //String.Format("{0:dd MMM yyyy}", logAct.Where(x => x.ActivityLabel == "SA_DOC_RECEIVE_DHAS").OrderByDescending(x => x.LogID).Select(x => x.LogTime).FirstOrDefault());
                ViewBag.DivAsetJobTitle = "Asset Assessment Division Head";
            }
            else
            {
                ViewBag.DivAsetApprDate = "";
                ViewBag.DivAsetApprUser = "";
                ViewBag.DivAsetJobTitle = "";
            }


            appr = logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV_ACC").OrderByDescending(x => x.LogID).FirstOrDefault();
            if (appr != null)
            {
                ApprUser = appr.name;
                ApprNoted = appr.LogText;
                ApprDate = String.Format("{0:dd MMM yyyy}", appr.LogTime);
                ApprJobTitle = appr.PositionName;
            }
            else
            {
                ApprUser = "";
                ApprNoted = "";
                ApprDate = "";
                ApprJobTitle = "";
            }

            ViewBag.DivAccApprUser = ApprUser;// logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV_ACC").OrderByDescending(x => x.LogID).Select(x => x.name).FirstOrDefault();
            ViewBag.DivAccNoted = ApprNoted;// logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV_ACC").OrderByDescending(x => x.LogID).Select(x => x.LogText).FirstOrDefault();
            ViewBag.DivAccApprDate = "";
            ViewBag.DivAccJobTitle = ApprJobTitle;// String.Format("{0:dd MMM yyyy}", logAct.OrderBy(x => x.LogTime).Select(x => x.LogTime).FirstOrDefault());

            appr = logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV_CHIEF").OrderByDescending(x => x.LogID).FirstOrDefault();
            if (appr != null)
            {
                ApprUser = appr.name;
                ApprNoted = appr.LogText;
                ApprDate = String.Format("{0:dd MMM yyyy}", appr.LogTime);
                ApprJobTitle = appr.PositionName;
            }
            else
            {
                ApprUser = "";
                ApprNoted = "";
                ApprDate = "";
                ApprJobTitle = "";
            }


            ViewBag.ChiefHeadApprUser = ApprUser;// logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV_CHIEF").OrderByDescending(x => x.LogID).Select(x => x.name).FirstOrDefault();
            ViewBag.ChiefHeadNoted = ApprNoted;// logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV_CHIEF").OrderByDescending(x => x.LogID).Select(x => x.LogText).FirstOrDefault();
            ViewBag.ChiefHeadApprDate = ApprDate;// String.Format("{0:dd MMM yyyy}", logAct.Where(x => x.ActivityLabel == "SA_APPR_DIV_CHIEF").OrderByDescending(x => x.LogID).Select(x => x.LogTime).FirstOrDefault());
            ViewBag.ChiefHeadJobTitle = ApprJobTitle;

            appr = logAct.Where(x => x.ActivityLabel == "SA_APPR_CHIEF_MKT").OrderByDescending(x => x.LogID).FirstOrDefault();
            if (appr != null)
            {
                ApprUser = appr.name;
                ApprNoted = appr.LogText;
                ApprDate = String.Format("{0:dd MMM yyyy}", appr.LogTime);
                ApprJobTitle = appr.PositionName;
            }
            else
            {
                ApprUser = "";
                ApprNoted = "";
                ApprDate = "";
                ApprJobTitle = "";
            }

            ViewBag.ChiefMktApprUser = ApprUser;// logAct.Where(x => x.ActivityLabel == "SA_APPR_CHIEF_MKT").OrderByDescending(x => x.LogID).Select(x => x.name).FirstOrDefault();
            ViewBag.ChiefMktNoted = ApprNoted; //logAct.Where(x => x.ActivityLabel == "SA_APPR_CHIEF_MKT").OrderByDescending(x => x.LogID).Select(x => x.LogText).FirstOrDefault();
            ViewBag.ChiefMktApprDate = ApprDate;// String.Format("{0:dd MMM yyyy}", logAct.Where(x => x.ActivityLabel == "SA_APPR_CHIEF_MKT").OrderByDescending(x => x.LogID).Select(x => x.LogTime).FirstOrDefault());
            ViewBag.ChiefMktJobTitle = ApprJobTitle;

            ViewBag.Summary = atachment;
            atachment = "";
            int noUrut = 1;
            foreach (var item in StopAccrueDetail)
            {
                atachment += "<tr>"
                                    + "<td style='border:1px solid black; text-align:center;padding:2px 3px 2px 3px' valign='top'>" + noUrut + "</td>"
                                    + "<td style='border:1px solid black; text-align:center;padding:2px 3px 2px 3px'  valign='top'>" + item.SONumber + "</td>"
                                    + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.SiteName + "</td>"
                                    + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.Customer + "</td>"
                                    + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.Company + "</td>"
                                    + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.Product + "</td>"
                                    + "<td style='border:1px solid black; text-align:center;padding:2px 3px 2px 3px'  valign='top'>" + String.Format("{0:dd MMM yyyy}", item.RFIDone) + "</td>"
                                    + "<td style='border:1px solid black; text-align:right;padding:2px 3px 2px 3px'  valign='top'>" + String.Format("{0:#,###}", item.RevenueAmount) + "</td>"
                                    + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.CategoryCase + "</td>"
                                    + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.DetailCase + "</td>"
                                    + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.DepartName + "</td>"
                                    + "<td style='border:1px solid black; text-align:center;padding:2px 3px 2px 3px'  valign='top'>" + String.Format("{0:dd MMM yyyy}", item.EffectiveDate) + "</td>"
                                    + "<td style='border:1px solid black; text-align:right;padding:2px 3px 2px 3px' v>" + String.Format("{0:#,###}", item.CapexAmount) + "</td>"
                                    //+ "<td style='border:1px solid black; text-align:center;padding:2px 3px 2px 3px'  valign='top'>-</td>"
                                    + "</tr>";
                noUrut++;
            }
            atachment += "<tr>"
                          + "<td colspan='7' style='border:1px solid black; text-align:center;padding:2px 3px 2px 3px' valign='top'><b>Jumlah</b></td>"
                          + "<td style='border:1px solid black; text-align:right;padding:2px 3px 2px 3px' v><b>Rp. " + String.Format("{0:#,###}", StopAccrueDetail.Sum(x => x.RevenueAmount)) + "</b></td>"
                          + "<td  colspan='4' style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'></td>"
                          + "<td style='border:1px solid black; text-align:right;padding:2px 3px 2px 3px'  valign='top'><b>Rp. " + String.Format("{0:#,###}", StopAccrueDetail.Sum(x => x.CapexAmount)) + "</b></td>"
                          //+ "<td style='border:1px solid black; text-align:center;padding:2px 3px 2px 3px'  valign='top'>-</td>"
                          + "</tr>";

            ViewBag.Attachment1 = atachment;
            atachment = "";
            noUrut = 1;
            foreach (var item in StopAccrueDetail)
            {
                atachment += "<tr>"
                                + "<td style='border:1px solid black;text-align:center;padding:2px 3px 2px 3px'  valign='top'>" + noUrut + "</td>"
                                + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.SONumber + "</td>"
                                + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.SiteName + "</td>"
                                + "<td style='border:1px solid black;padding:2px 3px 2px 3px'  valign='top'>" + item.Remarks + "</td>"
                                + "</tr>";
                noUrut++;
            }
            ViewBag.Attachment2 = atachment;

            string jsScript = "<script>function subst() { var vars = { };var x = document.location.search.substring(1).split('&');";
            jsScript += "for(var i in x) { var z = x[i].split('=', 2); vars[z[0]] = unescape(z[1]); }";
            jsScript += "var x =['frompage', 'topage', 'page', 'webpage', 'section', 'subsection', 'subsubsection'];";
            jsScript += "for(var i in x){";
            jsScript += "var y = document.getElementsByClassName(x[i]);";
            jsScript += "for (var j = 0; j < y.length; ++j) y[j].textContent = vars[x[i]];}}</script > ";
            string customeSwitches = "";
            string footerHtml = "";
            footerHtml += "<!DOCTYPE html><html><head>";
            footerHtml += jsScript + "</head><body onload='subst()'>";
            footerHtml += "<table style='width:100%'>";
            footerHtml += "<td style='width:50px'> <img src='" + GenerateQRCode(ViewBag.RequestNumber, "StopAccrue_" + post.HeaderID + ".jpg") + "' width='50px' height='50px' style='float:right'/>";
            footerHtml += "</td>";
            footerHtml += "</tr>";
            footerHtml += "</table>";
            footerHtml += "</body></html>";

            string footerPath = Server.MapPath("~/Views/StopAccrue/_PrintFooter.html");
            System.IO.File.WriteAllText(footerPath, footerHtml);
            customeSwitches = string.Format("--print-media-type --footer-html  \"{0}\" --header-spacing \"20\" ", footerPath);


            return new Rotativa.ViewAsPdf("~/Views/StopAccrue/PrintRequest.cshtml")
            {
                FileName = post.RequestNumber + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(10, 5, 15, 5),
                MinimumFontSize = 8,
                CustomSwitches = customeSwitches
            };


        }
        private string GenerateQRCode(string textQr, string fileName)
        {
            string pathDOc;
            string fileDoc;
            //=====================================================================//
            pathDOc = Server.MapPath(Helper.Helper.GetDocPath() + @"StopAccrue\QRCode");
            ThoughtWorks.QRCode.Codec.QRCodeEncoder qrCode = new ThoughtWorks.QRCode.Codec.QRCodeEncoder();
            System.Drawing.Image bitMap;
            //=====================================================================//
            qrCode.QRCodeEncodeMode = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCode.QRCodeScale = 7;
            qrCode.QRCodeVersion = 0;
            qrCode.QRCodeErrorCorrect = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.Q;
            bitMap = qrCode.Encode(textQr);
            // =====================================================================//
            fileDoc = pathDOc + @"\" + fileName;
            if (Directory.Exists(pathDOc) == false)
            {
                Directory.CreateDirectory(pathDOc);
            }
            //  ==== save file ====== //
            if (System.IO.File.Exists(fileDoc) == true)
            {
                System.IO.File.Delete(fileDoc);
                bitMap.Save(fileDoc);
            }
            else
            {
                bitMap.Save(fileDoc);
            }
            bitMap.Dispose();
            return fileDoc;
        }
    }
}