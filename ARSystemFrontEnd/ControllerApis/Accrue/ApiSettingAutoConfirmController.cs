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
using System.Web;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/SettingAutoConfirm")]
    public class ApiSettingAutoConfirmController : ApiController
    {
        SettingAutoConfirmService client = new SettingAutoConfirmService();

        [HttpPost, Route("grid")]
        public IHttpActionResult Grid(PostAccrueView post)
        {
            try
            {
                int intTotalRecord = 0;
                List<vwmstAccrueSettingAutoConfirm> List = new List<vwmstAccrueSettingAutoConfirm>();

                intTotalRecord = client.GetSettingAutoConfirmToListCount("", post.vwmstAccrueSettingAutoConfirm, post.date, post.week);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                List = client.GetSettingAutoConfirmToList("", post.vwmstAccrueSettingAutoConfirm, post.date, post.week, strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = List });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Save")]
        public IHttpActionResult Save(PostAccrueView post)
        {
            try
            {               
                mstAccrueSettingAutoConfirm data = new mstAccrueSettingAutoConfirm();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new mstAccrueSettingAutoConfirm(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                {
                    data = client.Save(userCredential.UserID, post.vwmstAccrueSettingAutoConfirm, post.date, post.week, post.autoConfirmDate);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Delete")]
        public IHttpActionResult Delete(PostAccrueView post)
        {
            try
            {
                mstAccrueSettingAutoConfirm data = new mstAccrueSettingAutoConfirm();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new mstAccrueSettingAutoConfirm(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                {
                    data = client.Delete(userCredential.UserID, post.vwmstAccrueSettingAutoConfirm);
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