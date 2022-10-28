using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service
{
    public class MstNotificationService
    {
        public List<Notification> GetNotificationToList(string UserID, string WhereClause, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var ActivityRepo = new NotificationRepository(context);
            List<Notification> listActivity = new List<Notification>();

            try
            {
                string strWhereClause = "1=1 AND ";

                if (!string.IsNullOrEmpty(WhereClause))
                {
                    strWhereClause += WhereClause;
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                listActivity = ActivityRepo.GetList(strWhereClause, strOrderBy);


                return listActivity;
            }
            catch (Exception ex)
            {
                listActivity.Add(new Notification((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NotificationService", "GetNotificationToList", UserID)));
                return listActivity;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<Notification> GetNotificationByUserID(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var ActivityRepo = new NotificationRepository(context);
            List<Notification> listActivity = new List<Notification>();

            try
            {

                listActivity = ActivityRepo.pGetByUserID(UserID);


                return listActivity;
            }
            catch (Exception ex)
            {
                listActivity.Add(new Notification((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NotificationService", "GetNotificationByUserID", UserID)));
                return listActivity;
            }
            finally
            {
                context.Dispose();
            }
        }

        public Notification SubmitNotification(string UserID, Notification post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var ActivityRepo = new NotificationRepository(context);
            List<Notification> listActivity = new List<Notification>();

            try
            {
                post = ActivityRepo.Create(post);

                return post;
            }
            catch (Exception ex)
            {
                return new Notification((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NotificationService", "SubmitNotification", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public Notification ClaimNotification(string UserID, long Id)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var ActivityRepo = new NotificationRepository(context);
            List<Notification> listActivity = new List<Notification>();
            Notification post = new Notification();

            try
            {
                post.IsReminder = true;
                post.SentTo = UserID;
                post.NotificationType = "Warning";
                post.UpdateBy = UserID;
                post.Id = Id;

                post = ActivityRepo.Claim(post);

                return post;
            }
            catch (Exception ex)
            {
                return new Notification((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NotificationService", "SubmitNotification", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public Notification UpdateNotification(string UserID, long Id)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var ActivityRepo = new NotificationRepository(context);
            List<Notification> listActivity = new List<Notification>();
            Notification post = new Notification();

            try
            {
                post.IsRead = true;
                post.IsDeleted = false;
                post.UpdateBy = UserID;
                post.Id = Id;

                post = ActivityRepo.Update(post);

                return post;
            }
            catch (Exception ex)
            {
                return new Notification((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NotificationService", "SubmitNotification", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public Notification DeleteNotification(string UserID, long Id)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var ActivityRepo = new NotificationRepository(context);
            List<Notification> listActivity = new List<Notification>();
            Notification post = new Notification();

            try
            {
                post.IsRead = true;
                post.IsDeleted = true;
                post.UpdateBy = UserID;
                post.Id = Id;

                post = ActivityRepo.Update(post);

                return post;
            }
            catch (Exception ex)
            {
                return new Notification((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NotificationService", "SubmitNotification", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }


        public Notification SubmitBulkyNotification(string UserID, string Action, List<Notification> post)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var ActivityRepo = new NotificationRepository(context);
            List<Notification> listActivity = new List<Notification>();

            try
            {
                if (Action.Equals("Create"))
                {
                    post = ActivityRepo.CreateBulky(post);
                }

                return post.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Notification((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "NotificationService", "SubmitBulkyNotification", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
