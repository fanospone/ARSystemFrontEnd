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
    [RoutePrefix("api/PostingInvoiceOtherProduct")]
    public class ApiPostingInvoiceOtherProductController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetInvoiceOtherProductDetailToList(string soNumber, string invoiceNumber)
        {
            try
            {
                List<ARSystemService.vwInvoiceOtherProductDetail> data = new List<ARSystemService.vwInvoiceOtherProductDetail>();
                using (var client = new ARSystemService.ItrxInvoiceOtherProductDetailServiceClient())
                {
                    data = client.GetTrxInvoiceOtherProductDetailToList(UserManager.User.UserToken, invoiceNumber, "trxInvoiceOtherProductDetailID", 0, 0).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetInvoiceOtherProductDetailDataToGrid(PostTrxInvoiceOtherProductView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwInvoiceOtherProductDetail> data = new List<ARSystemService.vwInvoiceOtherProductDetail>();
                using (var client = new ARSystemService.ItrxInvoiceOtherProductDetailServiceClient())
                {
                    intTotalRecord = client.GetInvoiceOtherProductDetailCount(UserManager.User.UserToken, post.invoiceNumber);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetTrxInvoiceOtherProductDetailToList(UserManager.User.UserToken, post.invoiceNumber, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        
        [HttpPost, Route("")]
        public IHttpActionResult PostingTrxInvoiceOtherProduct(PostTrxInvoiceOtherProductPosting post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceOtherProductDetailServiceClient())
                {
                    invoice = client.PostingInvoiceOtherProductDetail(UserManager.User.UserToken, post.trxInvoiceHeaderID, post.invoiceDate, post.subject, post.signature);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Cancel")]
        public IHttpActionResult CancelTrxInvoiceOtherProduct(PostTrxInvoiceOtherProductPosting post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceOtherProductDetailServiceClient())
                {
                    invoice = client.CancelInvoiceOtherProductDetail(UserManager.User.UserToken, post.trxInvoiceHeaderID, post.invoiceDate, post.subject, post.signature);
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