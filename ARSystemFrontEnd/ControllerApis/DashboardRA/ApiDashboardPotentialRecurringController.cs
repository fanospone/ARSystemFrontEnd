using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using System.Threading.Tasks;
using ARSystemFrontEnd.Helper;
using System.Data;
using Newtonsoft.Json;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/DashboardPotentialRecurring")]
    public class ApiDashboardPotentialRecurringController : ApiController
    {
        private readonly DashboardPotentialRecurringService summary;
        private readonly DashboardTSELService DashboardTSELService;
        private readonly DashboardTSELOverdueService DashboardTSELOverdueService;

        //DashboardTSELOverdueService DashboardTSELService = new DashboardTSELOverdueService();
        private const string CacheKeySection = "MstRASection";
        private const string CacheKeySOW = "MstRASOW";
        public ApiDashboardPotentialRecurringController()
        {
            DashboardTSELOverdueService = new ARSystem.Service.DashboardTSELOverdueService();
            DashboardTSELService = new ARSystem.Service.DashboardTSELService();
            summary = new ARSystem.Service.DashboardPotentialRecurringService();
        }

        [HttpGet, Route("GetSummary")]
        public IHttpActionResult GetOutstandingSummaryYear(string type, int? STIPDate, int? RFIDate, string SectionID, string SOWID, int? ProductID, int? STIPID, int? RegionalID, string CompanyID, string Customer)
        {
            var table = summary.GetDashboardSummaryList(type, STIPDate, RFIDate, SectionID, SOWID, ProductID, STIPID, RegionalID, CompanyID, "", "", "", Customer);
            string[] columnList = new string[table.Columns.Count];

            int idx = 0;
            List<string> custom = new List<string>();

            string columnHEad = "<tr><th rowspan='2' class='datatable-col-100' style='vertical-align:middle;text-align:center;'>Description </th>";
            columnHEad += "<th colspan='6' class='datatable-col-300' style='vertical-align:middle;text-align:center;'>Billing Year</th>";
            columnHEad += "<th rowspan='2' class='datatable-col-50' style='vertical-align:middle;text-align:center;'>Grand Total</th>";
            columnHEad += "</tr>";
            columnHEad += "<tr>";
            foreach (DataColumn column in table.Columns)
            {
                custom.Add(column.ColumnName);
                if (idx > 0 && idx < 7)
                {
                    columnHEad += "<td class='text-center'><b>" + column.ColumnName + "</b></td>";
                }

                idx += 1;
            }
            columnHEad += "</tr>";

            var json = JsonConvert.SerializeObject(table);

            return Ok(new { dataSummary = json, tblHead = columnHEad, customcolumn = custom });
        }
        [HttpGet, Route("GetSummaryMonth")]
        public IHttpActionResult GetOutstandingSummaryMonth(string type, int? STIPDate, int? RFIDate, string SectionID, string SOWID, int? ProductID, int? STIPID, int? RegionalID, string CompanyID, string year, string month, string desc, string Customer)
        {
            var table = summary.GetDashboardSummaryList(type, STIPDate, RFIDate, SectionID, SOWID, ProductID, STIPID, RegionalID, CompanyID, year, month, desc, Customer);
            string[] columnList = new string[table.Columns.Count];

            int idx = 0;
            List<string> custom = new List<string>();

            string columnHEad = "<tr><th rowspan='2' class='datatable-col-100' style='vertical-align:middle;text-align:center;'>Description </th>";
            columnHEad += "<th colspan='12' class='datatable-col-300' style='vertical-align:middle;text-align:center;'>Billing Month "+year+"</th>";
            columnHEad += "<th rowspan='2' class='datatable-col-50' style='vertical-align:middle;text-align:center;'>Total</th>";
            columnHEad += "</tr>";
            columnHEad += "<tr>";
            foreach (DataColumn column in table.Columns)
            {
                custom.Add(column.ColumnName);
                if (idx > 0 && idx < 13)
                {
                    columnHEad += "<td class='text-center'  style='vertical-align:middle;text-align:center;'><b>" + fnGetMonthHeader(column.ColumnName) + "</b></td>";
                }

                idx += 1;
            }
            columnHEad += "</tr>";

            var json = JsonConvert.SerializeObject(table);

            return Ok(new { dataSummary = json, tblHead = columnHEad, customcolumn = custom });
        }

      
        [HttpPost, Route("GetDetail")]
        public IHttpActionResult GetDetailOutstanding(ARSystemFrontEnd.Models.PostRecurring post)
        {
            try
            {
                string strWhereClause = " 1=1 AND  DepartmentName !='Not Mapping'  AND CustomerID IS NOT NULL AND TenantType IS NOT NULL ";

                if (post.paramRow != null)
                {
                    if (post.type == "GroupBySection")
                        strWhereClause += " AND DepartmentName = '" + post.paramRow.Trim() + "'";
                    else if (post.type == "GroupBySOW")
                        strWhereClause += " AND TenantType = '" + post.paramRow.Trim() + "'";
                    else if (post.type == "GroupByProduct")
                        strWhereClause += " AND ProductName = '" + post.paramRow + "'";
                    else if (post.type == "GroupBySTIPCategory")
                        strWhereClause += " AND StipCategory = '" + post.paramRow + "'";
                    else if (post.type == "GroupByRegional")
                        strWhereClause += " AND RegionName = '" + post.paramRow + "'";
                    else if (post.type == "GroupByOperator")
                        strWhereClause += " AND CustomerInvoice = '" + post.paramRow + "'";
                    else if (post.type == "GroupByCompany")
                        strWhereClause += " AND CustomerID = '" + post.paramRow + "'";
                    
                }
                if (post.YearBill != null)
                {
                    strWhereClause += " AND YearInvoiceCategory = '" + post.YearBill + "'";
                }
                if (post.paramColumn != null && post.paramColumn != "13" && post.paramColumn.Length != 0)
                {
                    strWhereClause += " AND MONTH(StartInvoiceDate) = '" + fnGetMonthHeaderString(post.paramColumn) + "'";
                }

                if (post.STIPDate != null)
                    strWhereClause += " AND YEAR(StipDate) = '" + post.STIPDate.ToString() + "'";
                if (post.RFIDate != null)
                    strWhereClause += " AND YEAR(RFIDate) = '" + post.RFIDate.ToString() + "'";
                if (post.SecName != null)
                    strWhereClause += " AND DepartmentCode = '" + post.SecName.ToString().Trim() + "'";
                if (post.SOWName != null)
                    strWhereClause += " AND TenantType = '" + post.SOWName.ToString().Trim() + "'";
                if (post.ProductID != null)
                    strWhereClause += " AND ProductID = " + post.ProductID;
                if (post.STIPID != null)
                    strWhereClause += " AND STIPID = " + post.STIPID;
                if (post.RegionalID != null)
                    strWhereClause += " AND RegionID = " + post.RegionalID;
                if (post.CompanyID != null)
                    strWhereClause += " AND CompanyInvoice = '" + post.CompanyID + "'";
                if (post.Customer != null)
                    strWhereClause += " AND CustomerID = '" + post.Customer + "'";
                
                if (post.SoNumber != null && post.SoNumber != "")
                {
                    strWhereClause += " AND SoNumber = '" + post.SoNumber + "'";
                }

                if (post.SiteID != null && post.SiteID != "")
                {
                    strWhereClause += " AND SiteID = '" + post.SiteID + "'";
                }

                if (post.SiteName != null && post.SiteName != "")
                {
                    strWhereClause += " AND SiteName LIKE '%" + post.SiteName + "%'";
                }
                var CountOfRows = summary.GetDetailCount(strWhereClause);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();


                var data = summary.GetDetailList(strWhereClause, post.start, post.length, strOrderBy);

                return Ok(new { draw = post.draw, recordsTotal = CountOfRows, recordsFiltered = CountOfRows, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex.InnerException);
            }
        }


        #region Master
        [HttpGet, Route("customer")]
        public IHttpActionResult GetCustomerToList()
        {
            try
            {
                //List<MstRASectionProduct> section = new List<MstRASectionProduct>();
                //section = HttpRuntime.Cache.GetOrStore<List<MstRASectionProduct>>($"User{CacheKeySection}", () => DashboardTSELService.GetSectionToList(UserManager.User.UserToken, "").ToList())
                //        .ToList();

                List<MstInitialDepartment> section = new List<MstInitialDepartment>();
                section = summary.GetCustomer().ToList();

                return Ok(section);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpGet, Route("Section")]
        public IHttpActionResult GetSectionToList()
        {
            try
            {
                //List<MstRASectionProduct> section = new List<MstRASectionProduct>();
                //section = HttpRuntime.Cache.GetOrStore<List<MstRASectionProduct>>($"User{CacheKeySection}", () => DashboardTSELService.GetSectionToList(UserManager.User.UserToken, "").ToList())
                //        .ToList();

                List<MstInitialDepartment> section = new List<MstInitialDepartment>();
                section = summary.GetSection().ToList();

                return Ok(section);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpGet, Route("SOW")]
        public IHttpActionResult GetSOWToList()
        {
            try
            {
                //var sow = HttpRuntime.Cache.GetOrStore<List<MstRASowProduct>>($"User{CacheKeySOW}", () => DashboardTSELService.GetSOWToList(UserManager.User.UserToken, "").ToList())
                //        .Where(w => w.SectionProductId.ToString() == SectionID).ToList();

                List<MstInitialDepartment> sow = new List<MstInitialDepartment>();
                sow = summary.GetSOW().ToList();

                return Ok(sow);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpGet, Route("STIPCategory")]
        public IHttpActionResult GetSTIPCategoryToList()
        {
            try
            {
                var result = DashboardTSELOverdueService.GetMasterSTIPListDropdown().ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("CompanyList")]
        public IHttpActionResult GetCompanyToList()
        {
            try
            {
                var result = DashboardTSELOverdueService.GetMasterCompanyListDropdown().ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ProductList")]
        public IHttpActionResult GetProductToList(string sectionID, string sowID)
        {
            try
            {
                var result = DashboardTSELOverdueService.GetProductListDropdown(sectionID, sowID).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("SOWbyParamList")]
        public IHttpActionResult GetSOWbyParamToList(string sectionID)
        {
            try
            {
                var result = DashboardTSELOverdueService.GetSOWListDropdown(sectionID).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion



        static string fnGetMonthHeader(string MonthNumber)
        {
            string month = "";
            switch (MonthNumber)
            {
                case "1":
                    month = "JAN";
                    break;
                case "2":
                    month = "FEB";
                    break;
                case "3":
                    month = "MAR";
                    break;
                case "4":
                    month = "APR";
                    break;
                case "5":
                    month = "MAY";
                    break;
                case "6":
                    month = "JUN";
                    break;
                case "7":
                    month = "JUL";
                    break;
                case "8":
                    month = "AUG";
                    break;
                case "9":
                    month = "SEP";
                    break;
                case "10":
                    month = "OCT";
                    break;
                case "11":
                    month = "NOV";
                    break;
                case "12":
                    month = "DEC";
                    break;
                default:
                    month = MonthNumber;
                    break;
            }
            return month;
        }

        static string fnGetMonthHeaderString(string MonthText)
        {
            string month = "";
            switch (MonthText)
            {
                case "JAN":
                    month = "1";
                    break;
                case "FEB":
                    month = "2";
                    break;
                case "MAR":
                    month = "3";
                    break;
                case "APR":
                    month = "4";
                    break;
                case "MAY":
                    month = "5";
                    break;
                case "JUN":
                    month = "6";
                    break;
                case "JUL":
                    month = "7";
                    break;
                case "AUG":
                    month = "8";
                    break;
                case "SEP":
                    month = "9";
                    break;
                case "OCT":
                    month = "10";
                    break;
                case "NOV":
                    month = "11";
                    break;
                case "DEC":
                    month = "12";
                    break;
                default:
                    month = MonthText;
                    break;
            }
            return month;
        }
    }
}