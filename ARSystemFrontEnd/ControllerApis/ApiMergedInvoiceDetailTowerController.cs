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
    [RoutePrefix("api/ManageMergedInvoiceDetailTower")]
    public class ApiMergedInvoiceDetailTowerController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetMergedInvoiceDetailTowerToList(string operatorID, string companyId, string invNo)
        {
            try
            {
                List<ARSystemService.vwMergedInvoiceDetailTower> data = new List<ARSystemService.vwMergedInvoiceDetailTower>();
                using (var client = new ARSystemService.ItrxManageMergedInvoiceDetailTowerServiceClient())
                {
                    data = client.GetMergedInvoiceTowerDetailToList(UserManager.User.UserToken, companyId, operatorID, invNo, "BatchID, trxInvoiceHeaderID DESC", 0, 0).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetMergedInvoiceDetailTowerDataToGrid(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwMergedInvoiceDetailTower> data = new List<ARSystemService.vwMergedInvoiceDetailTower>();
                using (var client = new ARSystemService.ItrxManageMergedInvoiceDetailTowerServiceClient())
                {
                    intTotalRecord = client.GetMergedInvoiceTowerDetailCount(UserManager.User.UserToken, post.companyId, post.operatorId, post.InvNo);

                    string strOrderBy = "BatchID, trxInvoiceHeaderID DESC";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetMergedInvoiceTowerDetailToList(UserManager.User.UserToken, post.companyId, post.operatorId, post.InvNo, strOrderBy, post.start, post.length).ToList();
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