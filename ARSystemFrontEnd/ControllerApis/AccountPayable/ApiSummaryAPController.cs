using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystem.Service;
using ARSystemFrontEnd.Models;

namespace ARSystemFrontEnd.ControllerApis.AccountPayable
{
    [RoutePrefix("api/SummaryAP")]
    public class ApiSummaryAPController : ApiController
    {
        private readonly SummaryDataAPService summary;
            
        public ApiSummaryAPController()
        {
            summary = new SummaryDataAPService();
        }

        [HttpPost, Route("GetGroup")]
        public IHttpActionResult GetGroup()
        {
            var table = summary.GetGroupSummaryDataAP();
            string[] columnList = new string[table.Columns.Count];
            int idx = 0;
            List<DatatableColumnCustom> custom = new List<DatatableColumnCustom>();

            string columnHEad = "<tr><th class='datatable-col-250' style='vertical-align:middle;text-align:center;'>Product Site </th>";
            columnHEad += "<th class='datatable-col-250' style='vertical-align:middle;text-align:center;'>Qty (Sonumb)</th>";
            columnHEad += "<th class='datatable-col-250' style='vertical-align:middle;text-align:center;'>Total Cost (Mio)</th>";
            columnHEad += "<th class='datatable-col-250' style='vertical-align:middle;text-align:center;'>Total Revenue (Mio)</th></tr><tr>";

            foreach (DataColumn column in table.Columns)
            {
                columnList[idx] = column.ColumnName;

                if (idx > 1 && !column.ColumnName.Contains("Total"))
                {
                    columnHEad += "<td class='datatable-col-50 text-center'>" + column.ColumnName + "</td>";
                    custom.Add(new DatatableColumnCustom
                    {
                        data = column.ColumnName,
                        className = "text-right",
                        render = "$.fn.dataTable.render.number(',', '.', 2, '')"
                    });
                }
                else
                {
                    custom.Add(new DatatableColumnCustom
                    {
                        data = column.ColumnName,
                        className = "text-center",
                    });
                }

                idx += 1;
            }

            columnHEad += "</tr>";

            var json = JsonConvert.SerializeObject(table);

            return Ok(new { dataSummary = json, tblHead = columnHEad, customcolumn = custom });
        }

        [HttpGet, Route("GetData")]
        public IHttpActionResult GetData(string ProductSite)
        {
            if (!string.IsNullOrEmpty(ProductSite))
            {
                ProductSite = " ProductSite =  '" + ProductSite + "' ";
            }
            else
            {
                ProductSite = " ISNULL(ProductSite,'') =  '' ";
            }

            var table = summary.GetSummaryDataAP(ProductSite);
            string[] columnList = new string[table.Columns.Count];
            int idx = 0;
            List<DatatableColumnCustom> custom = new List<DatatableColumnCustom>();

            string columnHEad = "<tr><th rowspan='2'  class='datatable-col-75' style='vertical-align:middle;text-align:center;'>Product </th>";
            columnHEad += "<th rowspan='2' style='vertical-align:middle;text-align:center;width:35px;'>Qty (Sonumb)</th>";
            columnHEad += "<th colspan='3'  class='text-center datatable-col-250'>Cost Expense (Mio)</th>";
            columnHEad += "<th rowspan='2'  class='datatable-col-75' style='vertical-align:middle;text-align:center;'>Total Cost (Mio)</th>";
            columnHEad += "<th colspan='" + (table.Columns.Count - 7) + "'  class='text-center' style='width:350px;'>Revenue (Mio)</th>";
            columnHEad += "<th rowspan='2'  class='datatable-col-75' style='vertical-align:middle;text-align:center;'>Total Revenue (Mio)</th></tr><tr>";

            foreach (DataColumn column in table.Columns)
            {
                columnList[idx] = column.ColumnName;
                
                if(idx > 1 && !column.ColumnName.Contains("Total"))
                {
                    columnHEad += "<td class='datatable-col-50 text-center'>" + column.ColumnName + "</td>";
                    custom.Add(new DatatableColumnCustom
                    {
                        data = column.ColumnName,
                        className = "text-right",
                        render = "$.fn.dataTable.render.number(',', '.', 2, '')"
                    });
                }
                else
                {
                    custom.Add(new DatatableColumnCustom
                    {
                        data = column.ColumnName,
                        className = "text-center",
                    });
                }

                idx += 1;
            }

            columnHEad += "</tr>";

            var json = JsonConvert.SerializeObject(table);

            return Ok(new { dataSummary = json, tblHead = columnHEad, customcolumn = custom });
        }

        [HttpPost, Route("GetTenantData")]
        public IHttpActionResult GetTenantData(PostAccountPayableSummary post)
        {

            var IntTotalRows = Int32.Parse(post.strQty); //summary.GetCountSummaryDataAPTenant(post.strProduct);

            string strOrderBy = "";

            var dataresult = summary.GetPageSummaryDataAPTenant(post.strProduct,post.start,post.length).ToList();

            return Ok(new { draw = post.draw, recordsTotal = IntTotalRows, recordsFiltered = IntTotalRows, data = dataresult });
        }

        [HttpGet, Route("GetTenantCostData")]
        public IHttpActionResult GetTenantCostData(string SONumber)
        {
            var dataresult = summary.GetListDataAPTenantCost(SONumber);

            return Ok(dataresult);
        }

        [HttpGet, Route("GetTenantRevenueData")]
        public IHttpActionResult GetTenantRevenueData(string SONumber)
        {
            var dataresult = summary.GetListDataAPTenantRevenue(SONumber);

            return Ok(dataresult);
        }

        public class DatatableColumnCustom
        {
            public string data { get; set; }
            public string className { get; set; }
            public string render { get; set; }
            public string mRender { get; set; }
        }
    }
}