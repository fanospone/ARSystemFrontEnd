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
    [RoutePrefix("api/PrintInvoiceBuilding")]
    public class ApiPrintInvoiceBuildingController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetPrintInvoiceBuildingDataToList(string companyName, string invoiceTypeId, DateTime? startPeriod, DateTime? endPeriod, string invNo)
        {
            try
            {
                List<ARSystemService.vwPrintInvoiceBuilding> data = new List<ARSystemService.vwPrintInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    data = client.GetPrintInvoiceBuildingToList(UserManager.User.UserToken, companyName, invoiceTypeId, startPeriod, endPeriod, -1, invNo, "trxInvoiceHeaderID", 0, 0).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetPrintInvoiceDataToGrid(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwPrintInvoiceBuilding> data = new List<ARSystemService.vwPrintInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    intTotalRecord = client.GetPrintInvoiceBuildingCount(UserManager.User.UserToken, post.CompanyName, post.InvoiceTypeId, post.StartPeriod, post.EndPeriod, post.InvoiceStatusId, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    data = client.GetPrintInvoiceBuildingToList(UserManager.User.UserToken, post.CompanyName, post.InvoiceTypeId, post.StartPeriod, post.EndPeriod, post.InvoiceStatusId, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultMerge")]
        public IHttpActionResult GetValidateResultMerge(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    Result = client.ValidateMergeInvoiceBuilding(UserManager.User.UserToken, post.HeaderId.ToArray());
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("InvoicelistGrid")]
        public IHttpActionResult GetSiteListData(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                List<ARSystemService.vwPrintInvoiceBuilding> data = new List<ARSystemService.vwPrintInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    data = client.GetInvoiceBuildingListDataToList(UserManager.User.UserToken, post.HeaderId.ToArray()).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("PostingInvoiceMerge")]
        public IHttpActionResult PostingInvoice(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    ARSystemService.vmCreateInvoice vm = new ARSystemService.vmCreateInvoice();
                    InvoiceHeader = client.MergeInvoiceBuilding(UserManager.User.UserToken, post.InvoiceDate, post.Subject, post.Signature, post.HeaderId.ToArray());
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultPrint")]
        public IHttpActionResult GetValidateResultPrint(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    Result = client.ValidatePrintInvoiceBuilding(UserManager.User.UserToken, post.HeaderId.ToArray());
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("PrintInvoice")]
        public IHttpActionResult PrintInvoice(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader header;
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    header = client.PrintInvoiceBuildingDetail(UserManager.User.UserToken, post.HeaderId.ToArray(), post.PICAReprintID, post.ReprintRemark);
                }

                return Ok(header);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CNInvoiceBuilding")]
        public IHttpActionResult CNInvoiceBuilding(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();

                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    InvoiceHeader = client.CNPrintInvoiceBuilding(UserManager.User.UserToken, post.HeaderId.ToArray(), post.RemarkCN, post.InvoiceStatusId);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApproveCNInvoiceBuilding")]
        public IHttpActionResult ApproveCNInvoiceBuilding(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();

                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    InvoiceHeader = client.ApproveCNPrintInvoiceBuilding(UserManager.User.UserToken, post.HeaderId.ToArray());
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultCN")]
        public IHttpActionResult GetValidateResultCN(PostTrxPrintInvoiceBuildingView vm)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    Result = client.ValidateCNPrintInvoiceBuilding(UserManager.User.UserToken, vm.HeaderId.ToArray());
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                List<int> Result = new List<int>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    Result = client.GetPrintInvoiceBuildingListId(UserManager.User.UserToken, post.CompanyName, post.InvoiceTypeId, post.StartPeriod, post.EndPeriod, post.InvoiceStatusId, post.InvNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("RejectCNInvoiceBuilding")]
        public IHttpActionResult RejectCNInvoiceBuilding(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();

                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    InvoiceHeader = client.RejectCNPrintInvoiceBuilding(UserManager.User.UserToken, post.HeaderId.ToArray());
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridReprint")]
        public IHttpActionResult GetApprovalReprintBuildingToGrid(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwDeptHeadReprintApprovalInvoiceBuilding> PostedData = new List<ARSystemService.vwDeptHeadReprintApprovalInvoiceBuilding>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    intTotalRecord = client.GetApprovalReprintBuildingCount(UserManager.User.UserToken, post.CompanyName, post.InvoiceTypeId, post.StartPeriod, post.EndPeriod, post.InvoiceStatusId, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    PostedData = client.GetApprovalReprintBuildingtoList(UserManager.User.UserToken, post.CompanyName, post.InvoiceTypeId, post.StartPeriod, post.EndPeriod, post.InvoiceStatusId, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = PostedData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApprovalInvoice")]
        public IHttpActionResult ApprovalInvoice(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();

                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    InvoiceHeader = client.ApprovalRequestReprintBuilding(UserManager.User.UserToken, post.HeaderId.ToArray(), post.ApprovalStatus);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetReprintListId")]
        public IHttpActionResult GetReprintListId(PostTrxPrintInvoiceBuildingView post)
        {
            try
            {
                List<int> Result = new List<int>();
                using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
                {
                    Result = client.GetRePrintInvoiceBuildingListId(UserManager.User.UserToken, post.CompanyName, post.InvoiceTypeId, post.StartPeriod, post.EndPeriod, post.InvoiceStatusId, post.InvNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}