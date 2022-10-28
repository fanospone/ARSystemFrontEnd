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
    [RoutePrefix("api/ManageMergedInvoiceOnlyBuilding")]
    public class ApiMergedInvoiceOnlyBuildingController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetMergedInvoiceOnlyBuildingToList(string customerName, string companyId, string invNo)
        {
            try
            {
                List<ARSystemService.vwMergedInvoiceOnlyBuilding> data = new List<ARSystemService.vwMergedInvoiceOnlyBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    data = client.GetMergedInvoiceBuildingOnlyToList(UserManager.User.UserToken, companyId, customerName, invNo, "trxInvoiceHeaderID", 0, 0).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetMergedInvoiceOnlyBuildingDataToGrid(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwMergedInvoiceOnlyBuilding> data = new List<ARSystemService.vwMergedInvoiceOnlyBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    intTotalRecord = client.GetMergedInvoiceBuildingOnlyCount(UserManager.User.UserToken, post.companyId, post.customerName, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetMergedInvoiceBuildingOnlyToList(UserManager.User.UserToken, post.companyId, post.customerName, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CancelMerge")]
        public IHttpActionResult CancelMergeInvoiceBuilding(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    invoice = client.CancelMergeInvoiceBuilding(UserManager.User.UserToken, post.listHeaderId.ToArray());
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        
        [HttpPost, Route("GetValidateResultPrint")]
        public IHttpActionResult GetValidateResultPrint(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    Result = client.ValidatePrintMergedInvoiceOnlyBuilding(UserManager.User.UserToken, post.listHeaderId.ToArray());
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultCancelMerge")]
        public IHttpActionResult GetValidateResultCancelMerge(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    Result = client.ValidateCancelMergeInvoiceBuilding(UserManager.User.UserToken, post.listHeaderId.ToArray());
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("PrintInvoice")]
        public IHttpActionResult PrintInvoice(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader header;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    header = client.PrintMergedInvoiceBuilding(UserManager.User.UserToken, post.listHeaderId.ToArray(), post.PICAReprintID, post.ReprintRemark);
                }

                return Ok(header);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                List<int> Result = new List<int>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    Result = client.GetMergedInvoiceBuildingOnlyListId(UserManager.User.UserToken, post.companyId, post.customerName, post.InvNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetReprintListId")]
        public IHttpActionResult GetReprintListId(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                List<int> Result = new List<int>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    Result = client.GetMergedReprintInvoiceBuildingOnlyListId(UserManager.User.UserToken, post.companyId, post.customerName, post.InvNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("gridReprint")]
        public IHttpActionResult GetApprovalReprintMergedTowerToGrid(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwDeptHeadReprintAppInvMergedBuilding> PostedData = new List<ARSystemService.vwDeptHeadReprintAppInvMergedBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    intTotalRecord = client.GetApprovalReprintMergedBuildingCount(UserManager.User.UserToken, post.companyId, post.customerName, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    PostedData = client.GetApprovalReprintMergedBuildingtoList(UserManager.User.UserToken, post.companyId, post.customerName, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = PostedData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApprovalInvoice")]
        public IHttpActionResult ApprovalInvoice(PostTrxManageMergedInvoiceOnlyBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();

                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    InvoiceHeader = client.ApprovalRequestReprintMergedBuilding(UserManager.User.UserToken, post.listHeaderId.ToArray(), post.ApprovalStatus);
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