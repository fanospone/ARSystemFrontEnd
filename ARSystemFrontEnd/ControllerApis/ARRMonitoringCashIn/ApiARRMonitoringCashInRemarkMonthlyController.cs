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
    [RoutePrefix("api/CashInRemarkMonthly")]
    public class ApiARRMonitoringCashInRemarkMonthlyController : ApiController
    {
        TrxARRMonitoringCashInRemarkMonthlyService _service = new TrxARRMonitoringCashInRemarkMonthlyService();


        #region List - Requeastor
        [HttpPost, Route("grid")]
        public IHttpActionResult GetBapsDataToGrid(PostTrxARRMonitoringCashInRemarkDetailMonthly post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<TrxARRMonitoringCashInRemarkDetailMonthly> data = new List<TrxARRMonitoringCashInRemarkDetailMonthly>();

                intTotalRecord = _service.GetDataCount(userCredential.UserID, post.Periode, post.Month);

                //string UserID, string strWhereClause = "", int Periode = 0, int Month = 0

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                data = _service.GetDataToList(userCredential.UserID, post.Periode, post.Month,
                    strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("filter")]
        public IHttpActionResult GetSTIPCategoryToList(string Type, int Year, int Month)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var result = _service.GetFilterList(Type, Year, Month, userCredential.UserID);
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
        public IHttpActionResult EditRemark(PostTrxARRMonitoringCashInRemarkDetailMonthly post)
        {
            try
            {
             
                TrxARRMonitoringCashInRemarkDetailMonthly trx = new TrxARRMonitoringCashInRemarkDetailMonthly();
                trx.Month = post.Month;
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
        public IHttpActionResult Submit(string Type, int Year, int Month)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var result = _service.Submit(Type, Year, Month, userCredential.UserID);
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
        public IHttpActionResult GetApprovalList(PostTrxARRMonitoringCashInRemarkDetailMonthly post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<TrxARRMonitoringCashInRemarkDetailMonthly> data = new List<TrxARRMonitoringCashInRemarkDetailMonthly>();

                intTotalRecord = _service.GetDataCountApproval(userCredential.UserID, post.Periode, post.Month);

                //string UserID, string strWhereClause = "", int Periode = 0, int Month = 0

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                data = _service.GetDataToListApproval(userCredential.UserID, post.Periode, post.Month,
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
        public IHttpActionResult Approval(string Type, int Year, int Month, int Isapproval, string Remark)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var result = _service.Approve(Type, Year, Month, userCredential.UserID, Isapproval, Remark);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}