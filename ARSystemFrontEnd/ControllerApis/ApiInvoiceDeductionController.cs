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
using System.IO;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/MstInvoiceDeduction")]
    public class ApiInvoiceDeductionController : ApiController
    {
        [HttpPost, Route("")]
        public IHttpActionResult CreateInvoiceDeduction(ARSystemService.mstInvoiceDeduction invoiceDeduction)
        {
            try
            {
                using (var client = new ARSystemService.ImstInvoiceDeductionServiceClient())
                {
                    invoiceDeduction = client.CreateInvoiceDeduction(UserManager.User.UserToken, invoiceDeduction);
                }

                return Ok(invoiceDeduction);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridSKB")]
        public IHttpActionResult GetInvoiceDeductionToGrid(PostInvoiceDeductionView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwInvoiceDeduction> productTypes = new List<ARSystemService.vwInvoiceDeduction>();
                using (var client = new ARSystemService.ImstInvoiceDeductionServiceClient())
                {
                    intTotalRecord = client.GetInvoiceDeductionCount(UserManager.User.UserToken, post.mstDeductionTypeId, post.companyId, post.operatorId);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    productTypes = client.GetMstInvoiceDeductionToList(UserManager.User.UserToken, post.mstDeductionTypeId, post.companyId, post.operatorId, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = productTypes });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        
        [HttpPost, Route("SaveSKB")]
        public IHttpActionResult SaveSKB()
        {
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["File"];
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                ARSystemService.mstInvoiceDeduction invoiceDeduction = new ARSystemService.mstInvoiceDeduction();
                using (var client = new ARSystemService.ImstInvoiceDeductionServiceClient())
                {
                    invoiceDeduction = MapModel(invoiceDeduction, nvc, postedFile);
                    invoiceDeduction = client.CreateInvoiceDeduction(UserManager.User.UserToken, invoiceDeduction);
                }

                return Ok(invoiceDeduction);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SaveWAPU")]
        public IHttpActionResult SaveWAPU(ARSystemService.mstInvoiceDeduction post)
        {
            try
            {
                ARSystemService.mstInvoiceDeduction invoiceDeduction = new ARSystemService.mstInvoiceDeduction();
                using (var client = new ARSystemService.ImstInvoiceDeductionServiceClient())
                {
                    invoiceDeduction = client.CreateInvoiceDeduction(UserManager.User.UserToken, post);
                }

                return Ok(invoiceDeduction);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("UpdateSKB")]
        public IHttpActionResult UpdateSKB()
        {
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["File"];
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                ARSystemService.mstInvoiceDeduction invoiceDeduction = new ARSystemService.mstInvoiceDeduction();
                using (var client = new ARSystemService.ImstInvoiceDeductionServiceClient())
                {
                    invoiceDeduction = MapModel(invoiceDeduction, nvc, postedFile);
                    invoiceDeduction = client.UpdateInvoiceDeduction(UserManager.User.UserToken, invoiceDeduction.mstInvoiceDeductionId, invoiceDeduction);
                }

                return Ok(invoiceDeduction);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("UpdateWAPU/{id}")]
        public IHttpActionResult UpdateWAPU(int id, ARSystemService.mstInvoiceDeduction post)
        {
            try
            {
                ARSystemService.mstInvoiceDeduction invoiceDeduction = new ARSystemService.mstInvoiceDeduction();
                using (var client = new ARSystemService.ImstInvoiceDeductionServiceClient())
                {
                    invoiceDeduction = client.UpdateInvoiceDeduction(UserManager.User.UserToken, id, post);
                }

                return Ok(invoiceDeduction);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #region Private Methods
        private ARSystemService.mstInvoiceDeduction MapModel(ARSystemService.mstInvoiceDeduction invoice, NameValueCollection nvc, HttpPostedFile postedFile)
        {
            string nullString = "null";

            if (nvc.Get("InvoiceDeductionId") != nullString)
                invoice.mstInvoiceDeductionId = int.Parse(nvc.Get("InvoiceDeductionId").ToString());

            if (nvc.Get("DeductionTypeId") != nullString)
                invoice.mstDeductionTypeId = int.Parse(nvc.Get("DeductionTypeId").ToString());

            if (nvc.Get("StartPeriod") != nullString)
                invoice.StartPeriod = DateTime.Parse(nvc.Get("StartPeriod").ToString());

            if (nvc.Get("EndPeriod") != nullString)
                invoice.EndPeriod = DateTime.Parse(nvc.Get("EndPeriod").ToString());

            if (nvc.Get("CompanyId") != nullString)
                invoice.CompanyId = nvc.Get("CompanyId").ToString();

            if (nvc.Get("IsActive") != nullString)
                invoice.IsActive = bool.Parse(nvc.Get("IsActive").ToString());

            if (postedFile != null)
            {
                invoice.UploadBA = postedFile.FileName;
                invoice.FilePath = "\\Deduction\\" + Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                invoice.ContentType = postedFile.ContentType;

                string uploadDir = HttpContext.Current.Server.MapPath(Helper.Helper.GetDocPath() + "\\Deduction\\");
                DirectoryInfo di = new DirectoryInfo(uploadDir);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(uploadDir + Helper.Helper.GetFileTimeStamp(postedFile.FileName));
            }

            return invoice;
        }
        #endregion
    }
}