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

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/FinanceConfirmation")]
    public class ApiFinanceConfirmationController : ApiController
    {
        FinanceConfirmationService client = new FinanceConfirmationService();

        [HttpPost, Route("grid")]
        public IHttpActionResult Grid(PostAccrueView post)
        {
            try
            {
                int intTotalRecord = 0;
                List<vwtrxDataAccrue> List = new List<vwtrxDataAccrue>();

                intTotalRecord = client.GetFinanceConfirmToListCount("", post.vwtrxDataAccrue, post.date, post.week);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                List = client.GetFinanceConfirmToList("", post.vwtrxDataAccrue, post.date,post.week, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = List });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("GettrxDataAccrueListId")]
        public IHttpActionResult GettrxDataAccrueListId(PostAccrueView post)
        {
            try
            {
                List<string> ListId = new List<string>();
                ListId = client.GettrxDataAccrueListId("", post.vwtrxDataAccrue, post.date, post.week).ToList();
                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("ConfirmFinance")]
        public IHttpActionResult ConfirmFinance(PostAccrueView post)
        {
            try
            {
                trxDataAccrue data = new trxDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    data = client.ConfirmFinance(userCredential.UserID, post.ListID.ToList(), post.remarks, post.paramAllData,post.vwtrxDataAccrue, post.date, post.week);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("ValidateAllType")]
        public IHttpActionResult ValidateAllType(PostAccrueView post)
        {
            try
            {
                trxDataAccrue data = new trxDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    data = client.ValidateTypeMove(userCredential.UserID, post.ListID.ToList(), post.paramAllData, post.vwtrxDataAccrue, post.date, post.week);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("Move")]
        public IHttpActionResult Move(PostAccrueView post)
        {
            try
            {
                trxDataAccrue data = new trxDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    data = client.Move(userCredential.UserID, post.ListID.ToList(), post.department, post.Type, post.paramAllData, post.vwtrxDataAccrue, post.date, post.week);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Calculate")]
        public IHttpActionResult Calculate(PostAccrueView post)
        {
            try
            {
                decimal sum = -1;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    sum = -1;
                else
                    sum = client.GetCalculateAmount(userCredential.UserID, post.vwtrxDataAccrue);

                return Ok(sum);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("UpdateAmount")]
        public IHttpActionResult UpdateAmount(PostAccrueView post)
        {
            try
            {
                trxDataAccrue data = new trxDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    data = client.UpdateAmount(userCredential.UserID, post.vwtrxDataAccrue);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}