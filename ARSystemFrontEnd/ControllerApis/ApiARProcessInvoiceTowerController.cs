using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Collections.Specialized;
using System.Web;
using System.IO;

using ARSystem.Service.ARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ARProcessInvoiceTower")]
    public class ApiARProcessInvoiceTowerController : ApiController
    {
        private trxARProcessInvoiceTowerService _services;

        public ApiARProcessInvoiceTowerController()
        {
            _services = new trxARProcessInvoiceTowerService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetARProcessInvoiceTower(PostARProcessInvoiceTower post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmARProcessInvoiceTower> list = new List<ARSystemService.vmARProcessInvoiceTower>();
                using (var client = new ARSystemService.ItrxARProcessInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetARProcessInvoiceTowerCount(UserManager.User.UserToken, post.invoiceTypeId, post.invOperatorId, post.invCompanyId, post.invNo,post.StatusReceipt);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetARProcessInvoiceTowerToList(UserManager.User.UserToken, post.invoiceTypeId, post.invOperatorId, post.invCompanyId, post.invNo,post.StatusReceipt, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridHistory")]
        public IHttpActionResult GetARProcessInvoiceTowerHistory(PostARProcessInvoiceTower post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwARProcessInvoiceTowerHistory> list = new List<ARSystemService.vwARProcessInvoiceTowerHistory>();
                using (var client = new ARSystemService.ItrxARProcessInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetARProcessInvoiceTowerHistoryCount(UserManager.User.UserToken, post.invoiceTypeId, post.invOperatorId, post.invCompanyId, post.invNo, post.receiptDateFrom, post.receiptDateTo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetARProcessInvoiceTowerHistoryToList(UserManager.User.UserToken, post.invoiceTypeId, post.invOperatorId, post.invCompanyId, post.invNo, post.receiptDateFrom, post.receiptDateTo, strOrderBy, post.start, post.length).ToList();
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
                using (var client = new ARSystemService.ItrxARProcessInvoiceTowerServiceClient())
                {
                    invoice = MapModel(invoice, nvc, postedFile);
                    invoice = client.SaveReceiptInvoiceTower(UserManager.User.UserToken, invoice);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("PICA")]
        public IHttpActionResult PICA(PostARProcessInvoiceTowerReceipt post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    trxPICAAR data = new trxPICAAR(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    trxPICAAR pica = new trxPICAAR();
                    pica.mstPICATypeID = post.mstPICATypeID;
                    pica.mstPICAMajorID = post.mstPICAMajorID;
                    pica.mstPICADetailID = post.mstPICADetailID;
                    pica.NeedCN = post.NeedCN;
                    pica.Remark = post.Remark;
                    pica.trxInvoiceHeaderID = post.trxInvoiceHeaderID;

                    pica = _services.SavePICAARProcessInvoiceTower(userCredential.UserID, pica, post.mstInvoiceCategoryId);

                    return Ok(pica);
                }          
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                pDisposeService();
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

            if (nvc.Get("mstInvoiceCategoryId") != nullString)
                invoice.mstInvoiceCategoryId = int.Parse(nvc.Get("mstInvoiceCategoryId").ToString());

            if (nvc.Get("ARProcessPenalty") != nullString)
                invoice.ARProcessPenalty = decimal.Parse(nvc.Get("ARProcessPenalty").ToString());

            if (postedFile != null)
            {
                string dir = "\\Receipt\\Tower\\";
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