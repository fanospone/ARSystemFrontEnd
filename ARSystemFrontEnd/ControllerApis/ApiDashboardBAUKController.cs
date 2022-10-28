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
    [RoutePrefix("api/dashboardBAUK")]
    public class ApiDashboardBAUKController : ApiController
    {
        private readonly DashboardBAUKService dashboardBAUKService;
        public ApiDashboardBAUKController()
        {
            dashboardBAUKService = new DashboardBAUKService();
        }

        [HttpPost, Route("activity")]
        public IHttpActionResult GetActivityData(vmDashboardBAUKFilter filter)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                var dtNow = ARSystem.Service.Helper.GetDateTimeNow();
                if (filter.Months.Count == 1 && filter.Months[0] == 0)
                    filter.Months.Add(dtNow.Month);
                filter.Month = filter.Months.Count == 0 ? dtNow.Month : filter.Months[filter.Months.Count - 1];
                filter.Year = filter.Year == 0 ? dtNow.Year : filter.Year;

                var post = new PostBAUKActivity();

                List<vmDashboardBAUKActivity> result = new List<vmDashboardBAUKActivity>();

                result = dashboardBAUKService.GetDashboardActivity(userCredential.UserID, filter).ToList();

                return Ok(new { draw = post.draw, recordsTotal = 0, recordsFiltered = 0, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("activity/detail")]
        public IHttpActionResult GetActivityDetailData(PostBAUKDetail post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                var dtNow = ARSystem.Service.Helper.GetDateTimeNow();

                var filter = new vmDashboardBAUKDetail();
                if (post.Months.Count == 1 && post.Months[0] == 0)
                    filter.Months.Add(dtNow.Month);
                else
                    filter.Months = post.Months;
                filter.Month = post.Months.Count == 1 && post.Months[0] == 0 ? dtNow.Month : post.Months[post.Months.Count - 1];
                filter.Year = post.Year == 0 ? dtNow.Year : post.Year;
                filter.CompanyIDs = post.CompanyIDs;
                filter.CustomerIDs = post.CustomerIDs;
                filter.STIPIDs = post.STIPIDs;
                filter.ProductIDs = post.ProductIDs;
                filter.SelectedData = post.SelectedData;

                List<vmDashboardBAUKDetail> result = new List<vmDashboardBAUKDetail>();

                result = dashboardBAUKService.GetDashboardActivityDetail(userCredential.UserID, filter).ToList();

                return Ok(new { draw = post.draw, recordsTotal = 0, recordsFiltered = 0, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("forecast")]
        public IHttpActionResult GetForecastData(vmDashboardBAUKFilter filter)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                var dtNow = ARSystem.Service.Helper.GetDateTimeNow();
                filter.Month = filter.Month == 0 ? dtNow.Month : filter.Month;
                filter.Year = filter.Year == 0 ? dtNow.Year : filter.Year;

                var post = new PostBAUKActivity();

                List<vmDashboardBAUKForecast> result = new List<vmDashboardBAUKForecast>();

                result = dashboardBAUKService.GetDashboardForecast(userCredential.UserID, filter).ToList();

                return Ok(new { draw = post.draw, recordsTotal = 0, recordsFiltered = 0, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("forecast/detail")]
        public IHttpActionResult GetForecastDetailData(PostBAUKDetail post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                var dtNow = ARSystem.Service.Helper.GetDateTimeNow();

                var filter = new vmDashboardBAUKDetail();
                filter.Month = post.Month == 0 ? dtNow.Month : post.Month;
                filter.Year = post.Year == 0 ? dtNow.Year : post.Year;
                filter.CompanyIDs = post.CompanyIDs;
                filter.CustomerIDs = post.CustomerIDs;
                filter.STIPIDs = post.STIPIDs;
                filter.ProductIDs = post.ProductIDs;
                filter.SelectedData = post.SelectedData;

                List<vmDashboardBAUKDetail> result = new List<vmDashboardBAUKDetail>();

                result = dashboardBAUKService.GetDashboardForecastDetail(userCredential.UserID, filter).ToList();

                return Ok(new { draw = post.draw, recordsTotal = 0, recordsFiltered = 0, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("achievement")]
        public IHttpActionResult GetAchievementData(vmDashboardBAUKFilter filter)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                var dtNow = ARSystem.Service.Helper.GetDateTimeNow();
                filter.Month = filter.Month == 0 ? dtNow.Month : filter.Month;
                filter.Year = filter.Year == 0 ? dtNow.Year : filter.Year;

                var post = new PostBAUKActivity();

                List<vmDashboardBAUKAchievement> result = new List<vmDashboardBAUKAchievement>();

                result = dashboardBAUKService.GetDashboardAchievement(userCredential.UserID, filter).ToList();

                return Ok(new { draw = post.draw, recordsTotal = 0, recordsFiltered = 0, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("achievement/detail")]
        public IHttpActionResult GetAchievementDetailData(PostBAUKDetail post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                var dtNow = ARSystem.Service.Helper.GetDateTimeNow();

                var filter = new vmDashboardBAUKDetail();
                filter.Month = post.Month == 0 ? dtNow.Month : post.Month;
                filter.Year = post.Year == 0 ? dtNow.Year : post.Year;
                filter.CompanyIDs = post.CompanyIDs;
                filter.CustomerIDs = post.CustomerIDs;
                filter.STIPIDs = post.STIPIDs;
                filter.ProductIDs = post.ProductIDs;

                List<vmDashboardBAUKDetail> result = new List<vmDashboardBAUKDetail>();

                result = dashboardBAUKService.GetDashboardAchievementDetail(userCredential.UserID, filter).ToList();

                return Ok(new { draw = post.draw, recordsTotal = 0, recordsFiltered = 0, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("rejectSummary")]
        public IHttpActionResult GetRejectSummaryData(vmDashboardBAUKFilter filter)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                var dtNow = ARSystem.Service.Helper.GetDateTimeNow();
                if (filter.Months.Count == 1 && filter.Months[0] == 0)
                    filter.Months.Add(dtNow.Month);
                filter.Month = filter.Months.Count == 0 ? dtNow.Month : filter.Months[filter.Months.Count - 1];
                filter.Year = filter.Year == 0 ? dtNow.Year : filter.Year;

                var post = new PostBAUKRejectSummary();

                List<vmDashboardBAUKReject> result = new List<vmDashboardBAUKReject>();

                result = dashboardBAUKService.GetDashboardReject(userCredential.UserID, filter).ToList();

                return Ok(new { draw = post.draw, recordsTotal = 0, recordsFiltered = 0, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("rejectSummary/Detail")]
        public IHttpActionResult GetRejectDetailData(PostBAUKRejectDocDetail post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                vwBAUKRejectDocumentDetail filter = new vwBAUKRejectDocumentDetail();
                List<int> listMonths = new List<int>();

                var dtNow = ARSystem.Service.Helper.GetDateTimeNow();
                if (post.Months.Count == 1 && post.Months[0] == 0)
                    listMonths.Add(dtNow.Month);
                else
                    listMonths = post.Months;
                filter.Month = post.Months.Count == 1 && post.Months[0] == 0 ? dtNow.Month : post.Months[post.Months.Count - 1];
                filter.Year = post.Year == 0 ? dtNow.Year : post.Year;
                filter.CustomerID = post.CustomerID;
                filter.STIPID = post.STIPID;
                filter.CheckType = post.CheckType;

                List<vwBAUKRejectDocumentDetail> result = new List<vwBAUKRejectDocumentDetail>();

                int totCount = dashboardBAUKService.GetCountBAUKRejectDetail(userCredential.UserID, filter, listMonths);

                result = dashboardBAUKService.GetListBAUKRejectDetail(userCredential.UserID, filter, listMonths).ToList();

                return Ok(new { draw = post.draw, recordsTotal = totCount, recordsFiltered = 0, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}