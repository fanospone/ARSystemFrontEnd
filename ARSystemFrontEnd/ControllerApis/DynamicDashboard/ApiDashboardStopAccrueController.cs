using ARSystem.Domain.Models;
using ARSystem.Service;
using ARSystemFrontEnd.Models;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ARSystemFrontEnd.ControllerApis.DashboardStopAccrue
{
    [RoutePrefix("api/DashboardStopAccrue")]
    public class ApiDashboardStopAccrueController : ApiController
    {
        DashboardStopAccrueService service = new DashboardStopAccrueService();
        [HttpGet, Route("GetDirectorate")]
        public IHttpActionResult GetDirectorate()
        {
            try
            {
                var data = new List<MstInitialDepartment>();
                data = service.GetDirectorate();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("dashboardHeader")]
        public IHttpActionResult DashboardHeader(PostStopAccrueHeader post)
        {
            try
            {
                int totalRecords = 0;
                var data = new List<vwStopAccrueDashboardHeader>();
                var param = new vwStopAccrueDashboardHeader();
                param.RequestTypeID = post.RequestTypeID;
                param.RequestNumber = post.RequestNumber;
                param.CraetedDate = post.CreatedDate;
                param.CraetedDate2 = post.CreatedDate2;
                param.DepartName = post.DepartName;
                param.Activity = post.Activity;
                param.SubmissionDateFrom = post.SubmissionDateFrom;
                param.SubmissionDateTo = post.SubmissionDateTo;
                param.DirectorateCode = post.DirectorateCode;
                param.AccrueType = post.AccrueType;
                param.DetailCase = post.DetailCase;

                totalRecords = service.pGetCountDashboardHeader(param);
                data = service.GetDashboardHeader(param, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = totalRecords, recordsFiltered = totalRecords, data = data });
                //return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("dashboardDetail")]
        public IHttpActionResult DashboardDetail(PostStopAccrueHeader post)
        {
            try
            {
                var data = new List<vwStopAccrueDashboardDetail>();
                var param = new vwStopAccrueDashboardDetail();
                param.TrxStopAccrueHeaderID = post.HeaderID;
                param.DepartName = post.DepartName;
                param.DeptOrDetailCase = post.DeptOrDetailCase;
                param.DetailCase = post.DetailCase;
                param.SoNumberCount = post.SoNumberCount;

                //totalRecords = service.GetCountDashboardDetail(param);

                data = service.GetDashboardDetailList(param, post.start, Convert.ToInt32(post.SoNumberCount));
                return Ok(data);
                //return Ok(new { draw = post.draw, recordsTotal = totalRecords, recordsFiltered = totalRecords, data = data });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("CountData")]
        public IHttpActionResult DashboardData(PostStopAccrueHeader post)
        {
            try
            {
                var data = new List<vwStopAccrueDashboardHeader>();
                var ongoingStop = new List<vwStopAccrueDashboardHeader>();
                var param = new vwStopAccrueDashboardHeader();
                vwStopAccrueDashboardHeader headerData = new vwStopAccrueDashboardHeader();
                vwStopAccrueDashboardHeader headerDataStop = new vwStopAccrueDashboardHeader();
                param.SubmissionDateFrom = post.SubmissionDateFrom;
                param.SubmissionDateTo = post.SubmissionDateTo;
                param.DirectorateCode = post.DirectorateCode;
                param.DepartName = post.DepartName;
                param.Activity = post.Activity;

                var dataList = service.GetCountData(param, post.start, post.length);
                foreach (var dataongoinghold in dataList)
                {
                    headerData = dataongoinghold;
                    var sumData = dataList.Sum(x => x.CountDataa);
					if (Convert.ToDecimal(sumData) == 0)
                    {
                        sumData = 1;
                    }
                    Decimal count = Convert.ToDecimal(dataongoinghold.CountDataa) / Convert.ToDecimal(sumData);
                    headerData.CountData = Convert.ToInt32(Math.Floor(count * 100));

                    data.Add(headerData);
                }

                var ongoingStopList = service.GetCountDataSTOP(param, post.start, post.length);
                foreach (var dataongoingstop in ongoingStopList)
                {
                    headerDataStop = dataongoingstop;
                    var sumData = ongoingStopList.Sum(x => x.CountDataa);
					if (Convert.ToDecimal(sumData) == 0)
                    {
                        sumData = 1;
                    }
                    Decimal count = Convert.ToDecimal(dataongoingstop.CountDataa) / Convert.ToDecimal(sumData);
                    headerDataStop.CountData = Convert.ToInt32(Math.Floor(count * 100));

                    ongoingStop.Add(headerDataStop);
                }
                return Ok(new { data = data, OngoingStop = ongoingStop });
                //return Ok(new { draw = post.draw, recordsTotal = totalRecords, recordsFiltered = totalRecords, data = data });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [HttpPost, Route("CountDataFinish")]
        public IHttpActionResult DashboardDataFinish(PostStopAccrueHeader post)
        {
            try
            {
                var data = new List<vwStopAccrueDashboardHeader>();
                var finishStop = new List<vwStopAccrueDashboardHeader>();
                var param = new vwStopAccrueDashboardHeader();
                vwStopAccrueDashboardHeader headerData = new vwStopAccrueDashboardHeader();
                vwStopAccrueDashboardHeader headerDataStop = new vwStopAccrueDashboardHeader();
                param.SubmissionDateFrom = post.SubmissionDateFrom;
                param.SubmissionDateTo = post.SubmissionDateTo;
                param.DirectorateCode = post.DirectorateCode;
                param.DepartName = post.DepartName;
                param.Activity = post.Activity;

                var dataList = service.GetCountDataFinish(param, post.start, post.length);
                foreach (var datafinishhold in dataList)
                {
                    headerData = datafinishhold;
                    var sumData = dataList.Sum(x => x.CountDataa);
                    if (Convert.ToDecimal(sumData) == 0)
                    {
                        sumData = 1;
                    }
                    Decimal count = Convert.ToDecimal(datafinishhold.CountDataa) / Convert.ToDecimal(sumData);
                    headerData.CountData = Convert.ToInt32(Math.Floor(count * 100));

                    data.Add(headerData);
                }
                var ongoingStopList = service.GetCountDataSTOPFinish(param, post.start, post.length);
                foreach (var datafinishstop in ongoingStopList)
                {
                    headerDataStop = datafinishstop;
                    var sumData = ongoingStopList.Sum(x => x.CountDataa);
                    if (Convert.ToDecimal(sumData) == 0)
                    {
                        sumData = 1;
                    }
                    Decimal count = Convert.ToDecimal(datafinishstop.CountDataa) / Convert.ToDecimal(sumData);
                    headerDataStop.CountData = Convert.ToInt32(Math.Floor(count * 100));

                    finishStop.Add(headerDataStop);
                }

                return Ok(new { data = data, OngoingStop = finishStop });
                //return Ok(new { draw = post.draw, recordsTotal = totalRecords, recordsFiltered = totalRecords, data = data });
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }
    }

}