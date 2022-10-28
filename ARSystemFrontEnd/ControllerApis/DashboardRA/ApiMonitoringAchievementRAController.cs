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

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/MonitoringAchievementRA")]
    public class ApiMonitoringAchievementRAController : ApiController
    {
        RTILeadTimeService LTservice = new RTILeadTimeService();


        [HttpGet, Route("DataChartByAverageLT")]
        public async Task<IHttpActionResult> DashboardLeadTimeAverage(int year, string groupby)
        {
            try
            {
                List<RTILeadTimeModel> data = new List<RTILeadTimeModel>();

                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    data.Add(new RTILeadTimeModel(userCredential.ErrorType, userCredential.ErrorMessage));
                    return Ok(data);
                }

                data = await LTservice.GetDataLeadTimeAvg(year, groupby);
                return Ok(data);
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }

        RTIAchievementService RTIservice = new RTIAchievementService();

        [HttpGet, Route("DataChartByGroupRTI")]
        public async Task<IHttpActionResult> DashboardLeadTimeRTI(string groupby, int year, string month)
        {
            try
            {
                List<RTIAchievementModel> data = new List<RTIAchievementModel>();

                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    data.Add(new RTIAchievementModel(userCredential.ErrorType, userCredential.ErrorMessage));
                    return Ok(data);
                }

                int? intMonth = convertMonth(month);
                data = await RTIservice.GetRTIAchivementByGroup(groupby, year, intMonth, String.Empty);
                return Ok(data);
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }

        RTIDoneNOverdueService serviceOD = new RTIDoneNOverdueService();

        [HttpGet, Route("DataChartByGroupOD")]
        public async Task<IHttpActionResult> DashboardOverdueChartGroup(int year, string customerid, string groupby)
        {
            try
            {
                List<vwRTINOverdueModel> data = new List<vwRTINOverdueModel>();
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    data.Add(new vwRTINOverdueModel(userCredential.ErrorType, userCredential.ErrorMessage));
                    return Ok(data);
                }

                data = await serviceOD.GetDataChartByGroup(year, customerid, userCredential.UserID, groupby);
                return Ok(data);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        private int convertMonth(string strmonth)
        {
            int month = 0;
            switch (strmonth)
            {
                case "JAN":
                    month = 1;
                    break;
                case "FEB":
                    month = 2;
                    break;
                case "MAR":
                    month = 3;
                    break;
                case "APR":
                    month = 4;
                    break;
                case "MAY":
                    month = 5;
                    break;
                case "JUN":
                    month = 6;
                    break;
                case "JUL":
                    month = 7;
                    break;
                case "AUG":
                    month = 8;
                    break;
                case "SEP":
                    month = 9;
                    break;
                case "OCT":
                    month = 10;
                    break;
                case "NOV":
                    month = 11;
                    break;
                case "DEC":
                    month = 12;
                    break;
            }
            return month;
        }


    }


}