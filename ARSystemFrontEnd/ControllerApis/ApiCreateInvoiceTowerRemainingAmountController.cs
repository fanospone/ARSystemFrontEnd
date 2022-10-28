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
    [RoutePrefix("api/CreateInvoiceTowerRemainingAmount")]
    public class ApiCreateInvoiceTowerRemainingAmountController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetInvoiceTowerDataToGrid(PostTrxCreateInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwDataBAPSRemainingAmount> bapsdata = new List<ARSystemService.vwDataBAPSRemainingAmount>();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerRemainingAmountServiceClient())
                {
                    intTotalRecord = client.GetTrxCreateInvoiceTowerRemainingCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strRegional,post.InvoiceCategoryId,post.strSoNumber,post.intmstInvoiceStatusId,post.strPONumber,post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    bapsdata = client.GetTrxCreateInvoiceTowerRemaningAmountToList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strRegional,post.InvoiceCategoryId,post.strSoNumber,post.intmstInvoiceStatusId,post.strPONumber,post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = bapsdata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SitelistGrid")]
        public IHttpActionResult GetSiteListData(PostTrxSiteListView post)
        {
            try
            {
                List<ARSystemService.vwDataBAPSRemainingAmount> BapsData = new List<ARSystemService.vwDataBAPSRemainingAmount>();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerRemainingAmountServiceClient())
                {
                    BapsData = client.GetSiteListRemainingAmountDataToList(UserManager.User.UserToken, post.ListId.ToArray()).ToList();
                }

                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CreateTowerInvoice")]
        public IHttpActionResult CreateInvoice(PostTrxInvoiceTowerProcess post)
        {
            try
            {
                ARSystemService.trxInvoiceHeaderRemainingAmount InvoiceHeader = new ARSystemService.trxInvoiceHeaderRemainingAmount();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerRemainingAmountServiceClient())
                {
                    ARSystemService.vmCreateInvoice vm = new ARSystemService.vmCreateInvoice();
                    vm.IsPPH = post.IsPPH;
                    vm.IsPPN = post.IsPPN;
                    vm.ListTrxArDetailRemaining = post.ListTrxArDetailRemaining.ToArray();
                    vm.PercentValue = post.PercentValue;
                    vm.SumADPP = post.SumADPP;
                    vm.SumAPenalty = post.SumAPenalty;
                    vm.SumAPPH = post.SumAPPH;
                    vm.SumAPPN = post.SumAPPN;
                    vm.SumATotalInvoice = post.SumATotalInvoice;
                    vm.SumADiscount = post.SumADiscount;

                    InvoiceHeader = client.CreateInvoiceTowerRemainingAmount(UserManager.User.UserToken, vm);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("CancelTowerInvoice")]
        public IHttpActionResult CancelInvoice(PostTrxInvoiceTowerProcess post)
        {
            try
            {
                ARSystemService.trxArDetailRemainingAmount ArDetail = new ARSystemService.trxArDetailRemainingAmount();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerRemainingAmountServiceClient())
                {
                    ArDetail = client.CancelCreateRemainingAmount(UserManager.User.UserToken,post.ListTrxArDetailRemaining.ToArray(),post.ReturnRemarks);
                }

                return Ok(ArDetail);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("ApproveCancelTowerInvoice")]
        public IHttpActionResult ApproveCancelInvoice(PostTrxInvoiceTowerProcess post)
        {
            try
            {
                ARSystemService.trxArDetailRemainingAmount ArDetail = new ARSystemService.trxArDetailRemainingAmount();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerRemainingAmountServiceClient())
                {
                    ArDetail = client.ApproveCancelCreateRemainingAmount(UserManager.User.UserToken, post.ListTrxArDetailRemaining.ToArray());
                }

                return Ok(ArDetail);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetValidateResult")]
        public IHttpActionResult GetValidateResult(PostTrxSiteListView post)
        {
            try
            {
                ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerRemainingAmountServiceClient())
                {
                    Result = client.ValidateRemainingAmount(UserManager.User.UserToken, post.ListId.ToArray());
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxCreateInvoiceTowerView post)
        {
            try
            {

                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerRemainingAmountServiceClient())
                {
                    ListId = client.GetListDataConfirmRemainingId(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.InvoiceCategoryId, post.strSoNumber, post.intmstInvoiceStatusId, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}