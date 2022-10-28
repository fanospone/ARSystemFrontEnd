using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/CNInvoiceTower")]
    public class ApiCNInvoiceTowerController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetChecklistInvoiceTowerToGrid(PostTrxCNInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmCNInvoiceTower> data = new List<ARSystemService.vmCNInvoiceTower>();
                using (var client = new ARSystemService.ItrxCNInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetCNInvoiceTowerCount(UserManager.User.UserToken, post.invoiceTypeId, post.companyId, post.operatorId, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetCNInvoiceTowerToList(UserManager.User.UserToken, post.invoiceTypeId, post.companyId, post.operatorId, post.invNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CNDeptHead")]
        public IHttpActionResult CNDeptHeadInvoiceTower(PostApproveCNInvoiceTower post)
        {
            try
            {
                ARSystemService.vwCNInvoiceTower invoice;
                using (var client = new ARSystemService.ItrxCNInvoiceTowerServiceClient())
                {
                    invoice = client.CNDeptHeadInvoiceTower(UserManager.User.UserToken, post.trxInvoiceHeaderID, post.mstInvoiceCategoryId, post.userID);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CNSPV")]
        public IHttpActionResult CNSPVInvoiceTower(PostApproveCNInvoiceTower post)
        {
            try
            {
                ARSystemService.vwCNInvoiceTower invoice;
                using (var client = new ARSystemService.ItrxCNInvoiceTowerServiceClient())
                {
                    invoice = client.CNSPVInvoiceTower(UserManager.User.UserToken, post.trxInvoiceHeaderID, post.mstInvoiceCategoryId, post.userID);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}