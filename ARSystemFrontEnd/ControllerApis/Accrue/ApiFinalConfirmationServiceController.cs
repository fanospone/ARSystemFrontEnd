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
    [RoutePrefix("api/FinalConfirmation")]
    public class ApiFinalConfirmationServiceController : ApiController
    {
        FinalConfirmationService client = new FinalConfirmationService();
        [HttpPost, Route("grid")]
        public IHttpActionResult Grid(PostAccrueView post)
        {
            try
            {
                int intTotalRecord = 0;
                List<vwAccrueFinalConfirm> List = new List<vwAccrueFinalConfirm>();
                
                //intTotalRecord = client.GetfinalConfirmToListCount("", post.vwAccrueFinalConfirm, post.date, post.week);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                List = client.GetFinalConfirmToList("", post.vwAccrueFinalConfirm, post.date, post.week, strOrderBy, post.start, 50);

                
                return Ok(new { draw = post.draw, recordsTotal = List.Count, recordsFiltered = List.Count, data = List });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridUserConfirmed")]
        public IHttpActionResult gridUserConfirmed(PostAccrueView post)
        {
            try
            {
                int intTotalRecord = 0;
                vmDynamicTable model = new vmDynamicTable();

                intTotalRecord = client.GetUserConfirmedFinalToListCount("", post.date, post.week);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                model = client.GetUserConfirmedFinalToList("", post.date, post.week, strOrderBy, post.start, post.length);


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = model });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("ReConfirm")]
        public IHttpActionResult ReConfirm(PostAccrueView post)
        {
            try
            {
                trxDataAccrue data = new trxDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    data = client.ReConfirm(userCredential.UserID, post.date, post.week);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("FinalConfirm")]
        public IHttpActionResult FinalConfirm(PostAccrueView post)
        {
            try
            {
                trxDataAccrue data = new trxDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    data = client.FinalConfirm(userCredential.UserID, post.date, post.week);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CheckIsReConfirm")]
        public IHttpActionResult CheckIsReConfirm(PostAccrueView post)
        {
            try
            {
                trxDataAccrue data = new trxDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    data = client.IsExistReConfirm(userCredential.UserID, post.date, post.week);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}