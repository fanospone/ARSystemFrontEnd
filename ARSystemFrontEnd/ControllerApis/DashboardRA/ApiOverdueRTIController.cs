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
    [RoutePrefix("api/OverdueRTI")]
    public class ApiOverdueRTIController : ApiController
    {
        RTIDoneNOverdueService service = new RTIDoneNOverdueService();

        [HttpGet, Route("DataChart")]
        public async Task<IHttpActionResult> DashboardOverdueChart(int year, string customerid, string groupBy)
        {
            try
            {
                vwRTINOverdueModel data = new vwRTINOverdueModel();
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    data = new vwRTINOverdueModel(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }

                data = await service.GetDataChart(year, customerid, userCredential.UserID, groupBy);
                return Ok(data);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpGet, Route("DataChartByGroup")]
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

                data = await service.GetDataChartByGroup(year, customerid, userCredential.UserID, groupby);
                return Ok(data);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost, Route("DataDetail")]
        public async Task<IHttpActionResult> DetailOverdueRTI(PostOverdueRTI post)
        {
            try
            {
                int month = 0;
                switch (post.Month)
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
                    case "MEI":
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
                List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
                int intTotalRecord = 0;
                intTotalRecord = service.GetCountDataDetail(post.DataType, post.Year, month, post.CustomerID);
                dataList = await service.GetDataDetail(post.DataType, post.Year, month, post.CustomerID, post.start, post.length, "");

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost, Route("DataDetailAll")]
        public async Task<IHttpActionResult> DetailOverdueRTIAll(PostOverdueRTI post)
        {
            int year = post.Year;
            List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
            int intTotalRecord = 0;
            intTotalRecord = service.GetCountDataDetail(post.DataType, post.Year, 2, post.CustomerID);

            dataList = await service.GetDataDetail(post.DataType, post.Year, 2, post.CustomerID, post.start, post.length, "");

           return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            
        }

    }

}