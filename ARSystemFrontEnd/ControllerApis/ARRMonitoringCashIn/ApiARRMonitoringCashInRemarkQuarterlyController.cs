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
    [RoutePrefix("api/CashInRemarkQuarterly")]
    public class ApiARRMonitoringCashInRemarkQuarterlyController : ApiController
    {
        TrxARRMonitoringCashInRemarkQuarterlyService _service = new TrxARRMonitoringCashInRemarkQuarterlyService();


        #region List - Requeastor
        [HttpPost, Route("grid")]
        public IHttpActionResult GetBapsDataToGrid(PostTrxARRMonitoringCashInRemarkDetailQuarterly post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<TrxARRMonitoringCashInRemarkDetailQuarterly> data = new List<TrxARRMonitoringCashInRemarkDetailQuarterly>();

                intTotalRecord = _service.GetDataCount(userCredential.UserID, post.Periode, post.Quarter);

                //string UserID, string strWhereClause = "", int Periode = 0, int Quarterly = 0

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                data = _service.GetDataToList(userCredential.UserID, post.Periode, post.Quarter,
                    strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("filter")]
        public IHttpActionResult GetSTIPCategoryToList(string Type, int Year, int Quarterly)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var result = _service.GetFilterList(Type, Year, Quarterly, userCredential.UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        #region Add Remark (Update)
        [HttpPost, Route("EditRemark")]
        public IHttpActionResult EditRemark(PostTrxARRMonitoringCashInRemarkDetailQuarterly post)
        {
            try
            {
             
                TrxARRMonitoringCashInRemarkDetailQuarterly trx = new TrxARRMonitoringCashInRemarkDetailQuarterly();
                trx.Quarter = post.Quarter;
                trx.Periode = post.Periode;
                trx.OperatorID = post.OperatorID;
                trx.Remarks = post.Remarks;
                trx = _service.EditRemark(UserManager.User.UserToken, trx);
                return Ok(trx);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        #endregion

        #region Submit
        [HttpGet, Route("submit")]
        public IHttpActionResult Submit(string Type, int Year, int Quarterly)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var result = _service.Submit(Type, Year, Quarterly, userCredential.UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        #region List - Approval
        [HttpPost, Route("gridapproval")]
        public IHttpActionResult GetApprovalList(PostTrxARRMonitoringCashInRemarkDetailQuarterly post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<TrxARRMonitoringCashInRemarkDetailQuarterly> data = new List<TrxARRMonitoringCashInRemarkDetailQuarterly>();

                intTotalRecord = _service.GetDataCountApproval(userCredential.UserID, post.Periode, post.Quarter);

                //string UserID, string strWhereClause = "", int Periode = 0, int Quarterly = 0

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                data = _service.GetDataToListApproval(userCredential.UserID, post.Periode, post.Quarter,
                    strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        [HttpGet, Route("approval")]
        public IHttpActionResult Approval(string Type, int Year, int Quarter, int Isapproval, string Remark)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var result = _service.Approve(Type, Year, Quarter, userCredential.UserID, Isapproval, Remark);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}