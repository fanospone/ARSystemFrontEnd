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
    [RoutePrefix("api/CashInRemarkWeekly")]
    public class ApiARRMonitoringCashInRemarkWeeklyController : ApiController
    {
        TrxARRMonitoringCashInRemarkWeeklyService _service = new TrxARRMonitoringCashInRemarkWeeklyService();

        #region List - Requestor
        [HttpPost, Route("grid")]
        public IHttpActionResult GetBapsDataToGrid(PostTrxARRMonitoringCashInRemarkDetailWeekly post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<TrxARRMonitoringCashInRemarkDetailWeekly> data = new List<TrxARRMonitoringCashInRemarkDetailWeekly>();

                intTotalRecord = _service.GetDataCount(userCredential.UserID, post.Periode, post.Month, post.Week);

                //string UserID, string strWhereClause = "", int Periode = 0, int Month = 0

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                data = _service.GetDataToList(userCredential.UserID, post.Periode, post.Month, post.Week,
                    strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("filter")]
        public IHttpActionResult GetFilter(string Type, int Year, int Month, int Week)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var result = _service.GetFilterList(Type, Year, Month, userCredential.UserID, Week);
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
        public IHttpActionResult EditRemark(PostTrxARRMonitoringCashInRemarkDetailWeekly post)
        {
            try
            {
             
                TrxARRMonitoringCashInRemarkDetailWeekly trx = new TrxARRMonitoringCashInRemarkDetailWeekly();
                      trx.Periode = post.Periode;
                trx.OperatorID = post.OperatorID;
                trx.Month = post.Month;
                trx.Week = post.Week;
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
        public IHttpActionResult Submit(string Type, int Year, int Month, int Week)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var result = _service.Submit(Type, Year, Month, Week, userCredential.UserID);
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
        public IHttpActionResult GetApprovalList(PostTrxARRMonitoringCashInRemarkDetailWeekly post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<TrxARRMonitoringCashInRemarkDetailWeekly> data = new List<TrxARRMonitoringCashInRemarkDetailWeekly>();

                intTotalRecord = _service.GetDataCountApproval(userCredential.UserID, post.Periode, post.Month, post.Week);

                //string UserID, string strWhereClause = "", int Periode = 0, int Month = 0

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                data = _service.GetDataToListApproval(userCredential.UserID, post.Periode, post.Month, post.Week,
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
        public IHttpActionResult Approval(string Type, int Year, int Month, int Week, int Isapproval, string Remark)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var result = _service.Approve(Type, Year, Month, Week, userCredential.UserID, Isapproval, Remark);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}