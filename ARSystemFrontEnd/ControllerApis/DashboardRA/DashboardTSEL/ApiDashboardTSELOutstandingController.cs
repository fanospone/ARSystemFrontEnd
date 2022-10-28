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
    [RoutePrefix("api/DashboardTSEL")]
    public class ApiDashboardTSELOutstandingController : ApiController
    {
        
        private readonly DashboardTSELOutstandingService summary;
       
        public ApiDashboardTSELOutstandingController()
        {
            summary = new DashboardTSELOutstandingService();            
        }

        [HttpGet, Route("GetTSELOutstandingSummary")]
        public IHttpActionResult GetTSELOutstandingSummary(string type, int? STIPDate, int? RFIDate, int? SectionID, int? SOWID, int? ProductID, int? STIPID, int? RegionalID, string CompanyID)
        {
            var table = summary.GetDashboardTSELOutstandingSummaryList(type, STIPDate, RFIDate, SectionID, SOWID, ProductID, STIPID, RegionalID, CompanyID);
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

        [HttpPost, Route("GetDetailSiteOutstanding")]
        public IHttpActionResult GetDetailSiteOverdue(PostDashboardTSEL post)
        {
            try
            {
                string strWhereClause = " 1=1 ";

                if (post.paramRow != null)
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
                if(post.paramColumn != null)
                {
                    strWhereClause += " AND YearInvoiceCategory = '" + post.paramColumn + "'";
                }
                if(post.STIPDate != null)
                    strWhereClause += " AND YEAR(StipDate) = '" + post.STIPDate.ToString() + "'";
                if (post.RFIDate != null)
                    strWhereClause += " AND YEAR(RFIDate) = '" + post.RFIDate.ToString() + "'";
                if (post.SectionID != null)
                    strWhereClause += " AND SectionProductId = " + post.SectionID ;
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
    }
}