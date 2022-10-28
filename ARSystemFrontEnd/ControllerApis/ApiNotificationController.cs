using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Helper;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/Notification")]
    public class ApiNotificationController : ApiController
    {
        NotificationService client = new NotificationService();
        [HttpGet, Route("GetList")]
        public IHttpActionResult GetNotificationList()
        {
            try
            {
                List<vwNotification> list = new List<vwNotification>();
                
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                //if (userCredential.ErrorType > 0)
                //    list.Add(new vwNotification(userCredential.ErrorType, userCredential.ErrorMessage));
                //else
                list = client.GetNotification(userCredential.UserID).ToList();



                return Ok(list);
            }
            catch(Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}