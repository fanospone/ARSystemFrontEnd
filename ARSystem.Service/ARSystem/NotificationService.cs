using System;
using System.Collections.Generic;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystem.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "NotificationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select NotificationService.svc or NotificationService.svc.cs at the Solution Explorer and start debugging.
    public partial class NotificationService 
    {
        public List<vwNotification> GetNotification(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwNotificationRepository(context);
            List<vwNotification> list = new List<vwNotification>();
            
            try
            {
                string role = UserHelper.GetUserARPosition(UserID);

                if (role == "DEPT HEAD")
                    list = repo.GetList("TaskCount > 0 AND TaskName NOT LIKE '%Request Approved%'", "Sort");
                else if (role == "AR DATA")
                    list = repo.GetList("TaskCount > 0 AND TaskName NOT LIKE '%Approval%'", "Sort");
                    //list = repo.GetList("TaskCount > 0 AND TaskName LIKE '%CN External%'", "Sort");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwNotification((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NotificationService", "GetNotification", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
