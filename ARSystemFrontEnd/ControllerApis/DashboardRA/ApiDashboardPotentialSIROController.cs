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
    [RoutePrefix("api/DashboardPotentialSIRO")]
    public class ApiDashboardPotentialSIROController : ApiController
    {
        private readonly DashboardPotentialSIROService summary;
        private readonly DashboardTSELService DashboardTSELService;
        private readonly DashboardTSELOverdueService DashboardTSELOverdueService;

        //DashboardTSELOverdueService DashboardTSELService = new DashboardTSELOverdueService();
   
        public ApiDashboardPotentialSIROController()
        {
            DashboardTSELOverdueService = new ARSystem.Service.DashboardTSELOverdueService();
            DashboardTSELService = new ARSystem.Service.DashboardTSELService();
            summary = new ARSystem.Service.DashboardPotentialSIROService();
        }

        [HttpGet, Route("GetSummary")]
        public IHttpActionResult GetOutstandingSummaryYear(
            string Type,
            string Key,
            string ProductID,
            string STIPID,
            string Customer,
            string CompanyID,
            string Year,
            string Month,
            string Desc

            )
        {
            var table = summary.GetDashboardSummaryList(
                                                        Type,
                                                        Key,
                                                        ProductID,
                                                        STIPID,
                                                        Customer,
                                                        CompanyID,
                                                        Year,
                                                        Month,
                                                        Desc
                                                        );
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

        [HttpPost, Route("GetSIROProgressSummary")]
        public IHttpActionResult GetSIROProgressSummary
            (
               PostSIRO p
            )
        {
            try
            {
                var CountOfRows = "1";
                var data = summary.GetDashboardSummaryList(
                        p.Type,
                        p.Key,
                        p.ProductID,
                        p.STIPID,
                        p.Customer,
                        p.CompanyID,
                        p.Year,
                        p.Month,
                        p.Desc
                    );

                return Ok(new { draw = 1000, recordsTotal = CountOfRows, recordsFiltered = CountOfRows, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex.InnerException);
            }
          
        }


        [HttpGet, Route("GetSummaryMonth")]
        public IHttpActionResult GetOutstandingSummaryMonth(
            string Type,
                string Key,
                string ProductID,
                string STIPID,
                string Customer,
                string CompanyID,
                string Year,
                string Month,
                string Desc

            )
        {
            var table = summary.GetDashboardSummaryList(
               Type,
                Key,
                ProductID,
                STIPID,
                Customer,
                CompanyID,
                Year,
                Month,
                Desc

                );
            string[] columnList = new string[table.Columns.Count];

            int idx = 0;
            List<string> custom = new List<string>();

            string columnHEad = "<tr><th rowspan='2' class='datatable-col-100' style='vertical-align:middle;text-align:center;'>Description </th>";
            columnHEad += "<th colspan='12' class='datatable-col-300' style='vertical-align:middle;text-align:center;'>Billing Month " + Year + "</th>";
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

        [HttpPost, Route("GetDetail")]
        public IHttpActionResult GetDetailOutstanding(PostSIRO post)
        {
            try
            {
                string strWhereClause = " 1=1  AND CustomerID IS NOT NULL AND YearCategory != 'Others'";

                if (post.Desc != null )
                {
                    if (post.Type == "SIROGroupByOperator")
                        strWhereClause += " AND CustomerID = '" + post.Desc.Trim() + "'";
                    else if (post.Type == "SIROGroupByProduct")
                        strWhereClause += " AND ProductName  = '" + post.Desc.Trim() + "'";
                    else if (post.Type == "SIROGroupByStipCategory")
                        strWhereClause += " AND StipCategory  = '" + post.Desc + "'";
                    else if (post.Type == "SIROGroupByRegional")
                        strWhereClause += " AND RegionName  = '" + post.Desc + "'";
                    else if (post.Type == "SIROGroupByCompany")
                        strWhereClause += " AND  CompanyInvoice = '" + post.Desc + "'";
                     
                }
               
                if (post.Year != null)
                {
                    strWhereClause += " AND YearCategory = '" + post.Year + "'";
                }
                if (post.Month != null && post.Month != "13" && post.Step == null)
                {
                    strWhereClause += " AND MONTH(EndBapsDate) = '" + fnGetMonthHeaderString(post.Month) + "'";
                }

                
                if (post.ProductID != null)
                    strWhereClause += " AND ProductID = " + post.ProductID;
                if (post.STIPID != null)
                    strWhereClause += " AND STIPID = " + post.STIPID;
                if (post.Customer != null)
                    strWhereClause += " AND CustomerID = '" + post.Customer + "' "; ;
                if (post.CompanyID != null)
                    strWhereClause += " AND Company = '" + post.CompanyID +"'";
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
                if (post.Step != null)
                {
                    strWhereClause += " AND SIROProgress ='" + post.Step + "' ";                     
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
    }
}


    