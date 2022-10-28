using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/user")]
    public class ApiUserController : ApiController
    {
        [HttpGet, Route("single")]
        public IHttpActionResult GetSingleUser()
        {
            try
            {
                SecureAccessService.mstUser user = new SecureAccessService.mstUser();
                using (var client = new SecureAccessService.UserServiceClient())
                {
                    user = client.GetUserSingle(UserManager.User.UserToken);
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("password")]
        public IHttpActionResult ChangePassword(PostUserProfile post)
        {
            try
            {
                SecureAccessService.mstUser user = new SecureAccessService.mstUser();
                using (var client = new SecureAccessService.UserServiceClient())
                {
                    user = client.UpdatePassword(UserManager.User.UserToken, post.OldPassword, post.NewPassword);
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetPosition")]
        public IHttpActionResult GetPosition()
        {
            try
            {
                ARSystemService.vmStringResult Result= new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.UserServiceClient())
                {
                    Result = client.GetARUserPosition(UserManager.User.UserToken);
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
