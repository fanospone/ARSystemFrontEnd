using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Collections.Specialized;
using System.Web;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/CreateInvoiceOtherProduct")]
    public class ApiCreateInvoiceOtherProductController : ApiController
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
        public IHttpActionResult CreateTrxInvoiceOtherProduct()
        {
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["File"];
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                ARSystemService.vwInvoiceOtherProductDetail invoice = new ARSystemService.vwInvoiceOtherProductDetail();
                using (var client = new ARSystemService.ItrxInvoiceOtherProductDetailServiceClient())
                {
                    invoice = MapModel(invoice, nvc, postedFile);
                    invoice = client.CreateInvoiceOtherProductDetail(UserManager.User.UserToken, invoice);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #region Private Methods

        private ARSystemService.vwInvoiceOtherProductDetail MapModel(ARSystemService.vwInvoiceOtherProductDetail invoice, NameValueCollection nvc, HttpPostedFile postedFile)
        {
            string nullString = "null";

            if (nvc.Get("InvTotalAPPN") != nullString)
                invoice.InvTotalAPPN = Decimal.Parse(nvc.Get("InvTotalAPPN"));

            if (nvc.Get("InvTotalPenalty") != nullString)
                invoice.InvTotalPenalty = Decimal.Parse(nvc.Get("InvTotalPenalty"));

            if (nvc.Get("invSumADPP") != nullString)
                invoice.InvSumADPP = Decimal.Parse(nvc.Get("invSumADPP"));

            if (nvc.Get("InvTotalAmount") != nullString)
                invoice.InvTotalAmount = Decimal.Parse(nvc.Get("InvTotalAmount"));

            if (nvc.Get("CompanyID") != nullString)
                invoice.CompanyID = nvc.Get("CompanyID");

            if (nvc.Get("OperatorID") != nullString)
                invoice.OperatorID = nvc.Get("OperatorID");

            if (nvc.Get("mstInvoiceCategoryId") != nullString)
                invoice.mstInvoiceCategoryId = int.Parse(nvc.Get("mstInvoiceCategoryId"));

            if (nvc.Get("ChargeEntityID") != nullString)
                invoice.ChargeEntityID = int.Parse(nvc.Get("ChargeEntityID"));

            if (nvc.Get("Currency") != nullString)
                invoice.Currency = nvc.Get("Currency");

            if (nvc.Get("Discount") != nullString)
                invoice.Discount = Decimal.Parse(nvc.Get("Discount"));

            if (nvc.Get("Inflation") != nullString)
                invoice.Inflation = Decimal.Parse(nvc.Get("Inflation"));

            if (nvc.Get("InvoiceStartDate") != nullString)
                invoice.InvoiceStartDate = DateTime.Parse(nvc.Get("InvoiceStartDate"));

            if (nvc.Get("InvoiceEndDate") != nullString)
                invoice.InvoiceEndDate = DateTime.Parse(nvc.Get("InvoiceEndDate"));

            if (nvc.Get("mstInvoiceTypeID") != nullString)
                invoice.mstInvoiceTypeId = nvc.Get("mstInvoiceTypeID");
            
            if (nvc.Get("PowerAmount") != nullString)
                invoice.PowerAmount = Decimal.Parse(nvc.Get("PowerAmount"));

            if (nvc.Get("PowerTypeID") != nullString)
                invoice.PowerTypeID = nvc.Get("PowerTypeID");

            invoice.ProductID = int.Parse(nvc.Get("ProductID"));

            if (nvc.Get("OperatorID") != nullString)
                invoice.OperatorID = nvc.Get("OperatorID");

            if (postedFile != null)
            {
                invoice.ReconcileDocument = postedFile.FileName;
                invoice.FilePath = "\\Product\\" + Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                invoice.ContentType = postedFile.ContentType;

                postedFile.SaveAs(Helper.Helper.GetDocPath() + invoice.FilePath);
            }

            return invoice;
        }

        #endregion
    }
}