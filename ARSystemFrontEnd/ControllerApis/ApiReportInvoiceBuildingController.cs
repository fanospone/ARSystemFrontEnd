using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Web;
using System.Collections.Specialized;
using ARSystemFrontEnd.Helper;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ReportInvoiceBuilding")]
    public class ApiReportInvoiceBuildingController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetProductToGrid(PostReportInvoiceBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwReportInvoiceBuilding> products = new List<ARSystemService.vwReportInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    intTotalRecord = client.GetReportInvoiceBuildingCount(UserManager.User.UserToken, post.invPrintDateFrom, post.invPrintDateTo, post.year, post.month, post.week, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    products = client.GetReportInvoiceBuildingToList(UserManager.User.UserToken, post.invPrintDateFrom, post.invPrintDateTo, post.year, post.month, post.week, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = products });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}