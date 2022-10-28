using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ManageMergedInvoiceDetailBuilding")]
    public class ApiMergedInvoiceDetailBuildingController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetMergedInvoiceDetailBuildingToList(string customerName, string companyId, string invNo)
        {
            try
            {
                List<ARSystemService.vwMergedInvoiceDetailBuilding> data = new List<ARSystemService.vwMergedInvoiceDetailBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    data = client.GetMergedInvoiceBuildingDetailToList(UserManager.User.UserToken, companyId, customerName, invNo, "BatchID, trxInvoiceHeaderID DESC", 0, 0).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetMergedInvoiceDetailBuildingDataToGrid(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwMergedInvoiceDetailBuilding> data = new List<ARSystemService.vwMergedInvoiceDetailBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    intTotalRecord = client.GetMergedInvoiceBuildingDetailCount(UserManager.User.UserToken, post.companyId, post.customerName, post.InvNo);

                    string strOrderBy = "BatchID, trxInvoiceHeaderID DESC";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetMergedInvoiceBuildingDetailToList(UserManager.User.UserToken, post.companyId, post.customerName, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}