using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Helper;
using System.Globalization;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/BapsData")]
    public class ApiBapsDataController : ApiController
    {
        trxBAPSDataService client = new trxBAPSDataService();
        BapsDataService service = new BapsDataService();

        [HttpPost, Route("grid")]
        public IHttpActionResult GetBapsDataToGrid(PostTrxBapsDataView post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<vwBAPSData> bapsdata = new List<vwBAPSData>();

                intTotalRecord = client.GetTrxBapsDataCount(userCredential.UserID, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.isReceive, post.strCreatedBy, post.strStatusDismantle);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0") 
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                bapsdata = client.GetTrxBAPSDataToList(userCredential.UserID, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.isReceive, post.strCreatedBy, post.strStatusDismantle , strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = bapsdata });
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
                trxBapsData BapsData = new trxBapsData();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    BapsData = new trxBapsData(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    BapsData = client.ConfirmBAPS(userCredential.UserID, post.ListId, post.Remarks);


                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("RejectBAPS")]
        public IHttpActionResult RejectBAPS(PostTrxBapsDataReject post)
        {
            try
            {
                trxBapsData BapsData = new trxBapsData();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    BapsData = new trxBapsData(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    BapsData = client.RejectBAPS(userCredential.UserID, post.ListId, post.Remarks, post.MstRejectDtlId, post.statusRejectPOConfirm);

                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("RejectHdr")]
        public IHttpActionResult GetRejectHdrToList()
        {
            try
            {
                List<mstPICAType> HdrList = new List<mstPICAType>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    HdrList.Add(new mstPICAType(userCredential.ErrorType, userCredential.ErrorMessage));
                else
                    HdrList = client.GetRejectHdrToList(userCredential.UserID, "").ToList();


                return Ok(HdrList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("RejectDtl")]
        public IHttpActionResult GetRejectDtlToList(int HdrId)
        {
            try
            {
                List<mstPICADetail> DtlList = new List<mstPICADetail>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    DtlList.Add(new mstPICADetail(userCredential.ErrorType, userCredential.ErrorMessage));
                else
                    DtlList = client.GetRejectDtlToList(userCredential.UserID, HdrId, "").ToList();



                return Ok(DtlList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("PICAEmails")]
        public IHttpActionResult GetPICAEmails(int HdrId)
        {
            try
            {
                mstPICAType picaType = new mstPICAType();
                mstRejectService client2 = new mstRejectService();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                picaType = client2.GetEmailByPICAType(userCredential.UserID, HdrId);


                return Ok(picaType);
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
                trxBapsData BapsData = new trxBapsData();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    BapsData = new trxBapsData(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    BapsData = client.UpdateInvoiceAmount(userCredential.UserID, id, RoundingAmount);

                return Ok(BapsData);
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
                vwBAPSData BapsData = new vwBAPSData();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    BapsData = new vwBAPSData(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    BapsData = client.ReceiveBAPS(userCredential.UserID, post.ListId);

                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxBapsDataView post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                List<int> ListId = new List<int>();

                ListId = client.GetBAPSDataListId(userCredential.UserID, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.isReceive).ToList();

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridReject")]
        public IHttpActionResult GetBapsRejectToGrid(PostTrxBapsDataView post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<vwBAPSDataReject> bapsdata = new List<vwBAPSDataReject>();

                intTotalRecord = client.GetTrxBapsRejectCount(userCredential.UserID, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                bapsdata = client.GetTrxBAPSRejectToList(userCredential.UserID, post.strCompanyId, post.strOperator, post.strStatusBAPS, post.strPeriodInvoice, post.strInvoiceType, post.strCurrency, post.strPONumber, post.strBAPSNumber, post.strSONumber, post.strBapsType, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = bapsdata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpGet, Route("UpdateInflationAmount")]
        public IHttpActionResult UpdateInflationAmount(long id, decimal InflationAmount)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                var Result = 0;

                Result = client.UpdateInflationAmount(userCredential.UserID, id, InflationAmount);
                if (userCredential.ErrorType > 0)
                    Result = 0;
                else
                    Result = client.UpdateInflationAmount(userCredential.UserID, id, InflationAmount);

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }



        [HttpPost, Route("gridFreezePOConfirm")]
        public IHttpActionResult GetFreezePOConfirmToGrid(PostTrxCreateInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<vwDataBAPSConfirm> bapsdata = new List<vwDataBAPSConfirm>();

                intTotalRecord = client.GetTrxBapsConfirmNewFlowCount(userCredential.UserID, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strSoNumber, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.isFreeze);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                bapsdata = client.GetTrxBapsConfirmNewFlowToList(userCredential.UserID, post.strCompanyId, post.strOperator, post.strBapsType, post.strPeriodInvoice, post.strInvoiceType, post.strSoNumber, post.strPONumber, post.strBAPSNumber, post.strSiteIdOld, post.strStartPeriod, post.strEndPeriod, post.isFreeze, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = bapsdata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridChangePPHFinal")]
        public IHttpActionResult GetChangePPHFinalToGrid(PostVwChangePPHFinal post)
        {
            try
            {
                int intTotalRecord = 0;

                vwChangePPHFinal vwChangePPHFinal = new vwChangePPHFinal();
                vwChangePPHFinal.InvoiceNumber = post.strInvoiceNumber;
                vwChangePPHFinal.SONumber = post.strSONumber;
                vwChangePPHFinal.BAPSNumber = post.strBAPSNumber;
                vwChangePPHFinal.PONumber = post.strPONumber;
                vwChangePPHFinal.OperatorID = post.strOperatorID;
                if (post.strStartDateInvoice != null)
                {
                    vwChangePPHFinal.StartDateInvoice = DateTime.ParseExact(post.strStartDateInvoice, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }
                if (post.strEndDateInvoice != null)
                {
                    vwChangePPHFinal.EndDateInvoice = DateTime.ParseExact(post.strEndDateInvoice, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }

                List<vwChangePPHFinal> vwChangePPHFinalList = new List<vwChangePPHFinal>();

                if (UserManager.User.CompanyCode == Constants.CompanyCode.PKP)
                {
                    vwChangePPHFinal.CompanyID = Constants.CompanyCode.PKP;
                }

                intTotalRecord = service.GetVwChangePPHFinalCount("", vwChangePPHFinal);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                vwChangePPHFinalList = service.GetVwChangePPHFinalToList("", vwChangePPHFinal, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = vwChangePPHFinalList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("doChangePPHFinal/{intPPHType}")]
        public IHttpActionResult DoChangePPHFinal(List<vwChangePPHFinal> post, int intPPHType)
        {
            try
            {
                var result = 0;

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    result = 0;
                else
                    result = service.UpdatePPHFinal(userCredential.UserID, post, intPPHType);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ValidateModalReject")]
        public IHttpActionResult ValidateModalReject(PostTrxBapsDataReject post)
        {
            try
            {
                trxBapsData BapsData = new trxBapsData();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);


                if (userCredential.ErrorType > 0)
                    BapsData = new trxBapsData(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    BapsData = client.ValidateModalReject(userCredential.UserID, post.ListId, post.Remarks);

                return Ok(BapsData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}