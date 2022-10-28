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
    [RoutePrefix("api/LeadTimeRTI")]
    public class ApiLeadTimeRTIController : ApiController
    {
        RTILeadTimeService service = new RTILeadTimeService();

        [HttpGet, Route("DataChartByOperators")]
        public async Task<IHttpActionResult> DashboardLeadTimeChart(int year, string groupby)
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

                data = await service.GetDataLeadTime(year, groupby);
                return Ok(data);
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }

        [HttpGet, Route("DataChartByAverage")]
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

                data = await service.GetDataLeadTimeAvg(year, groupby);
                return Ok(data);
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }

        [HttpGet, Route("DataChartByStatus")]
        public async Task<IHttpActionResult> DashboardLeadTimebyStatus(string customerid, int year)
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

                data = await service.GetStatusReconcile(customerid, year);
                return Ok(data);
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }

        [HttpPost, Route("DataDetail")]
        public async Task<IHttpActionResult> DetailLeadTimeRTI(PostRTILeadTime post)
        {
            try
            {

                List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
                int intTotalRecord = 0;
                intTotalRecord = service.GetCountLeadTimeDetail(post.LeadTime, post.Year, post.CustomerID);
                dataList = await service.GetDataLeadTimeDetail(post.LeadTime, post.Year, post.CustomerID, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost, Route("DataDetailByStatus")]
        public async Task<IHttpActionResult> DetailLeadTimeRTIByStatus(PostRTILeadTimeByStatus post)
        {
            try
            {

                List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
                int intTotalRecord = 0;
                intTotalRecord = service.GetCountStatusReconcileDetail(post.CustomerID, post.Year, post.currentStatus);
                dataList = await service.GetListStatusReconcileDetail(post.CustomerID, post.Year, post.currentStatus, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }



        //[HttpPost, Route("DataDetailAll")]
        //public async Task<IHttpActionResult> DetailLeadTimeRTIAll(PostOverdueRTI post)
        //{
        //    int year = post.Year;
        //    List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
        //    int intTotalRecord = 0;
        //    intTotalRecord = service.GetCountDataDetail(post.DataType, post.Year, 2, post.CustomerID);

        //    dataList = await service.GetDataDetail(post.DataType, post.Year, 2, post.CustomerID, post.start, post.length, "");

        //    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });

        //}

    }

}