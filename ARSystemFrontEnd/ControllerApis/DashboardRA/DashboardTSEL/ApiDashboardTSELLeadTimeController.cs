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


namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/DashboardTSELLeadTime")]
    public class ApiDashboardTSELLeadTimeController : ApiController
    {
        DashboardTSELLeadTimeService client = new DashboardTSELLeadTimeService();

        private readonly DashboardTSELLeadTimeService summary;
        public ApiDashboardTSELLeadTimeController()
        {
            summary = new DashboardTSELLeadTimeService();
        }
        
        /*
        [HttpGet, Route("GetData")]
        public IHttpActionResult GetData(string YearBill)
        {
            int ctr = 0;
            int[] dataarray;
            int[] dataSowarray = new int[12];
            int[] dataTargetarray = new int[12];
            string[] colorstring = new string[] { "#03d3fc", "#fc6b03", "#999591", "#c2a906", "#065ec2" };
            string[] sowName;
            string strWhereClause = " CustomerID = 'TSEL' AND SectionName = 'TOWER' ";
            List<HighchartData> result = new List<HighchartData>();

            if (!string.IsNullOrEmpty(YearBill))
                strWhereClause += " AND YearBill = " + YearBill;

            strWhereClause += " ";

            var data = summary.GetDataAverageLeadTime(strWhereClause);
            var dataSow = summary.GetDataAverageSection(strWhereClause);

            sowName = new string[data.Count()];

            foreach (var row in data)
            {
                dataarray = new int[1];
                dataarray[0] = row.AvgLeadTime;

                result.Add(new HighchartData
                {
                    type = "column",
                    name = row.SowName,
                    data = dataarray,
                    color = colorstring[ctr]
                });

                sowName[ctr] = row.SowName;
                ctr = ctr + 1;
                
            }

            return Ok(new
            {
                YearlyData = result,
                SowName = sowName
            });
        }

        [HttpGet, Route("GetDataAchievement")]
        public IHttpActionResult GetDataAchievement(string YearBill)
        {
            int ctr = 0;
            int[] dataarray;
            int[] dataSectionArray = new int[12];
            int[] dataTargetArray = new int[12];
            string[] colorstring = new string[] { "#03d3fc", "#fc6b03", "#999591", "#c2a906", "#065ec2" };
            string[] monthlists = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string strWhereClause = " CustomerID = 'TSEL' AND SectionName = 'TOWER' ";
            List<HighchartData> result = new List<HighchartData>();

            if (!string.IsNullOrEmpty(YearBill))
                strWhereClause += " AND YearBill = " + YearBill;

            strWhereClause += " ";

            var data = summary.GetDataAchievement(strWhereClause);
            var dataSection = summary.GetDataAchievementLeadTime(strWhereClause);

            foreach (var row in data.Select(p => p.SowName)
                                  .Distinct())
            {
                dataarray = new int[12];
                for (int i = 0; i < 12; i++)
                {
                    if (data.Where(w => w.MonthBill == (i + 1) && w.SowName == row.ToString()).FirstOrDefault() != null)
                        dataarray[i] = data.Where(w => w.MonthBill == (i + 1) && w.SowName == row.ToString()).FirstOrDefault().AvgLeadTime;
                    else
                        dataarray[i] = 0;
                }

                result.Add(new HighchartData
                {
                    type = "column",
                    name = "Average LT",
                    data = dataarray,
                    color = colorstring[ctr]
                });

                ctr = ctr + 1;
            }

            for (int i = 0; i < 12; i++)
            {
                if (dataSection.Where(w => w.MonthBill == (i + 1)).FirstOrDefault() != null)
                    dataSectionArray[i] = dataSection.Where(w => w.MonthBill == (i + 1)).FirstOrDefault().AvgSection;
                else
                    dataSectionArray[i] = 0;

                dataTargetArray[i] = 50;
            }

            result.Add(new HighchartData
            {
                type = "spline",
                name = "Average SOW",
                data = dataSectionArray,
                color = "orange"
            });

            result.Add(new HighchartData
            {
                type = "spline",
                name = "Target LT",
                data = dataTargetArray,
                color = "grey"
            });

            return Ok(new
            {
                MonthList = monthlists,
                YearlyData = result
            });
        }
        */

        [HttpGet, Route("GetLeadTimeData")]
        public IHttpActionResult GetLeadTimeData(string YearBill,string SectionName)
        {
            int ctr = 0;
            int[] dataarray;
            string[] sowName;
            int[] avgsection;
            int[] targetsection;
            string strWhereClause = " CustomerID = 'TSEL' AND SectionName = '"+ SectionName + "' ";
            List<HighchartData> result = new List<HighchartData>();

            if (!string.IsNullOrEmpty(YearBill))
                strWhereClause += " AND YearBill = " + YearBill;

            strWhereClause += " ";

            var data = summary.GetDataAverageLeadTime(strWhereClause);
            var dataAvg = summary.GetDataAverageSection(strWhereClause);

            if (SectionName == "TOWER")
            {
                //if (data.Where(w => w.SowName == "OVERBLAST").FirstOrDefault() == null)
                //{
                //    data.Add(new vwRADetailSiteRecurring
                //    {
                //        SowName = "OVERBLAST",
                //        AvgLeadTime = 0
                //    });
                //}

                //if (data.Where(w => w.SowName == "TOWER").FirstOrDefault() == null)
                //{
                //    data.Add(new vwRADetailSiteRecurring
                //    {
                //        SowName = "TOWER",
                //        AvgLeadTime = 0
                //    });
                //}
            }

            sowName = new string[data.Count()];
            avgsection = new int[data.Count()];
            targetsection = new int[data.Count()];
            dataarray = new int[data.Count()];
            string[] colorstring = new string[] { "#03d3fc", "#fc6b03", "#999591", "#c2a906", "#065ec2" };

            foreach (var row in data)
            {
                
                dataarray[ctr] = row.AvgLeadTime;
                sowName[ctr] = row.SowName;
                avgsection[ctr] = dataAvg.FirstOrDefault().AvgSection;
                targetsection[ctr] = 50;
                ctr = ctr + 1;

            }

            result.Add(new HighchartData
            {
                type = "column",
                name = SectionName,
                data = dataarray,
                color = "#7cb5ec"
            });

            result.Add(new HighchartData
            {
                type = "spline",
                name = "AVG Section",
                data = avgsection,
                color = "orange"
            });

            result.Add(new HighchartData
            {
                type = "spline",
                name = "Target",
                data = targetsection,
                color = "grey"
            });

            return Ok(new
            {
                DataList = result,
                SowName = sowName
            });
        }

        [HttpGet, Route("GetLeadTimeAchievement")]
        public IHttpActionResult GetLeadTimeAchievement(string YearBill, string SectionName)
        {
            int ctr = 0;
            int[] dataarray = new int[12];
            int[] dataLT = new int[12];
            string[] monthlists = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            string strWhereClause = " CustomerID = 'TSEL' AND SectionName = '" + SectionName + "' ";
            List<HighchartData> result = new List<HighchartData>();

            if (!string.IsNullOrEmpty(YearBill))
                strWhereClause += " AND YearBill = " + YearBill;

            strWhereClause += " ";

            var data = summary.GetDataAchievement(strWhereClause);
            var dataAvg = summary.GetDataAchievementLeadTime(strWhereClause);

            string[] colorstring = new string[] { "#03d3fc", "#fc6b03", "#999591", "#c2a906", "#065ec2" };


            for (int i = 1; i <= 12; i++)
            {
                if (data.Where(w => w.MonthBill == i).FirstOrDefault() != null)
                    dataarray[ctr] = data.Where(w => w.MonthBill == i).FirstOrDefault().AvgLeadTime;
                else
                    dataarray[ctr] = 0;

                if (dataAvg.Where(w => w.MonthBill == i).FirstOrDefault() != null)
                    dataLT[ctr] = dataAvg.Where(w => w.MonthBill == i).FirstOrDefault().AvgLeadTime;
                else
                    dataLT[ctr] = 0;

                ctr = ctr + 1;
            }

            result.Add(new HighchartData
            {
                type = "column",
                name = "Achievement",
                data = dataarray,
                color = "#7cb5ec",
                //yAxis = 1
            });

            result.Add(new HighchartData
            {
                type = "spline",
                name = "LeadTime",
                data = dataLT,
                color = "orange",
                yAxis = 1
            });

            return Ok(new
            {
                DataList = result,
                MonthList = monthlists
            });
        }
    }
}