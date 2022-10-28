using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/PrintInvoiceTower")]
    public class ApiPrintInvoiceTowerController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult GetPrintInvoiceTowerDataToList(string strCompanyId, string Operator, string strInvoiceType, string strStartPeriod, string strEndPeriod, int intmstInvoiceStatusId, string invNo, int invoiceManual)
        {
            try
            {
                List<ARSystemService.vwDataPostedInvoiceTower> PostedData = new List<ARSystemService.vwDataPostedInvoiceTower>();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    PostedData = client.GetTrxPrintnvoiceTowerToList(UserManager.User.UserToken, strCompanyId, Operator, strInvoiceType, strStartPeriod,strEndPeriod, intmstInvoiceStatusId, invNo, invoiceManual, "trxInvoiceHeaderID", 0, 0).ToList();
                }

                return Ok(PostedData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetPrintInvoiceTowerDataToGrid(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwDataPostedInvoiceTower> PostedData = new List<ARSystemService.vwDataPostedInvoiceTower>();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetTrxPrintInvoiceTowerCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strInvoiceType, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId, post.InvNo, post.invoiceManual);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    PostedData = client.GetTrxPrintnvoiceTowerToList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strInvoiceType, post.strStartPeriod,post.strEndPeriod,post.intmstInvoiceStatusId, post.InvNo, post.invoiceManual, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = PostedData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultMerge")]
        public IHttpActionResult GetValidateResultMerge(ARSystemService.vmGetInvoicePostedList vm)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    Result = client.ValidateMerge(UserManager.User.UserToken, vm);
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("InvoicelistGrid")]
        public IHttpActionResult GetSiteListData(ARSystemService.vmGetInvoicePostedList vm)
        {
            try
            {
                List<ARSystemService.vwDataPostedInvoiceTower> PostedData = new List<ARSystemService.vwDataPostedInvoiceTower>();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    PostedData = client.GetInvoiceListDataToList(UserManager.User.UserToken, vm).ToList();
                }

                return Ok(PostedData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        
        [HttpPost, Route("PostingInvoiceMerge")]
        public IHttpActionResult PostingInvoice(PostTrxCreateInvoiceTowerPosting post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    ARSystemService.vmCreateInvoice vm = new ARSystemService.vmCreateInvoice();
                    vm.ListInvoicePosted = post.ListInvoicePosted.ToArray();
                    vm.SumADPP = post.SumADPP;
                    vm.SumAPenalty = post.SumAPenalty;
                    vm.SumAPPN = post.SumAPPN;
                    vm.SumADiscount = post.SumADiscount;
                    InvoiceHeader = client.PostingInvoiceTowerMerge (UserManager.User.UserToken, post.strInvoiceDate, post.strSubject, post.strOperatorRegionId, post.strSignature, vm,post.strAdditionalNote);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultPrint")]
        public IHttpActionResult GetValidateResultPrint(ARSystemService.vmGetInvoicePostedList vm)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    Result = client.ValidatePrint(UserManager.User.UserToken, vm);
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("PrintInvoice")]
        public IHttpActionResult PrintInvoice(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.HeaderId.ToArray();
                vm.CategoryId = post.CategoryId.ToArray();

                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.PrintInvoiceTower(UserManager.User.UserToken, vm, post.PICAReprintID, post.ReprintRemarks, post.IsCover);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CNInvoiceTower")]
        public IHttpActionResult CNInvoiceTower(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.HeaderId.ToArray();
                vm.CategoryId = post.CategoryId.ToArray();

                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.CNPrintInvoiceTower(UserManager.User.UserToken, vm, post.RemarksPrint,post.mstPICATypeID,post.mstPICADetailID);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApproveCNInvoiceTower")]
        public IHttpActionResult ApproveCNInvoiceTower(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.HeaderId.ToArray();
                vm.CategoryId = post.CategoryId.ToArray();

                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.ApproveCNPrintInvoiceTower(UserManager.User.UserToken, vm);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResultCN")]
        public IHttpActionResult GetValidateResultCN(ARSystemService.vmGetInvoicePostedList vm)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    Result = client.ValidateCNInvoice(UserManager.User.UserToken, vm, true);
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
     
        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                List<ARSystemService.vmCheckAllInvoice> Result = new List<ARSystemService.vmCheckAllInvoice>();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    Result = client.GetPrintInvoiceTowerListId(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strInvoiceType, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId, post.InvNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("RejectCNInvoiceTower")]
        public IHttpActionResult RejectCNInvoiceTower(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.HeaderId.ToArray();
                vm.CategoryId = post.CategoryId.ToArray();

                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.RejectCNARDataInvoiceTower(UserManager.User.UserToken, vm, "print");
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridReprint")]
        public IHttpActionResult GetApprovalReprintTowerToGrid(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwDeptHeadReprintApprovalInvoiceTower> PostedData = new List<ARSystemService.vwDeptHeadReprintApprovalInvoiceTower>();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetApprovalReprintTowerCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strInvoiceType, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId, post.InvNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    PostedData = client.GetApprovalReprintTowertoList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strInvoiceType, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId, post.InvNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = PostedData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApprovalInvoice")]
        public IHttpActionResult ApprovalInvoice(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.HeaderId.ToArray();
                vm.CategoryId = post.CategoryId.ToArray();

                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.ApprovalRequestReprint(UserManager.User.UserToken, vm, post.ApprovalStatus);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetReprintListId")]
        public IHttpActionResult GetReprintListId(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                List<ARSystemService.vmCheckAllInvoice> Result = new List<ARSystemService.vmCheckAllInvoice>();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    Result = client.GetRePrintInvoiceTowerListId(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strInvoiceType, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId, post.InvNo).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetSubject")]
        public IHttpActionResult GetSubjectFromMatrixMergedInvoice(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                ARSystemService.vmStringResult Subject = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    Subject = client.GetSubjectFromMatrixMergedInvoice(UserManager.User.UserToken, post.HeaderId.ToArray());
                }

                return Ok(Subject);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

    }
}