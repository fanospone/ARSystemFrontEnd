using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Models;
using ARSystem.Service;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/MstNotification")]
    public class ApiMstNotificationController : ApiController
    {
        private readonly MstNotificationService notif;

        public ApiMstNotificationController()
        {
            notif = new MstNotificationService();
        }

        [HttpPost, Route("getlist")]
        public IHttpActionResult getnotiflist(PostNotificationData post)
        {
            var datauser = UserService.CheckUserToken(UserManager.User.UserToken);

            string strWhereType = "";

            if(!string.IsNullOrEmpty(post.Action) && post.Action == "task")
            {
                strWhereType += " Type = 2  AND ";
            }
            else
            {
                strWhereType += " Type = 1  AND ";
            }

            var datalist = notif.GetNotificationToList(UserManager.User.UserToken, MappingParam(post) + strWhereType, "Id");

            return Ok(new { draw = post.draw, recordsTotal = datalist.Count, recordsFiltered = datalist.Count, data = datalist });
        }

        [HttpPost, Route("getdata")]
        public IHttpActionResult getdata(PostNotificationData post)
        {
            var datauser = UserService.CheckUserToken(UserManager.User.UserToken);

            var datalist = notif.GetNotificationToList(datauser.UserID, MappingParam(post), "Id").FirstOrDefault();

            return Ok(datalist);
        }

        [HttpPost, Route("submit")]
        public IHttpActionResult submit(PostNotificationData post)
        {
            var datauser = UserService.CheckUserToken(UserManager.User.UserToken);

            var datalist = notif.SubmitNotification(datauser.UserID, post.DataNotif);

            return Ok(datalist);
        }

        [HttpPost, Route("Claim")]
        public IHttpActionResult Claim(PostNotificationData post)
        {
            var datauser = UserService.CheckUserToken(UserManager.User.UserToken);

            var datalist = notif.ClaimNotification(datauser.UserID, post.DataNotif.Id);

            return Ok(datalist);
        }

        [HttpPost, Route("Update")]
        public IHttpActionResult Update(PostNotificationData post)
        {
            var datauser = UserService.CheckUserToken(UserManager.User.UserToken);

            var datalist = notif.UpdateNotification(datauser.UserID, post.DataNotif.Id);

            return Ok(datalist);
        }

        [HttpPost, Route("Delete")]
        public IHttpActionResult Delete(PostNotificationData post)
        {
            var datauser = UserService.CheckUserToken(UserManager.User.UserToken);

            var datalist = notif.DeleteNotification(datauser.UserID, post.DataNotif.Id);

            return Ok(datalist);
        }

        public string MappingParam(PostNotificationData post)
        {
            string strWhereClause = "";

            if (!string.IsNullOrEmpty(post.UserID))
            {
                strWhereClause += " RTRIM(LTRIM(SentTo)) = '" + post.UserID + "' AND ";
            }

            if (!string.IsNullOrEmpty(post.IsRead))
            {
                strWhereClause += " IsRead = " + post.IsRead + " AND ";
            }

            if (post.DataNotif != null && !string.IsNullOrEmpty(post.DataNotif.Id.ToString()))
            {
                strWhereClause += " Id = " + post.DataNotif.Id.ToString() + " AND ";
            }

            return strWhereClause;
        }
    }
}