using ARSystemFrontEnd.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ARSystemFrontEnd.Helper;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystem.Service;
using ARSystemFrontEnd.Models;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("StopAccrue")]
    public class DashboardStopAccrueController : BaseController
    {
        [Authorize]
        [Route("DashboardStopAccrue")]
        public ActionResult DashboardStopAccrue()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            return View();
        }

        [HttpGet, Route("exportDashboardStopAccrueDetail")]
        public void ExportDashboardDetail()
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

        [HttpGet, Route("exportAllDashboardDetail")]
        public void ExportToExcelAllDashboardDetail()
        {
            string SubmissionFrom = Request.QueryString["SubmissionFrom"];
            string SubmissionTo = Request.QueryString["SubmissionTo"];
            string AccrueType = "HOLD";//Request.QueryString["AccrueType"];
            //string Activity = "Ongoing" ; //Request.QueryString["Activity"];
            string DirectorateCode = Request.QueryString["DirectorateCode"];
            string GroupBy = Request.QueryString["GroupBy"];

            List<vwStopAccrueDashboardDetail> DetailHolder = new List<vwStopAccrueDashboardDetail>();
            var service = new DashboardStopAccrueService();
            DetailHolder = service.GetListExportAllDashboardDetail(SubmissionFrom, SubmissionTo, DirectorateCode, AccrueType, GroupBy);
            try
            {
                DataTable table = new DataTable();
                string[] ColumsShow = new string[] { "Number", "Activity","RequestNumber", "SubmissionType", "Company", "SONumber", "SiteName", "Customer", "TenantType", "RFIDate", "SaldoAccrue_mio", "Capex_mio", "CategoryCase", "DetailCase", "Compensation_mio", "PIC", "StartDate", "EndDate", "Initiator", "Department" };
                if (GroupBy == "Department")
                {
                    var reader = FastMember.ObjectReader.Create(DetailHolder.Select(i => new
                    {
                        Number = i.RowIndex,
                        Activity = i.Activity,
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
                        Department = i.DepartName

                    }), ColumsShow);
                    table.Load(reader);
                }
                else
                {
                    var reader = FastMember.ObjectReader.Create(DetailHolder.Select(i => new
                    {
                        Number = i.RowIndex,
                        Activity = i.Activity,
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
                        PIC = i.DetailCase,
                        StartDate = String.Format("{0:dd/MM/yyyy}", i.StartEffectiveDate),
                        EndDate = String.Format("{0:dd/MM/yyyy}", i.EndEffectiveDate),
                        Initiator = i.Initiator,
                        Department = i.DepartName

                    }), ColumsShow);
                    table.Load(reader);
                }
                
                
                //var reader = FastMember.ObjectReader.Create(DetailHolder, "StatusProgress", "SONumber", "SiteName", "Operator", "Period", "Year", "ExpenseNumber"
                //    , "ApprovalAPDate", "ApprovalARDate", "InvoiceDate", "PaidDate", "Description", "Amount");

                //table.Load(reader);
                ExportToExcelHelper.Export(GroupBy, table);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        [HttpGet, Route("exportAllDashboardDetailByDate")]
        public void ExportToExcelDetailByDate()
        {
            string SubmissionFrom = Request.QueryString["SubmissionFrom"];
            string SubmissionTo = Request.QueryString["SubmissionTo"];
            string DirectorateCode = Request.QueryString["DirectorateCode"];
            string GroupBy = Request.QueryString["GroupBy"];
            string AccrueType = "HOLD";
            List<vwStopAccrueExportToExcel> DetailHolder = new List<vwStopAccrueExportToExcel>();
            var service = new DashboardStopAccrueService();
            DetailHolder = service.GetListExportDetailByDate(SubmissionFrom, SubmissionTo, DirectorateCode, AccrueType, GroupBy);
            //try
            //{
            //    DataTable table = new DataTable();
            //    string[] ColumsShow = new string[] { "actualstatus", "inputreqBy", "RequestNumber", "AppHeaderID", "Submit","VerificationDivHead", "Activtydivhead",
            //        "VerifiedbyChief", "Activtychief", "RecomendedbyChiefofMarketing", "Activtychiefmkt", "FeedbackDepHeadREAlatest",
            //        "ActivtydeptREAlatest", "FeedbackDepHeadREA2ndlatest", "ActivtydeptREA2ndlatest", "FeedbackfromDeptAcc", "ActivtydeptACC",
            //        "FeedbackVerificationDivControllershiplatest", "ActivtydivCONTlatest", "FeedbackVerificationDivControllership2ndlatest", "ActivtydivCONT2ndlatest",
            //        "FeedbackfromDivAcc", "ActivtydivACC", "SubmitDocument", "ActivtysubmitDoc", "DocumentReceivebyAccounting", "ActivtydocACC", "DocumentReceivebyAset",
            //        "ActivtydocAST", "Finish", "ActivtyFinish", "Rejected", "ActivtyReject"};
            //    var reader = FastMember.ObjectReader.Create(DetailHolder.Select(i => new
            //    {
            //        i.ActualStatus, i.InputReqBy, i.RequestNumber, i.AppHeaderID, i.VerificationDivHead, i.ActivtyDivHead,
            //        i.VerifiedByChief, i.ActivtyChief,i.RecomendedByChiefOfMarketing, i.ActivtyChiefMKT,i.FeedbackDepHeadREALatest,
            //        i.ActivtyDeptREALatest, i.FeedbackDepHeadREA2ndLatest,i.FeedbackFromDeptAcc,i.ActivtyDeptACC,
            //        i.FeedbackVerificationDivControllershipLatest,  i.ActivtyDivCONTLatest,i.FeedbackVerificationDivControllership2ndlatest,i.ActivtyDivCONT2ndLatest,
            //        i.FeedbackFromDivAcc, i.ActivtyDivACC,i.SubmitDocument,i.ActivtySubmitDoc,i.DocumentReceiveByAccounting, i.ActivtyDocACC, i.DocumentReceiveByAset,
            //        i.ActivtyDocAST, i.Finish, i.ActivtyFinish, i.Rejected, i.ActivtyReject
            //    }), ColumsShow);
            //    table.Load(reader);
            //    ExportToExcelHelper.Export(GroupBy, table);
            //}
            //catch (Exception ex)
            //{

            //}
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(DetailHolder, true);
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=AttachmentExportByDate.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }


        [Authorize]
        [HttpGet, Route("exportDashboardStopAccrueHeader")]
        public void ExportDashboardHeader(PostStopAccrueHeader post)
        {
            DataTable table = new DataTable();
            try
            {

                var service = new DashboardStopAccrueService();
                var data = new List<vwStopAccrueDashboardHeader>();
                var param = new vwStopAccrueDashboardHeader();
                param.RequestTypeID = post.RequestTypeID;
                param.RequestNumber = post.RequestNumber;
                param.SubmissionDateFrom = post.SubmissionDateFrom;
                param.SubmissionDateTo = post.SubmissionDateTo;
                param.DepartName = post.DepartName;
                param.Activity = post.Activity;
                param.AccrueType = post.AccrueType;
                param.DirectorateCode = post.DirectorateCode;
                param.DetailCase = post.DetailCase;

                data = service.GetDashboardHeader(param, post.start, post.length);
                
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
    }
}