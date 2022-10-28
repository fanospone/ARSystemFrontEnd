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
    [RoutePrefix("api/DashboardTSEL")]
    public class ApiDashboardTSELController : ApiController
    {
        DashboardTSELService client = new DashboardTSELService();

        private readonly DashboardTSELService summary;
        private const string CacheKeySection = "MstRASection";
        private const string CacheKeySOW = "MstRASOW";

        public ApiDashboardTSELController()
        {
            summary = new DashboardTSELService();
        }

        [HttpPost, Route("TSELGrid")]
        // GET: ApiDashboardTSEL
        public IHttpActionResult GetDashboardTSELDataToGrid(PostDashboardTSEL post)
        {
            try
            {
                var param = new vwRABapsSite();
                param.CompanyInvoiceId = post.strCompanyInvoice;
                param.CustomerID = post.strCustomerInvoice;
                param.SectionProductId = post.strSection;
                param.SowProductId = post.strSOW;
                param.YearBill = post.strBillingYear;
                param.RegionID = post.strRegional;
                param.ProvinceID = post.strProvince;
                param.ProductID = post.strTenantType;
                param.SONumber = post.schSONumber;
                param.SiteID = post.schSiteID;
                param.SiteName = post.schSiteName;
                param.CustomerSiteID = post.schCustomerSiteID;
                param.CustomerSiteName = post.schCustomerSiteName;
                param.RegionName = post.schRegionName;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                //if (!string.IsNullOrWhiteSpace(post.schCustomerID))
                //  param.CustomerID = post.schCustomerID;
                //if (post.schYearBill > 0)
                //  param.YearBill = post.schYearBill;
                param.TargetYear = post.strYearTargetHistory;
                param.TargetMonth = post.strMonthTargetHistory;
                param.StartInvoiceDate = post.StartInvoiceDate;
                param.EndInvoiceDate = post.EndInvoiceDate;


                int intTotalRecord = 0;

                List<vwRABapsSite> dashboardtseldata = new List<vwRABapsSite>();

                intTotalRecord = client.GetTrxDashbordTSELDataCount(param);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                if (intTotalRecord == 0)
                {
                    dashboardtseldata = client.GetTrxDashbordTSELDataToList(param, post.start, 0).ToList();
                }
                else
                    dashboardtseldata = client.GetTrxDashbordTSELDataToList(param, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dashboardtseldata });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpGet, Route("Section")]
        public IHttpActionResult GetSectionToList()
        {
            try
            {
                List<MstRASectionProduct> section = new List<MstRASectionProduct>();
                //section = client.GetSectionToList("", "").ToList();

                // Modification Or Added By Ibnu Setiawan 20. February 2020 
                section = HttpRuntime.Cache.GetOrStore<List<MstRASectionProduct>>($"User{CacheKeySection}", () => client.GetSectionToList(UserManager.User.UserToken, "").ToList())
                        .ToList();

                return Ok(section);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpGet, Route("SOW")]
        public IHttpActionResult GetSOWToList(string SectionID)
        {
            try
            {
                //List<MstRASowProduct> section = new List<MstRASowProduct>();
                //section = client.GetSOWToList("", "").ToList();

                // Modification Or Added By Ibnu Setiawan 20. February 2020 
                var sow = HttpRuntime.Cache.GetOrStore<List<MstRASowProduct>>($"User{CacheKeySOW}", () => client.GetSOWToList(UserManager.User.UserToken, "").ToList())
                        .Where(w => w.SectionProductId.ToString() == SectionID).ToList();

                return Ok(sow);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("AddDataDashboardTSEL")]
        public IHttpActionResult AddDataDashboardTSEL(PostDashboardTSEL post)
        {
            try
            {
                MstRATargetRecurring data = new MstRATargetRecurring();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                List<MstRATargetRecurring> _mstRATargetRecurring = new List<MstRATargetRecurring>();

                foreach (var item in post.vwRABapsSiteList)
                {
                    var mstbapsID = item.KeySetting.Split('_')[0].ToString();
                    var YearBillStr = item.KeySetting.Split('_')[1].ToString();
                    var startInvoiceDate = item.StartInvoiceDate;
                    var endInvoiceDate = item.EndInvoiceDate;
                    MstRATargetRecurring mstData = new MstRATargetRecurring();
                    mstData.mstBapsId = Int32.Parse(mstbapsID);
                    mstData.YearBill = Int32.Parse(YearBillStr);
                    mstData.Year = Int32.Parse(post.YearTarget);
                    mstData.Month = Int32.Parse(post.MonthBill);
                    mstData.BapsType = Int32.Parse(post.BapsType);
                    mstData.PowerType = post.PowerType;
                    mstData.CreatedDate = DateTime.Now;
                    mstData.CreatedBy = userCredential.UserID;
                    //new
                    mstData.StartInvoiceDate = startInvoiceDate;
                    mstData.EndInvoiceDate = endInvoiceDate;
                    mstData.DepartmentCode = post.DepartmentCode;
                    mstData.AmountIDR = item.AmountIDR;
                    mstData.SONumber = item.SONumber;

                    _mstRATargetRecurring.Add(mstData);
                }
                _mstRATargetRecurring = _mstRATargetRecurring.Distinct().ToList();
                data = client.AddDataDashboardTSEL(_mstRATargetRecurring).FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("AddDataTargetNewBaps")]
        public IHttpActionResult AddDataTargetNewBaps(PostDashboardTSEL post)
        {
            try
            {
                MstRATargetNewBaps data = new MstRATargetNewBaps();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                List<MstRATargetNewBaps> targetNewBaps = new List<MstRATargetNewBaps>();

                foreach (var item in post.vwRABapsSiteList)
                {
                    var mstbapsID = item.KeySetting.Split('_')[0].ToString();
                    var YearBillStr = item.KeySetting.Split('_')[1].ToString();
                    var startInvoiceDate = item.StartInvoiceDate;
                    var endInvoiceDate = item.EndInvoiceDate;
                    MstRATargetNewBaps mstData = new MstRATargetNewBaps();
                    mstData.SoNumber = item.SONumber;
                    mstData.YearBill = Int32.Parse(YearBillStr);
                    mstData.Year = Int32.Parse(post.YearTarget);
                    mstData.Month = Int32.Parse(post.MonthBill);
                    mstData.BapsType = Int32.Parse(post.BapsType);
                    mstData.PowerType = post.PowerType;
                    mstData.CreatedDate = DateTime.Now;
                    mstData.CreatedBy = userCredential.UserID;
                    //new
                    mstData.StartInvoiceDate = startInvoiceDate;
                    mstData.EndInvoiceDate = endInvoiceDate;
                    mstData.DepartmentCode = post.DepartmentCode;
                    mstData.AmountIDR = item.AmountIDR;
                    targetNewBaps.Add(mstData);
                }

                data = client.AddDataTargetNewBaps(targetNewBaps).FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("listhistory")]
        public IHttpActionResult GetHistoryTargetRecurring(PostDashboardTSEL post)
        {
            try
            {

                var data = new List<vwRABapsSite>();
                int intTotalRecord = 0;
                var param = new vwRABapsSite();
              
                param.TargetYear = post.strYearTargetHistory;
                param.TargetMonth = post.strMonthTargetHistory;
                param.ReconcileType = post.slReconType;
                param.PowerType = post.slPWRType;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.DepartmentType = post.DepartmentCode;

                param.CompanyInvoiceId = post.strCompanyInvoice;
                param.CustomerID = post.strCustomerInvoice;

                intTotalRecord = client.GetHistoryRecurringListCount(param, post.strSONumberMultiple);

                data = client.GetHistoryRecurringToList(param, post.strSONumberMultiple, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("GetListHistoryTargetIds")]
        public IHttpActionResult GetListHistoryTargetIds(PostDashboardTSEL post)
        {
            try
            {
                var data = new List<vwRABapsSite>();
                var param = new vwRABapsSite();

                param.TargetYear = post.strYearTargetHistory;
                param.TargetMonth = post.strMonthTargetHistory;
                param.ReconcileType = post.slReconType;
                param.PowerType = post.slPWRType;
                param.TargetBaps = post.BapsType;
                param.TargetPower = post.PowerType;
                param.DepartmentType = post.DepartmentCode;

                param.CompanyInvoiceId = post.strCompanyInvoice;
                param.CustomerID = post.strCustomerInvoice;

                data = client.GetHistoryRecurringToList(param, post.strSONumberMultiple, 0, 0);

                return Ok(data.Select(m => m.TargetID+ "_" + m.DepartmentCode).ToList());
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("GetListId")]
        public IHttpActionResult GetListId(PostDashboardTSEL post)
        {
            try
            {
                post.vwRABapsSite = MappingParam(post);
                var ListId = client.GetListId(post.vwRABapsSite).ToList();
                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        [HttpPost, Route("GetListData")]
        public IHttpActionResult GetListData(PostDashboardTSEL post)
        {
            try
            {
                post.vwRABapsSite = MappingParam(post);
                if (post.vwRABapsSiteList != null)
                {
                    string listid = post.vwRABapsSite.ListIdString;
                    post.vwRABapsSite.ListIdString = listid;
                }

                var ListData = client.GetTrxDashbordTSELDataToList(post.vwRABapsSite, 0, -1).ToList();
                return Ok(ListData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        private vwRABapsSite MappingParam(PostDashboardTSEL post)
        {
            var param = new vwRABapsSite();
            param.CompanyInvoiceId = post.strCompanyInvoice;
            param.CustomerID = post.strCustomerInvoice;
            param.SectionProductId = post.strSection;
            param.SowProductId = post.strSOW;
            param.YearBill = post.strBillingYear;
            param.RegionID = post.strRegional;
            param.ProvinceID = post.strProvince;
            param.ProductID = post.strTenantType;
            param.SONumber = post.schSONumber;
            param.SiteID = post.schSiteID;
            param.SiteName = post.schSiteName;
            param.CustomerSiteID = post.schCustomerSiteID;
            param.CustomerSiteName = post.schCustomerSiteName;
            param.RegionName = post.schRegionName;
            if (!string.IsNullOrWhiteSpace(post.schCustomerID))
                param.CustomerID = post.schCustomerID;
            if (post.schYearBill > 0)
                param.YearBill = post.schYearBill;
            param.TargetYear = post.strYearTargetHistory;
            param.TargetMonth = post.strMonthTargetHistory;
            param.TargetBaps = post.BapsType;
            param.TargetPower = post.PowerType;
            param.StartInvoiceDate = post.StartInvoiceDate;
            param.EndInvoiceDate = post.EndInvoiceDate;

            return param;
        }

        [HttpGet, Route("BapsTypeTSEL")]
        public IHttpActionResult GetBapsTypeToListTSEL()
        {
            try
            {
                List<mstBapsType> BapsTypeList = new List<mstBapsType>();
                BapsTypeList = client.GetBapsTypeToListTSEL("", "").ToList();

                return Ok(BapsTypeList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpGet, Route("BapsTypeInputTargetHistory")]
        public IHttpActionResult GetBapsTypeInputTargetHistory()
        {
            try
            {
                List<mstBapsType> BapsTypeList = new List<mstBapsType>();
                BapsTypeList = client.GetBapsTypeToList("", "", "mstBapsTypeId IN (3,5,2) AND ").ToList();

                return Ok(BapsTypeList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        #region Dashboard
        [HttpGet, Route("GetData")]
        public IHttpActionResult GetData(string YearBill, string MonthBill)
        {
            string strWhereClause = " CustomerID = 'TSEL' ";
            string[] monthlists = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string[] monthstr = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            int[] datamonth = new int[12];//All Data Perbulannya
            int[] towerdata = new int[12];//All Data Tower
            int[] dataAchievement = new int[12];//Data Achievement yang sesuai settingan target
            int[] dataOuterAchievement = new int[12];//Data Achievement yang diluar target
            List<HighchartData> versusdata = new List<HighchartData>();//Data Versus
            List<HighchartData> yearlydata = new List<HighchartData>();//Data Yearly

            int[] powerdata = new int[12];
            //YearBill = "2018";

            if (!string.IsNullOrEmpty(YearBill))
                strWhereClause += " AND YearBill = " + YearBill;

            strWhereClause += " ";

            //Get Data ALL Without Filter BapsType
            var dataresult = summary.GetDataRecurring(strWhereClause);
            int MonthTarget = 0;
            var alltarget = GetTarget(YearBill, "", "", Int32.Parse(MonthBill), MonthTarget); //Data Target All Tanpa Filter Type
            alltarget.type = "spline"; //Ubah type nya jadi line untuk Chart Yearly
            yearlydata.Add(alltarget); //Data Target All Untuk Chart Yearly

            MonthTarget = alltarget.data[(Int32.Parse(MonthBill) - 1)]; //Get Target di bulan yang di Filter


            //Get Data Tower
            //var datatower = summary.GetTowerDataRecurring(strWhereClause + " AND (BapsType = 'TOWER' OR (BapsType = 'POWER' AND PowerTypeCode = '0003')) ");
            var datatower = summary.GetTowerDataRecurring(strWhereClause + " AND SectionName = 'TOWER' ");
            var SectionTower = GetTowerData(datatower);
            //var targettower = GetTarget(YearBill, "5");
            //var datapower = summary.GetDataRecurring(strWhereClause + " AND PowerType = 'OVER BLAST' ");

            //var AchivementPower = summary.GetPowerRecurring(strWhereClause + " AND BapsType = 'POWER' AND PowerTypeCode != '0003' ");
            var AchivementPower = summary.GetPowerRecurring(strWhereClause + " AND SectionName = 'NON-TOWER' ");
            var NonTowerData = GetPowerData(AchivementPower);

            //Get Data Versus
            var dtversus = summary.GetAchievementTarget(strWhereClause);

            for (int i = 0; i < monthstr.Length; i++)
            {
                if (dataresult.Where(w => w.MonthBillName == monthstr[i]).FirstOrDefault() != null)
                    datamonth[i] = dataresult.Where(w => w.MonthBillName == monthstr[i]).FirstOrDefault().Qty;
                else
                    datamonth[i] = 0;

                //if (datatower.Where(w => w.MonthBillName == monthstr[i]).FirstOrDefault() != null)
                //    towerdata[i] = datatower.Where(w => w.MonthBillName == monthstr[i]).FirstOrDefault().Qty;
                //else
                //    towerdata[i] = 0;

                //if (datapower.Where(w => w.MonthBillName == monthstr[i]).FirstOrDefault() != null)
                //    powerdata[i] = datapower.Where(w => w.MonthBillName == monthstr[i]).FirstOrDefault().Qty;
                //else
                //    powerdata[i] = 0;

                if (dtversus.Where(w => w.MonthBillName == monthstr[i] && w.Targets == 1).FirstOrDefault() != null)
                    dataAchievement[i] = dtversus.Where(w => w.MonthBillName == monthstr[i] && w.Targets == 1).FirstOrDefault().Qty;
                else
                    dataAchievement[i] = 0;

                if (dtversus.Where(w => w.MonthBillName == monthstr[i] && w.Targets == 0).FirstOrDefault() != null)
                    dataOuterAchievement[i] = dtversus.Where(w => w.MonthBillName == monthstr[i] && w.Targets == 0).FirstOrDefault().Qty;
                else
                    dataOuterAchievement[i] = 0;

            }

            versusdata.Add(new HighchartData
            {
                name = "On Target",
                color = "blue",
                data = dataAchievement,
                type = "column"
            });
            versusdata.Add(new HighchartData
            {
                name = "Out Target",
                color = "green",
                data = dataOuterAchievement,
                type = "column"
            });
            versusdata.Add(new HighchartData
            {
                name = "Target",
                color = "red",
                data = alltarget.data,
                type = "column"
            });

            yearlydata.Add(new HighchartData
            {
                name = "Achievement",
                color = "blue",
                data = datamonth,
                type = "column"
            });
            //var powerdata = GetPowerData(datapower);

            int AchievementMonth = 0;

            if (!string.IsNullOrEmpty(MonthBill) && dataresult.Where(w => w.MonthBill == Int32.Parse(MonthBill)).FirstOrDefault() != null)
                AchievementMonth = dataresult.Where(w => w.MonthBill == Int32.Parse(MonthBill)).FirstOrDefault().Qty;

            return Ok(new
            {
                MonthList = monthlists,
                AchievementMonth = AchievementMonth,
                TargetMonth = MonthTarget
                ,
                TowerData = SectionTower,
                VersusData = versusdata,
                YearlyData = yearlydata,
                PowerData = powerdata,
                NonTowerData = NonTowerData
            });
        }

        private List<HighchartData> GetPowerData(List<vwGetInvoiceRecurring> data)
        {
            int[] dataarray;
            string[] colorstring = new string[] { "#03d3fc", "#fc6b03", "#999591", "#c2a906", "#065ec2" };
            int ctr = 0;
            List<HighchartData> result = new List<HighchartData>();

            foreach (var row in data.Select(p => p.PowerType)
                                  .Distinct())
            {
                dataarray = new int[12];
                for (int i = 0; i < 12; i++)
                {
                    if (data.Where(w => w.MonthBill == (i + 1) && w.PowerType == row.ToString()).FirstOrDefault() != null)
                        dataarray[i] = data.Where(w => w.MonthBill == (i + 1) && w.PowerType == row.ToString()).FirstOrDefault().Qty;
                    else
                        dataarray[i] = 0;

                }

                result.Add(new HighchartData
                {
                    type = "column",
                    name = row.ToString(),
                    data = dataarray,
                    color = colorstring[ctr]
                });

                ctr = ctr + 1;
            }

            return result;
        }

        private List<HighchartData> GetTowerData(List<vwGetInvoiceRecurring> data)
        {
            int[] dataarray;
            string[] colorstring = new string[] { "#03d3fc", "#fc6b03", "#999591", "#c2a906", "#065ec2" };
            int ctr = 0;
            List<HighchartData> result = new List<HighchartData>();

            foreach (var row in data.Select(p => p.BapsType)
                                  .Distinct())
            {
                dataarray = new int[12];
                for (int i = 0; i < 12; i++)
                {
                    if (data.Where(w => w.MonthBill == (i + 1) && w.BapsType == row.ToString()).FirstOrDefault() != null)
                        dataarray[i] = data.Where(w => w.MonthBill == (i + 1) && w.BapsType == row.ToString()).FirstOrDefault().Qty;
                    else
                        dataarray[i] = 0;

                }

                result.Add(new HighchartData
                {
                    type = "column",
                    name = row.ToString().ToUpper() == "POWER" ? "OVB" : row.ToString(),
                    data = dataarray
                });

                ctr = ctr + 1;
            }

            return result;
        }

        private HighchartData GetTarget(string Year, string BapsType = "", string PowerType = "", int Month = 0, int TargetMonth = 0)
        {
            HighchartData dttarget = new HighchartData();

            //Year = "2018";
            string strWhereClause = " 1=1 ";

            if (!string.IsNullOrEmpty(Year))
                strWhereClause += " AND [Year] = " + Year;

            if (!string.IsNullOrEmpty(BapsType))
                strWhereClause += " AND [BapsType] = " + BapsType;

            if (!string.IsNullOrEmpty(PowerType))
                strWhereClause += " AND [PowerType] = " + PowerType;

            List<MstRATargetRecurring> tr = new List<MstRATargetRecurring>();

            if (string.IsNullOrEmpty(BapsType))
            {
                tr = summary.GetAllTarget(strWhereClause);
                if (tr.Where(w => w.Month == Month).FirstOrDefault() != null)
                    TargetMonth = tr.Where(w => w.Month == Month).FirstOrDefault().Qty;
                else
                    TargetMonth = 0;
            }
            else
                tr = summary.GetTarget(strWhereClause);


            var dtarray = new int[12];
            for (int i = 0; i < 12; i++)
            {
                if (tr.Where(w => w.Month == (i + 1)).FirstOrDefault() != null)
                    dtarray[i] = tr.Where(w => w.Month == (i + 1)).FirstOrDefault().Qty;
                else
                    dtarray[i] = 0;
            }

            dttarget.color = "red";
            dttarget.data = dtarray;
            dttarget.name = "Target";
            dttarget.type = "column";

            return dttarget;
        }

        [HttpPost, Route("GetDetailSite")]
        public IHttpActionResult GetDetailSite(PostDashboardTSEL post)
        {
            try
            {
                //post.YearBill = "2018";
                string strWhereClause = " 1=1 ";

                switch (post.MonthBillName)
                {
                    case "Jan":
                        post.MonthBillName = "January";
                        break;
                    case "Feb":
                        post.MonthBillName = "February";
                        break;
                    case "Mar":
                        post.MonthBillName = "March";
                        break;
                    case "Apr":
                        post.MonthBillName = "April";
                        break;
                    case "May":
                        post.MonthBillName = "May";
                        break;
                    case "Jun":
                        post.MonthBillName = "June";
                        break;
                    case "Jul":
                        post.MonthBillName = "July";
                        break;
                    case "Aug":
                        post.MonthBillName = "August";
                        break;
                    case "Sep":
                        post.MonthBillName = "September";
                        break;
                    case "Oct":
                        post.MonthBillName = "October";
                        break;
                    case "Nov":
                        post.MonthBillName = "November";
                        break;
                    case "Dec":
                        post.MonthBillName = "December";
                        break;
                    default:
                        post.MonthBillName = post.MonthBillName;
                        break;
                }


                if (!string.IsNullOrEmpty(post.YearBill))
                    strWhereClause += " AND YearBill = " + post.YearBill;

                if (!string.IsNullOrEmpty(post.MonthBill))
                    strWhereClause += " AND MonthBill = " + post.MonthBill;

                if (!string.IsNullOrEmpty(post.BapsType))
                    strWhereClause += " AND mstBapsTypeId = " + post.BapsType;

                if (!string.IsNullOrEmpty(post.MonthBillName))
                    strWhereClause += " AND MonthBillName = '" + post.MonthBillName + "' ";

                if (!string.IsNullOrEmpty(post.PowerType))
                    strWhereClause += " AND ( PowerTypeCode = '" + post.PowerType + "' OR PowerType = '" + post.PowerType + "' ) ";

                if (!string.IsNullOrEmpty(post.Targets))
                    strWhereClause += " AND Targets = " + post.Targets;

                if (!string.IsNullOrEmpty(post.SecName))
                    strWhereClause += " AND SectionName = '" + post.SecName + "' ";

                if (!string.IsNullOrEmpty(post.SOWName))
                    strWhereClause += " AND SowName = '" + post.SOWName + "' ";

                var CountOfRows = summary.GetCountDetailSite(strWhereClause);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                var data = summary.GetPageDetailSite(strWhereClause, post.start, post.length, strOrderBy);

                return Ok(new { draw = post.draw, recordsTotal = CountOfRows, recordsFiltered = CountOfRows, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex.InnerException);
            }
        }

        [HttpPost, Route("GetDetailTargetSite")]
        public IHttpActionResult GetDetailTargetSite(PostDashboardTSEL post)
        {
            try
            {
                //post.YearBill = "2018";
                string strWhereClause = " 1=1 ";

                switch (post.MonthBillName)
                {
                    case "Jan":
                        post.MonthBillName = "January";
                        break;
                    case "Feb":
                        post.MonthBillName = "February";
                        break;
                    case "Mar":
                        post.MonthBillName = "March";
                        break;
                    case "Apr":
                        post.MonthBillName = "April";
                        break;
                    case "May":
                        post.MonthBillName = "May";
                        break;
                    case "Jun":
                        post.MonthBillName = "June";
                        break;
                    case "Jul":
                        post.MonthBillName = "July";
                        break;
                    case "Aug":
                        post.MonthBillName = "August";
                        break;
                    case "Sep":
                        post.MonthBillName = "September";
                        break;
                    case "Oct":
                        post.MonthBillName = "October";
                        break;
                    case "Nov":
                        post.MonthBillName = "November";
                        break;
                    case "Dec":
                        post.MonthBillName = "December";
                        break;
                    default:
                        post.MonthBillName = post.MonthBillName;
                        break;
                }

                if (!string.IsNullOrEmpty(post.YearBill))
                    strWhereClause += " AND YearTarget = " + post.YearBill;

                if (!string.IsNullOrEmpty(post.MonthBill))
                    strWhereClause += " AND MonthBill = " + post.MonthBill;

                if (!string.IsNullOrEmpty(post.BapsType))
                    strWhereClause += " AND mstBapsTypeId = " + post.BapsType;

                if (!string.IsNullOrEmpty(post.MonthBillName))
                    strWhereClause += " AND MonthBillName = '" + post.MonthBillName + "' ";

                if (!string.IsNullOrEmpty(post.PowerType))
                    strWhereClause += " AND PowerTypeCode = '" + post.PowerType + "' ";

                var CountOfRows = summary.GetCountDetailTargetSite(strWhereClause);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                var data = summary.GetPageDetailTargetSite(strWhereClause, post.start, post.length, strOrderBy);

                return Ok(new { draw = post.draw, recordsTotal = CountOfRows, recordsFiltered = CountOfRows, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex.InnerException);
            }
        }

        [HttpGet, Route("GetDepartmentInputTarget")]
        public IHttpActionResult GetDepartmentInputTarget()
        {
            try
            {
                MstDepartmentService department = new MstDepartmentService();
                var deparments = department.GetDepartmentListInputTarget();
                return Ok(deparments);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        #endregion



    }
}