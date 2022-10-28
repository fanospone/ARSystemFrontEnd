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
    [RoutePrefix("api/DashboardPotentialRFITo1stBAPS")]
    public class ApiDashboardPotentialRFITo1stBAPSBillingController : ApiController
    {
        private readonly DashboardPotentialRFITo1stBAPSBillingService summary;
        private readonly DashboardTSELService DashboardTSELService;
        private readonly DashboardTSELOverdueService DashboardTSELOverdueService;
        public ApiDashboardPotentialRFITo1stBAPSBillingController()
        {
            DashboardTSELOverdueService = new ARSystem.Service.DashboardTSELOverdueService();
            DashboardTSELService = new ARSystem.Service.DashboardTSELService();
            summary = new ARSystem.Service.DashboardPotentialRFITo1stBAPSBillingService();
        }

        [HttpGet, Route("GetSummary")]
        public IHttpActionResult GetDashboardSummaryList(string Type, string STIP, string year, string month, string desc)
        {
            try
            {
                DataTable result = summary.GetDashboardSummaryList(Type, STIP, year, month, desc);
                //var json = JsonConvert.SerializeObject(result);

                return Ok(
                    new { dataSummary = result });            
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpGet, Route("GetSummaryMonth")]
        public IHttpActionResult GetSummaryMonth(string Type, string STIP, string year, string month, string desc)
        {
            var table = summary.GetDashboardSummaryList(Type, STIP, year,month,desc);
            string[] columnList = new string[table.Columns.Count];

            int idx = 0;
            List<string> custom = new List<string>();

            string columnHEad = "<tr><th rowspan='2' class='datatable-col-100' style='vertical-align:middle;text-align:center;'>OPT </th>";
            columnHEad += "<th colspan='12' class='datatable-col-300' style='vertical-align:middle;text-align:center;'>FORECASTING 1st BILLING - " + year + "</th>";
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
 
        [HttpPost, Route("GetSummaryWeek")]
        public IHttpActionResult GetSummaryWeek(DashboardPotentialRFITo1stBAPSBilling post)
        {
            try
            {
                var CountOfRows = "10";
                var data = summary.GetDashboardSummaryList(post.Type, post.STIP, post.year, post.month, post.desc);
                return Ok(new { draw = 10, recordsTotal = CountOfRows, recordsFiltered = CountOfRows, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex.InnerException);
            }
        }       
        [HttpPost, Route("GetDetail")]
        public IHttpActionResult GetDetails(ARSystemFrontEnd.Models.PostDashboardPotential post)
        {
            try
            {
                string strWhereClause = " 1=1 ";


                #region filter
                if (post.RFIDateYear != null)
                {
                    strWhereClause += " AND YEAR(RFIDate) = '" + post.RFIDateYear + "'";
                }
                if (post.STIPCategory != null)
                {
                    strWhereClause += " AND STIPCategory = '" + post.STIPCategory + "'";
                }

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

                #endregion


                if (post.Type == "A")
                    strWhereClause += " AND POStep IN ('PO DONE', 'PO PROCESS')";
                else if (post.Type == "B")
                    strWhereClause += " AND DashboardStep IN ('RFI', 'CME', 'SITAC' ,'BAUK SUBMIT' )";
                else if (post.Type == "C")
                    strWhereClause += " AND DashboardStep IN ('BAUK REVIEW', 'BAUK DONE','CASH IN First BAPS BILLING', 'RTI', 'BAPS PRODUCTION', 'BAPS VALIDATION')";
                    
                else if (post.Type == "A1")
                    strWhereClause += " AND POStep IN ('PO PROCESS') ";
                else if (post.Type == "A2")
                    strWhereClause += " AND POStep IN ('PO DONE') ";

                else if (post.Type == "B1")
                    strWhereClause += " AND DashboardStep IN ('SITAC') ";
                else if (post.Type == "B2")
                    strWhereClause += " AND DashboardStep IN ('CME') ";
                else if (post.Type == "B3")
                    strWhereClause += " AND DashboardStep IN ('RFI') ";
                else if (post.Type == "B4")
                    strWhereClause += " AND DashboardStep = 'BAUK SUBMIT' ";

                else if (post.Type == "C1")
                    strWhereClause += " AND DashboardStep IN ('BAUK REVIEW') ";
                else if (post.Type == "C2")
                    strWhereClause += " AND DashboardStep IN ('BAUK DONE') ";
                else if (post.Type == "C3")
                    strWhereClause += " AND DashboardStep IN ('BAPS PRODUCTION') ";
                else if (post.Type == "C4")
                    strWhereClause += " AND DashboardStep IN ('BAPS VALIDATION') ";
                else if (post.Type == "C5")
                    strWhereClause += " AND DashboardStep IN ('RTI') ";
                else if (post.Type == "C6")
                    strWhereClause += " AND DashboardStep IN ('CASH IN First BAPS BILLING') ";
              

                #region forecasting 1st billing
                if (post.RFIDateMonth != null && post.Step != "FooterTotal")
                    strWhereClause += " AND MONTH(DATEADD(DD,SetDayForecasting,RFIDate)) = '" + post.RFIDateMonth + "'";
                if (post.CustomerID != null)
                    strWhereClause += " AND ISNULL(CustomerID,'NOT MAPPING') = '" + post.CustomerID + "' ";
                if (post.RFIDateWeek != null)
                    strWhereClause += " AND  CONVERT(VARCHAR(15), DATEPART(DAY, DATEDIFF(DAY, 0, DATEADD(DD, SetDayForecasting, RFIDate)) / 7 * 7) / 7 + 1) = '" + post.RFIDateWeek.Substring(1) + "' ";
                if (post.Step != null)
                {
                    if (post.Step.Contains("BAUK"))
                    {
                        strWhereClause += " AND DashboardStep ='" + post.Step + "' ";
                    }
                    else if (post.Step.Contains("BAPS"))
                    {
                        strWhereClause += " AND DashboardStep ='" + post.Step + "' ";
                    }
                    else if (post.Step.Contains("RTI"))
                    {
                        strWhereClause += " AND DashboardStep ='" + post.Step + "' ";
                    }
                    else if (post.Step == "Total")
                    {
                        strWhereClause += " AND DashboardStep NOT IN ( '-', 'CASH IN First BAPS BILLING' ) ";
                    }
                    else if (post.Step == "FooterTotal" && post.RFIDateMonth != null)
                    {
                        strWhereClause += " AND DashboardStep NOT IN ( '-', 'CASH IN First BAPS BILLING') AND MONTH(DATEADD(DD,SetDayForecasting,RFIDate)) = '" + fnGetMonthHeaderString(post.RFIDateMonth) + "' ";
                    }
                    else if (post.Step == "FooterTotal" && post.RFIDateMonth == null)
                    {
                        strWhereClause += " AND DashboardStep NOT IN ( '-', 'CASH IN First BAPS BILLING') ";
                    }
                    else if (post.Step == "WeekTotal")
                    {
                        strWhereClause += " AND DashboardStep NOT IN ( '-', 'CASH IN First BAPS BILLING') ";
                    }
                    
                }
                    

                #endregion


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

        #region Funtion
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
        #endregion
        #region Master

        [HttpGet, Route("STIP")]
        public IHttpActionResult GetSTIPCategoryToList()
        {
            try
            {
                var result = summary.GetFilterList("FilterSTIP");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("Year")]
        public IHttpActionResult GetYearToList()
        {
            try
            {
                var result = summary.GetFilterList("FilterYear");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion
    }


    public class DashboardPotentialRFITo1stBAPSBilling : DatatableAjaxModel
    {
    
        public string Type { get; set; } 
        public string STIP { get; set; } 
        public string year { get; set; }
        public string month { get; set; }
        public string desc { get; set; }
    }
}