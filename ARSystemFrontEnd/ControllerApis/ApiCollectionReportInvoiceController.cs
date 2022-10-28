using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/CollectionReportInvoiceTower")]
    public class ApiCollectionReportInvoiceController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetCollectionReportInvoiceDataToGrid(PostTrxCollectionReportInvoiceView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwCollectionReportInvoice> ReportInvoiceData = new List<ARSystemService.vwCollectionReportInvoice>();
                using (var client = new ARSystemService.ItrxCollectionReportInvoiceServiceClient())
                {
                    intTotalRecord = client.GetTrxCollectionReportInvoiceCount(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strStartPaidDate, post.strEndPaidDate, post.strCompanyId, post.strOperator, post.strPaidStatus, post.intInvoiceCategory, post.intCustomerId, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    ReportInvoiceData = client.GetTrxCollectionReportInvoiceToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strStartPaidDate, post.strEndPaidDate, post.strCompanyId, post.strOperator, post.strPaidStatus,post.intInvoiceCategory, post.intCustomerId, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = ReportInvoiceData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}