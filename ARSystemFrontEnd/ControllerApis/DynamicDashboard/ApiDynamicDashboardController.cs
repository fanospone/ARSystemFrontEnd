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
using System.Threading.Tasks;
using ARSystemFrontEnd.Helper;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class ApiDynamicDashboardController : ApiController
    {
        DynamicDashboardService service = new DynamicDashboardService();

        [HttpPost, Route("DashboardToPDF")]
        public IHttpActionResult DashboardToPDF(BapsPrintPDF post)
        {
            try
            {
                string html = "<style>table {width:100%; border : 1px solid black; font-size:10px; font-family:Arial; border-collapse:collapse; text-align:left  } td{border : 1px solid black;  font-weight: normal; padding-right:3px;padding-left:3px;} th{border : 1px solid black; background-color:#A9A9A9;font-weight: bold; padding:3px;}</style>\r\n";


                html = html + post.htmlElements;
                System.IO.File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Dashboard/PrintDashboard.cshtml"), html);
                return Ok("OK");
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SaveDashboardTemplate")]
        public async Task<IHttpActionResult> SaveDashboardTemplate(PostDynamicDashboard post)
        {
            try
            {
                mstDashboardTemplate repo = new mstDashboardTemplate();
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    repo = new mstDashboardTemplate(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(repo);
                }
                post.dashboardTemplate.RoleID = userCredential.UserRoleID;
                post.dashboardTemplate.CreatedBy = userCredential.UserID;

                repo = await service.DashboardTemlpateSave(post.dashboardTemplate);

                return Ok(repo);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetDashboardTemplate")]
        public async Task<IHttpActionResult> GetDashboardTemplate(PostDynamicDashboard post)
        {



            try
            {
                List<mstDashboardTemplate> repoList = new List<mstDashboardTemplate>();
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    repoList.Add(new mstDashboardTemplate(userCredential.ErrorType, userCredential.ErrorMessage));
                    return Ok(repoList);
                }
                int intTotalRecord = 0;


                mstDashboardTemplate dataPost = new mstDashboardTemplate();
                dataPost.AggregatorName = post.AggregatorName;
                dataPost.DataSourceID = post.DataSourceID;
                dataPost.TemplateName = post.TemplateName != null ? post.TemplateName.TrimStart().TrimEnd() : post.TemplateName;
                dataPost.RendererName = post.RendererName;

                intTotalRecord = await service.GetDashboardTemlpateCount(dataPost);
                repoList = await service.GetDashboardTemlpateList(dataPost, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = repoList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("GetDataSourceDashboard")]
        public async Task<IHttpActionResult> GetDataSourceDashboard()
        {
            try
            {
                mstDataSourceDashboard post = new mstDataSourceDashboard();
                List<mstDataSourceDashboard> repoList = new List<mstDataSourceDashboard>();
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    repoList.Add(new mstDataSourceDashboard(userCredential.ErrorType, userCredential.ErrorMessage));
                    return Ok(repoList);
                }

                repoList = await service.GetDataSourceDashboardList(post, userCredential.UserRoleID);

                return Ok(repoList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetDashboardData")]
        public async Task<IHttpActionResult> GetDashboardData(PostDynamicDashboard post)
        {
            try
            {

                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    //repoList.Add(new mstDataSourceDashboard(userCredential.ErrorType, userCredential.ErrorMessage));
                    //return Ok(repoList);
                }


                var paramJSON = new Dictionary<string, object>();
                if (post.ParamJSON != null && post.ParamJSON != "")
                {
                    paramJSON = JsonConvert.DeserializeObject<Dictionary<string, object>>(post.ParamJSON);
                }


                int countData = await service.GetDashboardCountData(userCredential.UserID, post.DataSourceID, paramJSON);
                int start = 0;
                int countDataPerBatch = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CountDataPerBatch"]);
                int length = countDataPerBatch;
                int counter = countData / length;
                if (counter * length < countData)
                    counter = counter + 1;


                List<Dictionary<string, string>> listData = new List<Dictionary<string, string>>();


                for (int i = 0; i < counter; i++)
                {
                    if (i == counter - 1)
                        length = length - ((length - countData));

                    DashboardData data = new DashboardData();
                    data = await service.GetDashboardData(userCredential.UserID, post.DataSourceID, paramJSON, start, length);
                    listData.AddRange(new List<Dictionary<string, string>>(data.dataList));
                    start = length;
                    length = length + countDataPerBatch;

                }



                var objJSS = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };

                return Ok(objJSS.Serialize(listData));

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SaveDataSourceDashboard")]
        public async Task<IHttpActionResult> SaveDataSourceDashboard(PostDataSourceDashboard post)
        {
            try
            {
                mstDataSourceDashboard data = new mstDataSourceDashboard();
                List<mstDataSourceDashboard> repoList = new List<mstDataSourceDashboard>();
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    data = new mstDataSourceDashboard(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }

                data.DataSourceName = post.DataSourceName.TrimStart().TrimEnd();
                data.ViewName = post.ViewName.TrimStart().TrimEnd();
                data.DBContext = post.DatabaseName.TrimStart().TrimEnd();
                data.DBSchema = post.Schema.TrimStart().TrimEnd();
                data.ID = post.ID;
                data.RoleID = string.Join(",", post.RoleID);
                data.ParamFilter = string.Join(",", post.ParamFilter);
                data.ShowColumn = string.Join(",", post.ShowColumn);
                data = await service.SaveDataSourceDashboard(UserManager.User.UserToken, data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetDataSourceDashboard")]
        public async Task<IHttpActionResult> GetDataSourceDashboard(PostDataSourceDashboard post)
        {
            try
            {
                int intTotalRecord = 0;
                List<mstDataSourceDashboard> repoList = new List<mstDataSourceDashboard>();

                mstDataSourceDashboard data = new mstDataSourceDashboard();
                data.DataSourceName = post.DataSourceName != null ? post.DataSourceName.TrimStart().TrimEnd() : post.DataSourceName;
                data.ViewName = post.ViewName != null ? post.ViewName.TrimStart().TrimEnd() : post.ViewName;

                data.DBContext = post.DatabaseName != null ? post.DatabaseName.TrimStart().TrimEnd() : post.DatabaseName;

                intTotalRecord = await service.GetDataSourceDashboardCount(data);
                repoList = await service.GetDataSourceDashboard(data, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = repoList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("GetListColumns")]
        public async Task<IHttpActionResult> GetListColumns(string dbName, string vwName)
        {
            try
            {

                List<mstDataSourceDashboard> repoList = new List<mstDataSourceDashboard>();

                var listData = new List<Dictionary<string, string>>();
                listData = await service.GetColumnList(vwName, dbName);
                var objJSS = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
                return Ok(objJSS.Serialize(listData));

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("GetListSchemaDB")]
        public async Task<IHttpActionResult> GetListSchemaDB()
        {
            try
            {
                List<mstDataSourceDashboard> repoList = new List<mstDataSourceDashboard>();
                var listData = new List<Dictionary<string, string>>();
                listData = await service.GetSchemaDBList(UserManager.User.UserToken);
                var objJSS = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
                return Ok(objJSS.Serialize(listData));

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

    }

}