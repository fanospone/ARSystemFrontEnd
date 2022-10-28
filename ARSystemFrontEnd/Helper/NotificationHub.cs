using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using System.Timers;
using ARSystem.Service;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd
{
    public class NotificationHub : Hub
    {
        private readonly MstNotificationService notif;

        public NotificationHub()
        {
            notif = new MstNotificationService();
        }

        public void GetNotif()
        {
            var datauser = UserService.CheckUserToken(UserManager.User.UserToken);

            string strWhereClause = "";
            if(datauser != null)
            {
                strWhereClause += "  IsDeleted = 0  AND Type = 1 AND  ( SentTo = '" + datauser.UserID + "' OR SentTo Like '%"+ datauser.UserID + "%' ) AND ";
            }

            var datanotif = notif.GetNotificationToList(UserManager.User.UserToken, strWhereClause, "Id");

            if (datauser.UserID != null)
                Clients.Group(datauser.UserID).buildsNotifUser(datanotif);
        }

        public void GetTask()
        {
            var datauser = UserService.CheckUserToken(UserManager.User.UserToken);

            string strWhereClause = "";
            if (datauser != null)
            {
                strWhereClause += "  IsDeleted = 0  AND Type = 2 AND ( SentTo = '" + datauser.UserID + "' OR SentTo Like '%" + datauser.UserID + "%' ) AND ";
            }

            var datanotif = notif.GetNotificationToList(UserManager.User.UserToken, strWhereClause, "Id");

            if (datauser.UserID != null)
                Clients.Group(datauser.UserID).buildsTaskUser(datanotif);
        }

        public void SendChatMessage(string who, string message)
        {
            string name = Context.User.Identity.Name;
            Clients.Group(who).addChatMessage(name, message);
            //Clients.Group("2@2.com").addChatMessage(name, message);
        }

        public override Task OnConnected()
        {
            var datauser = UserService.CheckUserToken(UserManager.User.UserToken);
            string name = datauser.UserID;

            if (name != null)
                Groups.Add(Context.ConnectionId, name);

            return base.OnConnected();
        }
    }
}