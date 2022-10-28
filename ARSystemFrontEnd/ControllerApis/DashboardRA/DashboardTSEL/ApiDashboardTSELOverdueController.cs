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
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using System.Threading.Tasks;
using ARSystemFrontEnd.Helper;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/DashboardTSEL")]
    public class ApiDashboardTSELOverdueController : ApiController
    {
        DashboardTSELOverdueService client = new DashboardTSELOverdueService();
        private readonly DashboardTSELOverdueService summary;
        private readonly DashboardTSELService Detailsummary;
        public ApiDashboardTSELOverdueController()
        {
            summary = new DashboardTSELOverdueService();
            Detailsummary = new DashboardTSELService();
        }


        [HttpGet, Route("GetDataOverdue")]
        public IHttpActionResult GetDataOverdue(string YearBill)
        {
            var dataPercentageTotal = summary.GetDataTSELOverduePercentage(YearBill, "PercentageSummary");
            var dataPercentageTower = summary.GetDataTSELOverduePercentage(YearBill, "PercentageTower");
            var dataPercentageNonTower = summary.GetDataTSELOverduePercentage(YearBill, "PercentageNonTower");

            var dataChartTower = summary.GetDataTSELOverdueChart(YearBill, "ChartTower");
            var dataChartNonTower = summary.GetDataTSELOverdueChart(YearBill, "ChartNonTower");

            var dataChartVersusTower = summary.GetDataTSELOverdueChart(YearBill, "ChartVersusTower");
            var dataChartVersusNonTower = summary.GetDataTSELOverdueChart(YearBill, "ChartVersusNonTower");

            return Ok(new
            {
                PercentageTotal = dataPercentageTotal,
                PercentageTower = dataPercentageTower,
                PercentageNonTower = dataPercentageNonTower,
                ChartTower = dataChartTower,
                ChartNonTower = dataChartNonTower,
                ChartVersusTower = dataChartVersusTower,
                ChartVersusNonTower = dataChartVersusNonTower
            });
        }

        [HttpPost, Route("GetDetailSiteOverdue")]
        public IHttpActionResult GetDetailSiteOverdue(PostDashboardTSEL post)
        {
            try
            {
                string strWhereClause = " 1=1 ";

                if (!string.IsNullOrEmpty(post.YearBill))
                    strWhereClause += " AND YearBill = " + post.YearBill;

                if (!string.IsNullOrEmpty(post.SecName))
                    strWhereClause += " AND SectionName = '" + post.SecName + "'";

                if (!string.IsNullOrEmpty(post.SOWName))
                    strWhereClause += " AND SowName = '" + post.SOWName + "'";
                if (!string.IsNullOrEmpty(post.IsOverdue))
                {
                    if (post.IsOverdue == "Overdue")
                        strWhereClause += " AND LeadTime > " + 50;
                    else
                        strWhereClause += " AND LeadTime <= " + 50;
                }
                else
                    strWhereClause += " AND LeadTime > " + 50;

                var CountOfRows = Detailsummary.GetCountDetailSite(strWhereClause);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                
                var data = Detailsummary.GetPageDetailSite(strWhereClause, post.start, post.length, strOrderBy);

                return Ok(new { draw = post.draw, recordsTotal = CountOfRows, recordsFiltered = CountOfRows, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex.InnerException);
            }
        }

        [HttpGet, Route("STIPCategory")]
        public IHttpActionResult GetSTIPCategoryToList()
        {
            try
            {               
                var result = summary.GetMasterSTIPListDropdown().ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("CompanyList")]
        public IHttpActionResult GetCompanyToList()
        {
            try
            {
                var result = summary.GetMasterCompanyListDropdown().ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ProductList")]
        public IHttpActionResult GetProductToList(string sectionID, string sowID)
        {
            try
            {
                var result = summary.GetProductListDropdown(sectionID,sowID).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("SOWbyParamList")]
        public IHttpActionResult GetSOWbyParamToList(string sectionID)
        {
            try
            {
                var result = summary.GetSOWListDropdown(sectionID).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}