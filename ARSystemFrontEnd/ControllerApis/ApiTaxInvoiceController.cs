using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/MstTaxInvoice")]
    public class ApiTaxInvoiceController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetTaxInvoiceToList(string taxInvoice = "", string invNo = "", string taxInvoiceNo = "")
        {
            try
            {
                List<ARSystemService.vwTaxInvoice> taxInvoices = new List<ARSystemService.vwTaxInvoice>();
                using (var client = new ARSystemService.ImstTaxInvoiceServiceClient())
                {
                    taxInvoices = client.GetMstTaxInvoiceToList(UserManager.User.UserToken, invNo, taxInvoiceNo, "TaxInvoiceId", 0, 0).ToList();
                }

                return Ok(taxInvoices);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreateTaxInvoice(ARSystemService.mstTaxInvoice TaxInvoice)
        {
            try
            {
                using (var client = new ARSystemService.ImstTaxInvoiceServiceClient())
                {
                    TaxInvoice = client.CreateTaxInvoice(UserManager.User.UserToken, TaxInvoice);
                }

                return Ok(TaxInvoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateTaxInvoice(int id, ARSystemService.mstTaxInvoice TaxInvoice)
        {
            try
            {
                using (var client = new ARSystemService.ImstTaxInvoiceServiceClient())
                {
                    TaxInvoice = client.UpdateTaxInvoice(UserManager.User.UserToken, id, TaxInvoice);
                }

                return Ok(TaxInvoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetTaxInvoiceToGrid(PostTaxInvoiceView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwTaxInvoice> taxInvoices = new List<ARSystemService.vwTaxInvoice>();
                using (var client = new ARSystemService.ImstTaxInvoiceServiceClient())
                {
                    intTotalRecord = client.GetTaxInvoiceCount(UserManager.User.UserToken, post.invNo, post.taxInvoiceNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    taxInvoices = client.GetMstTaxInvoiceToList(UserManager.User.UserToken, post.invNo, post.taxInvoiceNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = taxInvoices });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("BulkCreate")]
        public IHttpActionResult BulkCreateTaxInvoice(List<ARSystemService.vmTaxInvoice> data)
        {
            try
            {
                List<ARSystemService.mstTaxInvoice> ListTax = new List<ARSystemService.mstTaxInvoice>();
                using (var client = new ARSystemService.ImstTaxInvoiceServiceClient())
                {
                    ListTax = client.CreateBulkyTaxInvoice(UserManager.User.UserToken, data.ToArray()).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}