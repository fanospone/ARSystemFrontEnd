using System;
using System.Collections.Generic;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Service;

namespace ARSystemFrontEnd
{
    [RoutePrefix("api/MonitoringBapDone")]
    public class ApiMonitoringBapsDoneController : ApiController
    {
        [HttpPost, Route("summaryHeader")]
        public async Task<IHttpActionResult> SummaryHeader(PostDasboardMonitoringBAPSDone post)
        {
            try
            {
                List<vwMonitoringBapsDoneHeader> headerList = new List<vwMonitoringBapsDoneHeader>();
                var client = new RADashboardMonitoringBAPSDoneService();

                MonitoringBapsDoneHeaderParam param = new MonitoringBapsDoneHeaderParam();
                param.BapsType = post.BapsType;
                param.GroupBy = post.GroupBy;
                param.CustomerID = post.CustomerID;
                param.CompanyId = post.CompanyID;
                param.Year = post.Year;
                param.StipID = post.STIPID;
                param.RegionID = post.RegionID;
                param.ProvinceID = post.ProvinceID;
                param.ProductID = post.ProductID;
                param.PowerTypeID = post.PowerTypeID;
                param.DataType = post.DataType;

                int intTotalRecord = 0;
                 intTotalRecord = await client.GetMonitoringBapsDoneHeaderCount(UserManager.User.UserToken, param);
                headerList = await client.GetMonitoringBapsDoneHeaderList(UserManager.User.UserToken, param, post.start, post.length);
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = headerList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("summaryDetail")]
        public async Task<IHttpActionResult> SummaryDetail(PostDasboardMonitoringBAPSDone post)
        {
            try
            {
                List<vwMonitoringBapsDoneDetail> detailList = new List<vwMonitoringBapsDoneDetail>();
                var client = new RADashboardMonitoringBAPSDoneService();

                vwMonitoringBapsDoneDetail data = new vwMonitoringBapsDoneDetail();
                data.ProductID = post.ProductID;
                data.STIPID = post.STIPID;
                data.Year = post.Year;
                data.Month = post.Month;
                data.PowerTypeID = int.Parse(post.PowerTypeID == null ? "0" : post.PowerTypeID);
                data.CustomerId = post.CustomerID;
                data.CompanyInvoiceId = post.CompanyID;
                data.RegionID = post.RegionID;
                data.ProvinceID = post.ProvinceID;

                if (post.DescID == "Total")
                {
                    data.CustomerId = null;
                    data.CompanyInvoiceId = null;
                    data.RegionID = null;
                    data.ProvinceID = null;
                }
                else
                {

                    if (post.GroupBy == "customer")
                        data.CustomerId = post.DescID;
                    else if (post.GroupBy == "company")
                        data.CompanyInvoiceId = post.DescID;
                    else if (post.GroupBy == "region")
                        data.RegionID = int.Parse(post.DescID);
                    else if (post.GroupBy == "province")
                        data.ProvinceID = int.Parse(post.DescID);

                }

                if (post.Month == 13)
                {
                    data.Month = null;
                }

                //Added by ASE
                data.SoNumber = post.schSONumber;
                data.SiteID = post.schSiteID;
                data.SiteName = post.schSiteName;
                data.CustomerSiteID = post.schSiteCustID;
                data.CustomerSiteName = post.schSiteCustName;
                int intTotalRecord = 0;
                intTotalRecord = await client.GetMonitoringBAPSDoneDetailCount(UserManager.User.UserToken, data, post.GroupBy, post.BapsType);
                detailList = await client.GetMonitoringBAPSDoneDetailList(UserManager.User.UserToken, data, post.GroupBy, post.BapsType, post.start, post.length);
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = detailList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
