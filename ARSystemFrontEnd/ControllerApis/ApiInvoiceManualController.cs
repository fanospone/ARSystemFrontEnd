using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/TrxInvoiceManual")]
    public class ApiInvoiceManualController : ApiController
    {
        // GET: ApiInvoiceManual
        [HttpPost, Route("grid")]
        public IHttpActionResult GetInvoiceManualToGrid(PostTrxBapsDataView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwtrxInvoiceManualTemp> invoiceManual = new List<ARSystemService.vwtrxInvoiceManualTemp>();
                using (var client = new ARSystemService.ItrxInvoiceManualTempServiceClient())
                {
                    intTotalRecord = client.GetInvoiceManualCount(UserManager.User.UserToken, post.strSONumber, post.strStartPeriod, post.strEndPeriod);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    invoiceManual = client.GetInvoiceManualList(UserManager.User.UserToken, post.strSONumber, post.strStartPeriod, post.strEndPeriod, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = invoiceManual });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridBAPSReceive")]
        public IHttpActionResult GetBAPSReceiveList(PostTrxBapsDataView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwtrxInvoiceManualTemp> invoiceManual = new List<ARSystemService.vwtrxInvoiceManualTemp>();
                using (var client = new ARSystemService.ItrxInvoiceManualTempServiceClient())
                {
                    intTotalRecord = client.GetBAPSReceiveCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    invoiceManual = client.GetBAPSReceiveList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = invoiceManual });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridBAPSConfirm")]
        public IHttpActionResult GetBAPSConfirmList(PostTrxBapsDataView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwtrxInvoiceManual> invoiceManual = new List<ARSystemService.vwtrxInvoiceManual>();
                using (var client = new ARSystemService.ItrxInvoiceManualServiceClient())
                {
                    intTotalRecord = client.GetBAPSConfirmInvoiceManualCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    invoiceManual = client.GetBAPSConfirmInvoiceManualList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = invoiceManual });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridBAPSReject")]
        public IHttpActionResult GetBAPSRejectList(PostTrxBapsDataView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwInvoiceManualReject> invoiceManual = new List<ARSystemService.vwInvoiceManualReject>();
                using (var client = new ARSystemService.ItrxInvoiceManualServiceClient())
                {
                    intTotalRecord = client.GetBAPSRejectInvoiceManualCount(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    invoiceManual = client.GetBAPSRejectInvoiceManualList(UserManager.User.UserToken, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = invoiceManual });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ClearData")]
        public IHttpActionResult ClearData(PostTrxInvoiceManual post)
        {
            try
            {
                bool result = false;
                List<ARSystemService.trxInvoiceManualTemp> InvoiceManual = new List<ARSystemService.trxInvoiceManualTemp>();
                using (var client = new ARSystemService.ItrxInvoiceManualTempServiceClient())
                {
                    result = client.ClearData(UserManager.User.UserToken, post.ListTrxInvoiceManual.ToArray());
                }
                    
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxInvoiceManual post)
        {
            try
            {

                List<int> ListId = new List<int>();
                using (var client = new ARSystemService.ItrxInvoiceManualTempServiceClient())
                {
                    ListId = client.GetListInvoiceManualId(UserManager.User.UserToken, post.SONumber, post.dateFrom, post.dateTo).ToList();
                }

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ReceiveBAPS")]
        public IHttpActionResult ReceiveBAPS(PostTrxBapsDataReject post)
        {
            try
            {
                ARSystemService.trxInvoiceManual BapsData = new ARSystemService.trxInvoiceManual();
                using (var client = new ARSystemService.ItrxInvoiceManualTempServiceClient())
                {
                    BapsData = client.ReceiveBAPSInvoiceManual(UserManager.User.UserToken, post.ListId.ToArray());
                }

                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPut, Route("{id}/{RoundingAmount}")]
        public IHttpActionResult UpdateInvoiceAmount(int id, decimal RoundingAmount)
        {
            try
            {
                ARSystemService.trxInvoiceManual BapsData = new ARSystemService.trxInvoiceManual();
                using (var client = new ARSystemService.ItrxInvoiceManualServiceClient())
                {
                    BapsData = client.UpdateInvoiceAmountManual(UserManager.User.UserToken, id, RoundingAmount);
                }

                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("ConfirmBAPS")]
        public IHttpActionResult ConfirmBAPS(PostTrxBapsDataReject post)
        {
            try
            {
                ARSystemService.trxInvoiceManual BapsData = new ARSystemService.trxInvoiceManual();
                using (var client = new ARSystemService.ItrxInvoiceManualServiceClient())
                {
                    BapsData = client.ConfirmBAPSInvoiceManual(UserManager.User.UserToken, post.ListId.ToArray());
                }

                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("RejectBAPS")]
        public IHttpActionResult RejectBAPS(PostTrxInvoiceManual post)
        {
            try
            {
                ARSystemService.trxInvoiceManualReject BapsData = new ARSystemService.trxInvoiceManualReject();
                using (var client = new ARSystemService.ItrxInvoiceManualServiceClient())
                {
                    BapsData = client.RejectBAPSInvoiceManual(UserManager.User.UserToken, post.ListId.ToArray(), post.Remarks, post.MstRejectDtlId, post.isReceive);
                }

                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}


