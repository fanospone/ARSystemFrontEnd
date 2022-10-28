using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/BapsBigData")]
    public class ApiBapsBigDataController : ApiController
    {
        [HttpPost, Route("SitelistGrid")]
        public IHttpActionResult SitelistGrid(PostTrxBapsBigDataView post)
        {
            try
            {
                List<ARSystemService.vwBAPSData> result = new List<ARSystemService.vwBAPSData>();
                using (var client = new ARSystemService.ItrxBapsBigDataServiceClient())
                {
                    result = client.GetBapsBigDataExcludedSiteList(UserManager.User.UserToken, post.excludedIDs.ToArray()).ToList();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Validate")]
        public IHttpActionResult Validate(PostTrxBapsBigDataView post)
        {
            try
            {
                ARSystemService.vmStringResult result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxBapsBigDataServiceClient())
                {
                    result = client.ValidateConfirmBAPSBigData(UserManager.User.UserToken, post.excludedIDs.ToArray());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Receive")]
        public IHttpActionResult Receive(PostTrxBapsBigDataView post)
        {
            try
            {
                ARSystemService.vmStringResult result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxBapsBigDataServiceClient())
                {
                    result = client.ReceiveBAPSBigData(UserManager.User.UserToken, post.excludedIDs.ToArray(), post.strCompanyId,
                        post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber,
                        post.strSONumber, post.strBapsType,post.strSiteIdOld,post.strStartPeriod,post.strEndPeriod);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Reject")]
        public IHttpActionResult Reject(PostTrxBapsBigDataReject post)
        {
            try
            {
                ARSystemService.vmStringResult result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxBapsBigDataServiceClient())
                {
                    result = client.RejectBAPSBigData(UserManager.User.UserToken, post.excludedIDs.ToArray(), post.includedIDs.ToArray(), post.strCompanyId,
                        post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber,
                        post.strSONumber, post.strBapsType,  post.Remarks, post.MstRejectDtlId.ToString(), post.strSiteIdOld,post.strStartPeriod,post.strEndPeriod, post.isReceive);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Confirm")]
        public IHttpActionResult Confirm(PostTrxBapsBigDataReject post)
        {
            try
            {
                ARSystemService.vmStringResult result = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxBapsBigDataServiceClient())
                {
                    result = client.ConfirmBAPSBigData(UserManager.User.UserToken, post.excludedIDs.ToArray(), post.strCompanyId,
                        post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber,
                        post.strSONumber, post.strBapsType,post.strSiteIdOld,post.strStartPeriod,post.strEndPeriod);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxBapsBigDataView post)
        {
            try
            {
                List<int> result = new List<int>();
                using (var client = new ARSystemService.ItrxBapsBigDataServiceClient())
                {
                    result = client.GetBigBapsDataListId(UserManager.User.UserToken, post.strCompanyId,
                        post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber,
                        post.strSONumber, post.strBapsType,post.strSiteIdOld,post.strStartPeriod,post.strEndPeriod, post.isReceive).ToList();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}