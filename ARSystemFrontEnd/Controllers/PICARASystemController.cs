using ARSystem.Domain.Models;
using ARSystem.Service;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("RevenueAssurance")]
    public class PICARASystemController : BaseController
    {
        [Authorize]
        [Route("PICARASystem")]
        public ActionResult PICARASystem()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            return View();
        }

        [Authorize]
        [HttpGet, Route("ExportHeader")]
        public void ExportHeader()
        {
            DataTable table = new DataTable();
            try
            {

                var service = new PICARASystemService();
                var data = new List<vwRADashboardPICA>();
                var param = new vwRADashboardPICA();
                

                param.CompanyId = Request.QueryString["Company"];
                param.CustomerId = Request.QueryString["Customer"];
                param.StipSiro = Request.QueryString["StipSiro"];
                param.ProductID = Convert.ToInt32(Request.QueryString["ProductID"]);
                param.BapsType = Request.QueryString["BapsType"];
                param.ActivityName = Request.QueryString["ActivityName"];
                param.SONumber = Request.QueryString["SONumber"];

                //int intTotalRecord = service.GetCountListHeader(param);
                //int page = 1000;
                //int intBatch = intTotalRecord / page;

                data = service.GetListHeader(param);

                string[] ColumsShow = new string[] {"SONumber", "SiteId", "SiteName", "SiteIdOpr", "SiteNameOpr", "CustomerId", "CompanyId", "StipSiro", "BapsType", "StartBapsDate", "EndBapsDate", "StartDateInvoice", "EndDateInvoice", "AmountRental", "AmountService", "TotalAmountInvoice", "StatusActivity" };
                var reader = FastMember.ObjectReader.Create(data.Select(i => new
                {
                    //Number = i.RowIndex,
                    i.SONumber,
                    i.SiteId,
                    i.SiteName,
                    i.SiteIdOpr,
                    i.SiteNameOpr,
                    i.CustomerId,
                    i.CompanyId,
                    i.StipSiro,
                    i.BapsType,
                    i.StartBapsDate,
                    i.EndBapsDate,
                    i.StartDateInvoice,
                    i.EndDateInvoice,
                    i.AmountRental,
                    i.AmountService,
                    TotalAmountInvoice = i.InvoiceAmount,
                    StatusActivity = i.ActivityName

                }), ColumsShow);
                table.Load(reader);

                ExportToExcelHelper.Export("BapsNewHeader", table);
            }
            catch (Exception ex)
            {

            }
        }

        [Authorize]
        [HttpGet, Route("ExportDetail")]
        public void ExportDetail()
        {
            DataTable table = new DataTable();
            try
            {

                var service = new PICARASystemService();
                var data = new List<vwRADashboardPICA>();
                var param = new vwRADashboardPICA();


                param.CompanyId = Request.QueryString["Company"];
                param.CustomerId = Request.QueryString["Customer"];
                param.StipSiro = Request.QueryString["StipSiro"];
                param.ProductID = Convert.ToInt32(Request.QueryString["ProductID"]);
                param.BapsType = Request.QueryString["BapsType"];
                param.ActivityName = Request.QueryString["ActivityName"];
                param.SONumber = Request.QueryString["SONumber"];

                //int intTotalRecord = service.GetCountListHeader(param);
                //int page = 1000;
                //int intBatch = intTotalRecord / page;

                data = service.GetListDetail(param);

                string[] ColumsShow = new string[] { "Number", "StatusActivity", "Durasi", "StartTarget", "EndTarget", "StartActual", "EndActual", "LTActual"};
                var reader = FastMember.ObjectReader.Create(data.Select(i => new
                {
                    Number = i.RowIndex,
                    StatusActivity = i.ActivityName,
                    i.Durasi,
                    i.StartTarget,
                    i.EndTarget,
                    i.StartActual,
                    i.EndActual,
                    i.LTActual                    
                }), ColumsShow);
                table.Load(reader);

                ExportToExcelHelper.Export("BapsNewDetail", table);
            }
            catch (Exception ex)
            {

            }
        }

        [Authorize]
        [HttpGet, Route("ExportHistoryPICA")]
        public void ExportHistoryPICA()
        {
            DataTable table = new DataTable();
            try
            {

                var service = new PICARASystemService();
                var data = new List<vwRADashboardPICA>();
                var param = new vwRADashboardPICA();

                
                param.StipSiro = Request.QueryString["StipSiro"];
                param.SONumber = Request.QueryString["SONumber"];

                //int intTotalRecord = service.GetCountListHeader(param);
                //int page = 1000;
                //int intBatch = intTotalRecord / page;

                data = service.GetHistoryPICA(param);

                string[] ColumsShow = new string[] { "Number", "SONumber","SiteId", "SiteName", "SiteIdOpr", "SiteNameOpr", "ActivityName", "CategoryPICA", "PICA", "DetailPICA", "StipSiro", "TargetPICA", "CreatedDate", "CreatedBy"};
                var reader = FastMember.ObjectReader.Create(data.Select(i => new
                {
                    Number = i.RowIndex,
                    i.SONumber,
                    i.SiteId,
                    i.SiteName,
                    i.SiteIdOpr,
                    i.SiteNameOpr,
                    i.ActivityName,
                    i.CategoryPICA,
                    i.PICA,
                    i.TargetPICA,
                    i.DetailPICA,
                    i.StipSiro,
                    i.CreatedDate,
                    i.CreatedBy,
                }), ColumsShow);
                table.Load(reader);

                ExportToExcelHelper.Export("HistoryPICA", table);
            }
            catch (Exception ex)
            {

            }
        }

    }
}