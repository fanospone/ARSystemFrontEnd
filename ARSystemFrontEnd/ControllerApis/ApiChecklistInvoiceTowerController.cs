using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ChecklistInvoiceTower")]
    public class ApiChecklistInvoiceTowerController : ApiController
    {
        [HttpPost, Route("grid")]
        // GET: ApiChecklistInvoiceTower
        public IHttpActionResult GetChecklistInvoiceTowerToGrid(PostChecklistInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmChecklistInvoiceTower> data = new List<ARSystemService.vmChecklistInvoiceTower>();
                using (var client = new ARSystemService.ItrxChecklistInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetChecklistInvoiceTowerCount(UserManager.User.UserToken, post.InvoiceTypeId, post.CompanyId, post.OperatorId, post.PostingDateFrom, post.PostingDateTo, post.Status, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetChecklistInvoiceTowerToList(UserManager.User.UserToken, post.InvoiceTypeId, post.CompanyId, post.OperatorId, post.PostingDateFrom, post.PostingDateTo, post.Status, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult ChecklistTrxInvoiceTower(ARSystemService.vmChecklistInvoiceTower post)
        {
            try
            {
                ARSystemService.vwChecklistInvoiceTower invoice;
                using (var client = new ARSystemService.ItrxChecklistInvoiceTowerServiceClient())
                {
                    invoice = client.ChecklistInvoiceTowerDetail(UserManager.User.UserToken, post);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("")]
        public IHttpActionResult UpdateTaxInvoiceNo(PutChecklistInvoiceBuildingView put)
        {
            try
            {
                var invoice = new object();
                if(put.mstInvoiceCategoryId == 4 || put.mstInvoiceCategoryId == 5)
                {
                    using (var client = new ARSystemService.ImstTaxInvoiceServiceClient())
                    {
                        invoice = client.UpdateTaxInvoiceNumberRemaining(UserManager.User.UserToken, put.trxInvoiceHeaderId, put.taxInvoiceNo);
                    }
                }
                else
                {
                    using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                    {
                        invoice = client.UpdateTaxInvoiceNumber(UserManager.User.UserToken, put.trxInvoiceHeaderId, put.taxInvoiceNo, put.mstInvoiceCategoryId);
                    }
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultChecklist")]
        public IHttpActionResult ValidateChecklist(PutChecklistInvoiceBuildingView put)
        {
            try
            {
                ARSystemService.vmStringResult validationResult;
                using (var client = new ARSystemService.ItrxChecklistInvoiceTowerServiceClient())
                {
                    validationResult = client.ValidateChecklistInvoiceTower(UserManager.User.UserToken, put.trxInvoiceHeaderId, put.mstInvoiceCategoryId);
                }

                return Ok(validationResult);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Collection")]
        public IHttpActionResult ChecklistInvoiceTowerCollection(ARSystemService.vmChecklistInvoiceTower post)
        {
            try
            {
                ARSystemService.vwChecklistInvoiceTower invoice;
                using (var client = new ARSystemService.ItrxChecklistInvoiceTowerServiceClient())
                {
                    invoice = client.ChecklistInvoiceTowerARCollection(UserManager.User.UserToken, post);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CNInvoice")]
        public IHttpActionResult CNInvoice(PostChecklistInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxChecklistInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.CNChecklistARDataTower(UserManager.User.UserToken,post.dataChecklist, post.strRemarksCancel, post.mstPICATypeID, post.mstPICADetailID);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApproveCNInvoice")]
        public IHttpActionResult ApproveCNInvoice(PostChecklistInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxChecklistInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.ApproveCNChecklistDataTower(UserManager.User.UserToken, post.dataChecklist);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("RejectCNInvoice")]
        public IHttpActionResult RejectCNInvoice(PostChecklistInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                int[] HeaderID = { post.dataChecklist.trxInvoiceHeaderID };
                int[] CategoryId = { post.dataChecklist.mstInvoiceCategoryId.Value };
                vm.HeaderId = HeaderID;
                vm.CategoryId = CategoryId;
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.RejectCNARDataInvoiceTower(UserManager.User.UserToken, vm, "checklistardata");
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}