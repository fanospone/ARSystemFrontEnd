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
using System.IO;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ARProcessInvoiceBuilding")]
    public class ApiARProcessInvoiceBuildingController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetARProcessInvoiceBuilding(PostARProcessInvoiceBuilding post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmARProcessInvoiceBuilding> list = new List<ARSystemService.vmARProcessInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxARProcessInvoiceBuildingServiceClient())
                {
                    intTotalRecord = client.GetARProcessInvoiceBuildingCount(UserManager.User.UserToken, post.invoiceTypeId, post.invCompanyId, post.customerName, post.invNo,post.StatusReceipt);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetARProcessInvoiceBuildingToList(UserManager.User.UserToken, post.invoiceTypeId, post.invCompanyId, post.customerName, post.invNo,post.StatusReceipt, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridHistory")]
        public IHttpActionResult GetARProcessInvoiceBuildingHistory(PostARProcessInvoiceBuilding post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwARProcessInvoiceBuildingHistory> list = new List<ARSystemService.vwARProcessInvoiceBuildingHistory>();
                using (var client = new ARSystemService.ItrxARProcessInvoiceBuildingServiceClient())
                {
                    intTotalRecord = client.GetARProcessInvoiceBuildingHistoryCount(UserManager.User.UserToken, post.invoiceTypeId, post.invCompanyId, post.customerName, post.invNo, post.receiptDateFrom, post.receiptDateTo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetARProcessInvoiceBuildingHistoryToList(UserManager.User.UserToken, post.invoiceTypeId, post.invCompanyId, post.customerName, post.invNo, post.receiptDateFrom, post.receiptDateTo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Receipt")]
        public IHttpActionResult Receipt()
        {
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["InvReceiptFile"];
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                ARSystemService.trxInvoiceHeader invoice = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxARProcessInvoiceBuildingServiceClient())
                {
                    invoice = MapModel(invoice, nvc, postedFile);
                    invoice = client.SaveReceiptInvoiceBuilding(UserManager.User.UserToken, invoice);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("PICA")]
        public IHttpActionResult PICA(ARSystemService.trxPICAAR post)
        {
            try
            {
                ARSystemService.trxPICAAR pica;
                using (var client = new ARSystemService.ItrxARProcessInvoiceBuildingServiceClient())
                {
                    pica = client.SavePICAARProcessInvoiceBuilding(UserManager.User.UserToken, post);
                }

                return Ok(pica);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #region Private Methods
        private ARSystemService.trxInvoiceHeader MapModel(ARSystemService.trxInvoiceHeader invoice, NameValueCollection nvc, HttpPostedFile postedFile)
        {
            string nullString = "null";

            if (nvc.Get("InvInternalPIC") != nullString)
                invoice.InvInternalPIC = nvc.Get("InvInternalPIC").ToString();

            if (nvc.Get("InvReceiptDate") != nullString)
                invoice.InvReceiptDate = DateTime.Parse(nvc.Get("InvReceiptDate").ToString());

            if (nvc.Get("ARProcessRemark") != nullString)
                invoice.ARProcessRemark = nvc.Get("ARProcessRemark").ToString();

            if (nvc.Get("trxInvoiceHeaderID") != nullString)
                invoice.trxInvoiceHeaderID = int.Parse(nvc.Get("trxInvoiceHeaderID").ToString());

            if (nvc.Get("ARProcessPenalty") != nullString)
                invoice.ARProcessPenalty = decimal.Parse(nvc.Get("ARProcessPenalty").ToString());

            if (postedFile != null)
            {
                string dir = "\\Receipt\\Building\\";
                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                string filePath = dir + fileTimeStamp;
                invoice.InvReceiptFile = postedFile.FileName;
                invoice.FilePath = filePath;
                invoice.ContentType = postedFile.ContentType;

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + dir);
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(path + fileTimeStamp);
            }

            return invoice;
        }
        #endregion
    }
}