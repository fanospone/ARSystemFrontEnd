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

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("AccountPayable")]
    public class AccountPayableController : BaseController
    {
        private readonly SummaryDataAPService summary;

        public AccountPayableController()
        {
            summary = new SummaryDataAPService();
        }

        #region Routes
        [Authorize]
        [Route("SummaryAP")]
        public ActionResult SummaryAP()
        {
            string actionTokenView = "12B3E85B-59F1-4E5F-A3EE-68DB97E16949";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.UserFullNameLogin = UserManager.User.UserFullName;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        #endregion

        #region Export
        [Route("ExportTenant")]
        public void ExportTenant()
        {
            //Parameter
            string strProduct = Request.QueryString["strProduct"].ToString();

            var dataresult = summary.GetListSummaryDataAPTenant(strProduct);

            string[] fieldList = new string[] {
                        "RowIndex",
                        "SONumber",
                        "SiteID",
                        "SiteName",
                        "RegionalName",
                        "StipCategory",
                        "Product",
                        "TotalCost",
                        "TotalRevenue"
                    };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataresult, fieldList);
            table.Load(reader);

            table.Columns["RowIndex"].ColumnName = "No";
            table.Columns["SONumber"].ColumnName = "SO Number";
            table.Columns["SiteID"].ColumnName = "Site ID";
            table.Columns["SiteName"].ColumnName = "Site Name";
            table.Columns["RegionalName"].ColumnName = "Regional";
            table.Columns["StipCategory"].ColumnName = "Stip Category";
            table.Columns["TotalCost"].ColumnName = "Total Amount Cost";
            table.Columns["TotalRevenue"].ColumnName = "Total Amount Revenue";

            ExportToExcelHelper.Export("TenantList", table);
            
        }

        [Route("ExportTenantCost")]
        public void ExportTenantCost()
        {
            //Parameter
            string strSONumber = Request.QueryString["strSONumber"].ToString();

            var dataresult = summary.GetListDataAPTenantCost(strSONumber);

            string[] fieldList = new string[] {
                        "RowIndex" ,
                        "SourceData" ,
                        "DocumentNumber" ,
                        "Termin" ,
                        "VOUCHER" ,
                        "TRANSDATE" ,
                        "InvoiceNumber" ,
                        "Amount", 
                        "Description" 
                    };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataresult, fieldList);
            table.Load(reader);

            table.Columns["RowIndex"].ColumnName = "No";
            table.Columns["Termin"].ColumnName = "Appr Fin Date";
            table.Columns["InvoiceNumber"].ColumnName = "Invoice";
            table.Columns["DocumentNumber"].ColumnName = "Doc Number";

            ExportToExcelHelper.Export("TenantCostList", table);

        }

        [Route("ExportTenantRevenue")]
        public void ExportTenantRevenue()
        {
            //Parameter
            string strSONumber = Request.QueryString["strSONumber"].ToString();

            var dataresult = summary.GetListDataAPTenantRevenue(strSONumber);

            string[] fieldList = new string[] {
                        "RowIndex",
                        "TypePayment",
                        "CustomerID",
                        "InvoiceNumber",
                        "Period",
                        "Amount"
                    };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataresult, fieldList);
            table.Load(reader);

            table.Columns["RowIndex"].ColumnName = "No";
            table.Columns["TypePayment"].ColumnName = "Revenue Type";
            table.Columns["Period"].ColumnName = "Period Invoice";
            table.Columns["CustomerID"].ColumnName = "Customer ID";
            table.Columns["InvoiceNumber"].ColumnName = "Invoice Number";

            ExportToExcelHelper.Export("TenantRevenueList", table);

        }
        #endregion
    }
}