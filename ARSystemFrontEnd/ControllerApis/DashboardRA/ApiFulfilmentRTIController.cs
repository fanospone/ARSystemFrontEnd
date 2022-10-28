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
    [RoutePrefix("api/FulfilmentRTI")]
    public class ApiFulfilmentRTIController : ApiController
    {
        RTIAchievementService service = new RTIAchievementService();

        //RTIFulfilmentervice service = new RTIFulfilmentService();

        //[HttpPost, Route("FillTarget")]
        //public IHttpActionResult FillTarget(PostFulfilmentRTI post)
        //{
        //    try
        //    {
        //        ARSystemService.trxRABapsValidation result = new ARSystemService.trxRABapsValidation();
        //        using (var client = new ARSystemService.NewBapsStartBapsServiceClient())
        //        {
        //            result = client.GetValidationData(UserManager.User.UserToken, post.strSoNumber, post.strSiteID, post.strCustomerId, post.strStipSiro.ToString());
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex);
        //    }
        //}

        [HttpPost, Route("DataChart")]
        public async Task<IHttpActionResult> DashboardFulfilmentChart(PostDashboardRTI model)
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
                var vWhereClause = " 1=1 ";
                var custIds = String.Empty;
                if (model.CustomerID != null && model.CustomerID.Count() > 0)
                {
                    custIds = string.Join(",", model.CustomerID);
                    var customerIds = string.Join(", ", model.CustomerID.Select(cust => "'" + cust + "'"));
                    if(!model.CustomerID.Any(cust => cust == "ALL"))
                    {
                        vWhereClause += " AND CustomerID in (" + customerIds + ")";
                    }
                }
                if(model.DepartmentCode != null && model.DepartmentCode.Count() > 0)
                {
                    var deptCodes = string.Join(", ", model.DepartmentCode.Select(dept => "'" + dept + "'"));
                    vWhereClause += " AND DepartmentCode in (" + deptCodes + ")";
                }

                data = await service.GetRTIAchivementByCustomer(custIds, model.Year.GetValueOrDefault(), vWhereClause);
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

        [HttpPost, Route("DataChartByGroup")]
        public async Task<IHttpActionResult> DashboardLeadTimeAverage(PostDashboardRTI model)
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
                var vWhereClause = " 1=1 ";
                var custIds = String.Empty;
                if (model.CustomerID != null && model.CustomerID.Count() > 0)
                {
                    custIds = string.Join(",", model.CustomerID);
                    var customerIds = string.Join(", ", model.CustomerID.Select(cust => "'" + cust + "'"));
                    if (!model.CustomerID.Any(cust => cust == "ALL"))
                    {
                        vWhereClause += " AND CustomerID in (" + customerIds + ")";
                    }
                }
                if (model.DepartmentCode != null && model.DepartmentCode.Count() > 0)
                {
                    var deptCodes = string.Join(", ", model.DepartmentCode.Select(dept => "'" + dept + "'"));
                    vWhereClause += " AND DepartmentCode in (" + deptCodes + ")";
                }
                int? intMonth = convertMonth(model.Month);
                data = await service.GetRTIAchivementByGroup(model.GroupBy, model.Year.GetValueOrDefault(), intMonth, vWhereClause);
                return Ok(data);
            }
            catch (Exception e)
            {
                return Ok(e);
            }

        }


        [HttpPost, Route("DataDetail")]
        public async Task<IHttpActionResult> DetailLeadTimeRTI(PostDashboardRTI post)
        {
            try
            {

                List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
                int intTotalRecord = 0;
                int monthInt = convertMonth(post.Month);
                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                intTotalRecord = service.GetCountRTIAchivementDetailByCustomer(post.CustomerID, monthInt, post.Year.GetValueOrDefault(), post.Category, post.DepartmentCode);
                dataList = await service.GetRTIAchivementDetailByCustomer(post.CustomerID, monthInt, post.Year.GetValueOrDefault(), post.Category, post.DepartmentCode, post.start, post.length, strOrderBy);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        //[HttpPost, Route("DataDetailByStatus")]
        //public async Task<IHttpActionResult> DetailLeadTimeRTIByStatus(PostRTILeadTimeByStatus post)
        //{
        //    try
        //    {

        //        List<RTINOverdueDetailModel> dataList = new List<RTINOverdueDetailModel>();
        //        int intTotalRecord = 0;
        //        intTotalRecord = service.GetCountStatusReconcileDetail(post.CustomerID, post.Year, post.currentStatus);
        //        dataList = await service.GetListStatusReconcileDetail(post.CustomerID, post.Year, post.currentStatus, post.start, post.length);

        //        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(e);
        //    }
        //}



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