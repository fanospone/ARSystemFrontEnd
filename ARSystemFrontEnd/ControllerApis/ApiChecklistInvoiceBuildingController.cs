using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ChecklistInvoiceBuilding")]
    public class ApiChecklistInvoiceBuildingController : ApiController
    {
        [HttpPost, Route("grid")]
        // GET: ApiChecklistInvoiceBuilding
        public IHttpActionResult GetChecklistInvoiceBuildingToGrid(PostChecklistInvoiceBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmChecklistInvoiceBuilding> data = new List<ARSystemService.vmChecklistInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    intTotalRecord = client.GetChecklistInvoiceBuildingCount(UserManager.User.UserToken, post.companyName, post.InvoiceTypeId, post.Status, post.PostingDateFrom, post.PostingDateTo, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetChecklistInvoiceBuildingToList(UserManager.User.UserToken, post.companyName, post.InvoiceTypeId, post.Status, post.PostingDateFrom, post.PostingDateTo, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult ChecklistTrxInvoiceBuilding(ARSystemService.vmChecklistInvoiceBuilding post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    invoice = client.ChecklistInvoiceBuildingDetail(UserManager.User.UserToken, post);
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
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    invoice = client.UpdateTaxInvoiceNumber(UserManager.User.UserToken, put.trxInvoiceHeaderId, put.taxInvoiceNo, put.mstInvoiceCategoryId);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultChecklist")]
        public IHttpActionResult ValidateChecklist(ARSystemService.vmChecklistInvoiceBuilding post)
        {
            try
            {
                ARSystemService.vmStringResult validationResult;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    validationResult = client.ValidateChecklistInvoiceBuilding(UserManager.User.UserToken, post.trxInvoiceHeaderID);
                }

                return Ok(validationResult);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Collection")]
        public IHttpActionResult ChecklistInvoiceBuildingCollection(ARSystemService.vmChecklistInvoiceBuilding post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    invoice = client.ChecklistInvoiceBuildingARCollection(UserManager.User.UserToken, post);
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CNInvoice")]
        public IHttpActionResult CNInvoice(PostChecklistInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    InvoiceHeader = client.CNChecklistARDataBuilding(UserManager.User.UserToken, post.dataChecklist, post.strRemarksCancel, post.mstPICATypeID, post.mstPICADetailID);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApproveCNInvoice")]
        public IHttpActionResult ApproveCNInvoice(PostChecklistInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    InvoiceHeader = client.ApproveCNChecklistData(UserManager.User.UserToken, post.dataChecklist);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("RejectCNInvoice")]
        public IHttpActionResult RejectCNInvoice(PostChecklistInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                int[] HeaderID = { post.dataChecklist.trxInvoiceHeaderID };
                int[] CategoryId = { 2 };//Invoice Building
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