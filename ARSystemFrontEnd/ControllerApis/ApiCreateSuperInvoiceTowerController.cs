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
    [RoutePrefix("api/CreateSuperInvoiceTower")]
    public class ApiCreateSuperInvoiceTowerController : ApiController
    {

        [HttpPost, Route("grid")]
        public IHttpActionResult GetInvoiceTowerDataToGrid(PostTrxCreateInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwDataBAPSConfirm> bapsdata = new List<ARSystemService.vwDataBAPSConfirm>();
                using (var client = new ARSystemService.ItrxCreateBigInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetTrxCreateBigInvoiceTowerCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.strSoNumber, post.intmstInvoiceStatusId, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    bapsdata = client.GetTrxCreateBigInvoiceTowerToList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.strSoNumber, post.intmstInvoiceStatusId, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = bapsdata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetSumADPP")]
        public IHttpActionResult GetSumADPP(PostTrxCreateBigInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader data = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxCreateBigInvoiceTowerServiceClient())
                {
                    data = client.GetSumADPP(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType,
                        post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.strSONumber, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId, post.excludedIDs.ToArray());
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Validate")]
        public IHttpActionResult Validate(PostTrxCreateBigInvoiceTowerView post)
        {
            try
            {
                ARSystemService.vmStringResult data = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxCreateBigInvoiceTowerServiceClient())
                {
                    data = client.ValidateSuperCreateInvoice(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType,
                        post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.strSONumber, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId, post.excludedIDs.ToArray());
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Create")]
        public IHttpActionResult Create(PostTrxCreateBigInvoiceTowerProcess post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader data = new ARSystemService.trxInvoiceHeader();
                using (var client = new ARSystemService.ItrxCreateBigInvoiceTowerServiceClient())
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
                    //vm.IsGR = post.IsGR;

                    data = client.CreateBigInvoiceTower(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType,
                        post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.strSONumber, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId, post.excludedIDs.ToArray(), vm);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Cancel")]
        public IHttpActionResult Cancel(PostTrxCreateBigInvoiceTowerProcess post)
        {
            try
            {
                ARSystemService.trxArDetail data = new ARSystemService.trxArDetail();
                using (var client = new ARSystemService.ItrxCreateBigInvoiceTowerServiceClient())
                {
                    data = client.CancelBigInvoiceTower(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType,
                        post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.strSONumber, post.strPONumber, post.strBAPSNumber, post.intmstInvoiceStatusId, post.excludedIDs.ToArray(), post.CancelRemarks, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ApproveCancel")]
        public IHttpActionResult ApproveCancel(PostTrxCreateBigInvoiceTowerProcess post)
        {
            try
            {
                ARSystemService.trxArDetail data = new ARSystemService.trxArDetail();
                using (var client = new ARSystemService.ItrxCreateBigInvoiceTowerServiceClient())
                {
                    data = client.ApproveCancelBigInvoiceTower(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType,
                        post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.strSONumber, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId, post.excludedIDs.ToArray());
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxCreateBigInvoiceTowerView post)
        {
            try
            {
                List<int> data = new List<int>();
                using (var client = new ARSystemService.ItrxCreateBigInvoiceTowerServiceClient())
                {
                    data = client.GetBigInvoiceTowerListId(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strBapsType,
                        post.strPeriodInvoice, post.strInvoiceType, post.strRegional, post.strSONumber, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.intmstInvoiceStatusId,
                        post.excludedIDs.ToArray()).ToList();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}