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
    [RoutePrefix("api/CreateInvoiceTower")]
    public class ApiCreateInvoiceTowerController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetInvoiceTowerDataToGrid(PostTrxCreateInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwDataBAPSConfirm> bapsdata = new List<ARSystemService.vwDataBAPSConfirm>();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetTrxCreateInvoiceTowerCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strRegional,post.strSoNumber,post.intmstInvoiceStatusId,post.strPONumber,post.strBAPSNumber,post.strSiteIdOld,post.strStartPeriod,post.strEndPeriod, post.invoiceManual);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    bapsdata = client.GetTrxCreateInvoiceTowerToList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strRegional,post.strSoNumber,post.intmstInvoiceStatusId, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.invoiceManual, strOrderBy, post.start, post.length).ToList();
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
                List<ARSystemService.vwDataBAPSConfirm> BapsData = new List<ARSystemService.vwDataBAPSConfirm>();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerServiceClient())
                {
                    BapsData = client.GetSiteListDataToList(UserManager.User.UserToken, post.ListId.ToArray()).ToList();
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
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerServiceClient())
                {
                    ARSystemService.vmCreateInvoice vm = new ARSystemService.vmCreateInvoice();
                    vm.IsPPH = post.IsPPH;
                    vm.IsPPN = post.IsPPN;
                    vm.ListTrxArDetail = post.ListTrxArDetail.ToArray();
                    vm.PercentValue = post.PercentValue;
                    vm.SumADPP = post.SumADPP;
                    vm.SumAPenalty = post.SumAPenalty;
                    vm.SumAPPH = post.SumAPPH;
                    vm.SumAPPN = post.SumAPPN;
                    vm.SumATotalInvoice = post.SumATotalInvoice;
                    vm.PercentValue = post.PercentValue;
                    vm.SumADiscount = post.SumADiscount;
                    vm.InvoiceManual = post.ListTrxArDetail[0].InvoiceManual;
                    vm.IsGR = post.IsGR;

                    InvoiceHeader = client.CreateInvoiceTower(UserManager.User.UserToken, vm);
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
                ARSystemService.trxArDetail ArDetail = new ARSystemService.trxArDetail();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerServiceClient())
                {
                    ArDetail = client.CancelCreate(UserManager.User.UserToken,post.ListTrxArDetail.ToArray(),post.ReturnRemarks);
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
                ARSystemService.trxArDetail ArDetail = new ARSystemService.trxArDetail();
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerServiceClient())
                {
                    ArDetail = client.ApproveCancelCreate(UserManager.User.UserToken, post.ListTrxArDetail.ToArray());
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
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerServiceClient())
                {
                    Result = client.Validate(UserManager.User.UserToken, post.ListId.ToArray());
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
                using (var client = new ARSystemService.ItrxCreateInvoiceTowerServiceClient())
                {
                    ListId = client.GetListDataConfirmId(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.strSoNumber, post.intmstInvoiceStatusId, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod).ToList();
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