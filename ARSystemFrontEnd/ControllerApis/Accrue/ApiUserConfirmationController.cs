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
using System.Collections.Specialized;
using System.Web;
using System.IO;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/UserConfirmation")]
    public class ApiUserConfirmationController : ApiController
    {
        UserConfirmationService client = new UserConfirmationService();
        [HttpPost, Route("grid")]
        public IHttpActionResult Grid(PostAccrueView post)
        {
            try
            {
                int intTotalRecord = 0;
                List<vwtrxDataAccrue> List = new List<vwtrxDataAccrue>();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);                

                intTotalRecord = client.GetUserConfirmToListCount(userCredential.UserID, post.vwtrxDataAccrue, post.date, post.week, false);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                List = client.GetUserConfirmToList(userCredential.UserID, post.vwtrxDataAccrue, post.date, post.week, strOrderBy, post.start, post.length).ToList();

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
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                ListId = client.GettrxDataAccrueListId(userCredential.UserID, post.vwtrxDataAccrue, post.date, post.week).ToList();
                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetWeekNowSelected")]
        public IHttpActionResult GetWeekNowSelected(PostAccrueView post)
        {
            try
            {
                string weekNow = "";
                weekNow = client.GetWeekNowSelected("");
                return Ok(weekNow);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("ConfirmUser")]
        public IHttpActionResult ConfirmUser()
        {
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["FileConfirm"];
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                PostAccrueView post = new PostAccrueView();
               
                post.ListID = nvc.Get("ListID").ToString().Split(',').ToList();
                
                trxDataAccrue data = new trxDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                {
                    data = MapModel(data, nvc, postedFile, "confirm");
                    data.Remarks = nvc.Get("remarks").ToString();
                    data.WeekTargetUser = int.Parse(nvc.Get("weekTargetUser").ToString());
                    data.TargetDateUser = DateTime.Parse(nvc.Get("targetDate").ToString());
                    data.RootCauseID = int.Parse(nvc.Get("rootCauseID").ToString());
                    data.PicaID = int.Parse(nvc.Get("picaID").ToString());
                    data.PicaDetailID = int.Parse(nvc.Get("picaDetailID").ToString());
                    data = client.ConfirmUser(userCredential.UserID, post.ListID.ToList(), data.Remarks, data);
                }                    

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Move")]
        public IHttpActionResult Move()
        {
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files["FileMove"];
                NameValueCollection nvc = HttpContext.Current.Request.Form;
                PostAccrueView post = new PostAccrueView();
                //post.ListID = nvc.Get("InvInternalPIC").ToList()
                post.ListID = nvc.Get("ListID").ToString().Split(',').ToList();
                post.department = nvc.Get("department").ToString();
                trxDataAccrue data = new trxDataAccrue();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                {
                    data = MapModel(data, nvc, postedFile, "move");
                    data.Remarks = nvc.Get("remarks").ToString();
                    data = client.Move(userCredential.UserID, post.ListID.ToList(), post.department, data);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #region Private Methods
        private trxDataAccrue MapModel(trxDataAccrue accrue, NameValueCollection nvc, HttpPostedFile postedFile, string type)
        {
            //string nullString = "null";
            
            if (postedFile != null)
            {
                string dir = "\\Accrue\\User\\" + type + "\\";
                string fileTimeStamp = Helper.Helper.GetFileTimeStamp(postedFile.FileName);
                string filePath = dir + fileTimeStamp;
                if(type == "move")
                {
                    accrue.FileNameMove = postedFile.FileName;
                    accrue.FilePathMove = filePath;
                    accrue.ContentTypeMove = postedFile.ContentType;
                }
                else
                {
                    accrue.FileNameConfirm = postedFile.FileName;
                    accrue.FilePathConfirm = filePath;
                    accrue.ContentTypeConfirm = postedFile.ContentType;
                }

                string path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.TbigSysDoc() + dir);
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                    di.Create();

                postedFile.SaveAs(path + fileTimeStamp);
            }

            return accrue;
        }
        #endregion
    }
}