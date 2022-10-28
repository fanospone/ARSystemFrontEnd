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
    [RoutePrefix("api/ManageMergedInvoiceOnlyTower")]
    public class ApiMergedInvoiceOnlyTowerController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetMergedInvoiceOnlyTowerToList(string customerName, string companyId, string invNo)
        {
            try
            {
                List<ARSystemService.vwMergedInvoiceOnlyTower> data = new List<ARSystemService.vwMergedInvoiceOnlyTower>();
                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    data = client.GetMergedInvoiceTowerOnlyToList(UserManager.User.UserToken, companyId, customerName, invNo, "trxInvoiceHeaderID", 0, 0).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetMergedInvoiceOnlyTowerDataToGrid(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwMergedInvoiceOnlyTower> data = new List<ARSystemService.vwMergedInvoiceOnlyTower>();
                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    intTotalRecord = client.GetMergedInvoiceTowerOnlyCount(UserManager.User.UserToken, post.companyId, post.operatorId, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetMergedInvoiceTowerOnlyToList(UserManager.User.UserToken, post.companyId, post.operatorId, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CancelMerge")]
        public IHttpActionResult CancelMergeInvoiceTower(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader invoice;
                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    invoice = client.CancelMergeInvoiceTower(UserManager.User.UserToken, post.listHeaderId.ToArray());
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultPrint")]
        public IHttpActionResult GetValidateResultPrint(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    Result = client.ValidatePrintMergedInvoiceOnlyTower(UserManager.User.UserToken, post.listHeaderId.ToArray());
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultCancelMerge")]
        public IHttpActionResult GetValidateResultCancelMerge(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    Result = client.ValidateCancelMergeInvoiceTower(UserManager.User.UserToken, post.listHeaderId.ToArray());
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("PrintInvoice")]
        public IHttpActionResult PrintInvoice(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader header;
                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    header = client.PrintMergedInvoiceTower(UserManager.User.UserToken, post.listHeaderId.ToArray(), post.PICAReprintID, post.ReprintRemark);
                }

                return Ok(header);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                List<int> Result = new List<int>();
                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    Result = client.GetMergedInvoiceTowerOnlyListId(UserManager.User.UserToken, post.companyId, post.operatorId, post.InvNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetReprintListId")]
        public IHttpActionResult GetReprintListId(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                List<int> Result = new List<int>();
                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    Result = client.GetMergedReprintInvoiceTowerOnlyListId(UserManager.User.UserToken, post.companyId, post.operatorId, post.InvNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("gridReprint")]
        public IHttpActionResult GetApprovalReprintMergedTowerToGrid(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwDeptHeadReprintAppInvMergedTower> PostedData = new List<ARSystemService.vwDeptHeadReprintAppInvMergedTower>();
                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    intTotalRecord = client.GetApprovalReprintMergedTowerCount(UserManager.User.UserToken, post.companyId, post.operatorId, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    PostedData = client.GetApprovalReprintMergedTowertoList(UserManager.User.UserToken, post.companyId, post.operatorId, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = PostedData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApprovalInvoice")]
        public IHttpActionResult ApprovalInvoice(PostTrxManageMergedInvoiceOnlyTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();

                using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
                {
                    InvoiceHeader = client.ApprovalRequestReprintMergedTower(UserManager.User.UserToken, post.listHeaderId.ToArray(), post.ApprovalStatus);
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