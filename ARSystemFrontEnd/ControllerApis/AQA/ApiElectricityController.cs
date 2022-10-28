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


namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/Electricity")]
    public class ApiElectricityController : ApiController
    {
        ElectricityService client = new ElectricityService();

        [HttpPost, Route("grid")]
        public IHttpActionResult GetBapsDataToGrid(PostTrxElectricityData post)
        {
            try
            {
                int intTotalRecord = 0;
                //ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<vwElectricityData> bapsdata = new List<vwElectricityData>();

                intTotalRecord = client.GetDataCount("", MappingParameter(post));

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                //if (userCredential.ErrorType > 0)
                //    bapsdata.Add(new vwBAPSData(userCredential.ErrorType, userCredential.ErrorMessage));
                //else
                bapsdata = client.GetDataToList("", MappingParameter(post), strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = bapsdata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostTrxElectricityData post)
        {
            try
            {

                List<int> ListId = new List<int>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                
                //if (userCredential.ErrorType > 0)
                //    ListId.Add(0);
                //else
                ListId = client.GetElectricityDataListId(userCredential.UserID, MappingParameter(post)).ToList();

                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridReject")]
        public IHttpActionResult GetBapsRejectToGrid(PostTrxElectricityData post)
        {
            try
            {
                int intTotalRecord = 0;
                //ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<vwElectricityDataReject> bapsdata = new List<vwElectricityDataReject>();

                var parameters = MappingParameter(post);

                intTotalRecord = client.GetTrxElectricityRejectCount("", parameters);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                bapsdata = client.GetTrxElectricityRejectToList("", parameters, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = bapsdata });
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

        [HttpPost, Route("RejectMail")]
        public IHttpActionResult RejectMail(PostTrxBapsDataReject post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                vwElectricityData BapsData = new vwElectricityData();
                trxBAPSDataService BapsService = new trxBAPSDataService();

                if (userCredential.ErrorType > 0)
                {
                    BapsData = new vwElectricityData(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(BapsData);
                }  
                else
                {
                    var ListData = client.GetTrxElectricityListData(userCredential.UserID, post.ListId);

                    var Rejected = BapsService.RejectBAPS(userCredential.UserID, post.ListId, post.Remarks, post.MstRejectDtlId,post.statusRejectPOConfirm);

                    if (Rejected.ErrorType > 0)
                    {
                        return Ok(Rejected);
                    }
                    else
                    {
                        BapsData = client.EmailDataReject(userCredential.UserID, ListData, post.RejectType, post.Remarks, post.Recipient, post.CC);

                        return Ok(BapsData);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SendTask")]
        public IHttpActionResult SendTask(PostTrxElectricityData post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                vwElectricityData BapsData = new vwElectricityData();

                if (userCredential.ErrorType > 0)
                {
                    BapsData = new vwElectricityData(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(BapsData);
                }
                else
                {
                    var ListData = client.GetTrxElectricityListData(userCredential.UserID, post.ListId);
                    

                    if (ListData.FirstOrDefault().ErrorType > 0)
                    {
                        return Ok(ListData.FirstOrDefault());
                    }
                    else
                    {
                        var rslt = client.SendTask(userCredential.UserID, ListData, post.isReceive);

                        return Ok(rslt);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private string MappingParameter(PostTrxElectricityData post)
        {
            string strWhereClause = "";

            if (post.isReject > 0)
            {
                strWhereClause = "mstInvoiceStatusId IN (0,18) AND "; //TAB BAPS REJECT 
            }
            else
            {
                if (post.isReceive == 1)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.NotProcessed + " AND "; //TAB BAPS RECEIVE 
                else if (post.isReceive > 1)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.StateBAPSConfirm + " AND InvoiceDate IS NOT NULL AND "; //TAB BAPS CONFIRM & DONE INV 
                else
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.StateBAPSReceive + " AND "; //TAB BAPS CONFIRM 
            }


            if (!string.IsNullOrWhiteSpace(post.strCompanyId))
            {
                strWhereClause += "CompanyId LIKE '%" + post.strCompanyId.TrimStart().TrimEnd() + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strOperator))
            {
                strWhereClause += "Operator LIKE '%" + post.strOperator.TrimStart().TrimEnd() + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteName))
            {
                strWhereClause += "SiteName LIKE '%" + post.strSiteName + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strAccountNumber))
            {
                strWhereClause += "AccountNumber LIKE '%" + post.strAccountNumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strAccountName))
            {
                strWhereClause += "AccountName LIKE '%" + post.strAccountName + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strBankName))
            {
                strWhereClause += "BankName LIKE '%" + post.strBankName + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPONumber))
            {
                strWhereClause += "PoNumber LIKE '%" + post.strPONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteIDOpr))
            {
                strWhereClause += "SiteIDOpr LIKE '%" + post.strSiteIDOpr + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSONumber))
            {
                strWhereClause += "SONumber LIKE '%" + post.strSONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strVoucherNumber))
            {
                strWhereClause += "Voucher LIKE '%" + post.strVoucherNumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteIdOld))
            {
                strWhereClause += "SiteIdOld LIKE '%" + post.strSiteIdOld + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strDescription))
            {
                strWhereClause += "ExpDescription LIKE '%" + post.strDescription + "%' AND ";
            }

            if (!string.IsNullOrWhiteSpace(post.strStatus))
            {
                strWhereClause += "PICAStatus LIKE '%" + post.strStatus + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPICA))
            {
                strWhereClause += "Description LIKE '%" + post.strPICA + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strRejectRemarks))
            {
                strWhereClause += "RejectRemarks LIKE '%" + post.strRejectRemarks + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPeriodInvoice))
            {
                strWhereClause += "BapsPeriod LIKE '%" + post.strPeriodInvoice + "%' AND ";
            }

            if (!string.IsNullOrWhiteSpace(post.strRegion))
            {
                strWhereClause += "Regional LIKE '%" + post.strRegion.TrimStart().TrimEnd() + "%' AND ";
            }

            if (!string.IsNullOrWhiteSpace(post.strStartPeriod))
            {
                strWhereClause += ("YearPeriod BETWEEN " + post.strStartPeriod + " AND " + post.strEndPeriod + " AND ");
            }

            if (!string.IsNullOrWhiteSpace(post.strYearPeriod))
            {
                strWhereClause += "YearPeriod LIKE '%" + post.strYearPeriod + "%' AND ";
            }

            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

            return strWhereClause;
        }

        #region Method From Baps
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
                    BapsData = client.ConfirmBAPS(userCredential.UserID, post.ListId);


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
        #endregion
    }
}