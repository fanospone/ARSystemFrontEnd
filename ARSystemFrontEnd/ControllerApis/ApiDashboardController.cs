using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

using ARSystemFrontEnd.Helper;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Service.ARSystem;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("Api/Dashboard")]
    public class ApiDashboardController : ApiController
    {
        private MonitoringAgingExecutiveService _agingExecutiveService;
        private readonly MonitoringCNInvoiceService _monitoringCNService;

        public ApiDashboardController()
        {
            _agingExecutiveService = new MonitoringAgingExecutiveService();
            _monitoringCNService = new MonitoringCNInvoiceService();
        }

        #region KPI Dashboard

        #region Lead Time

        // GET: ApiDashboard
        [HttpPost, Route("LeadTimePIC")]
        public IHttpActionResult LeadTimePIC(PostDashboardView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmDashboardYear> years = new List<ARSystemService.vmDashboardYear>();

                List<ARSystemService.vmDashboardLeadTimePIC> data = new List<ARSystemService.vmDashboardLeadTimePIC>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    years = client.GetYear(UserManager.User.UserToken).ToList();
                }

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    if (post.Year == 0)
                    {
                        if (years.Count > 0)
                            post.Year = int.Parse(years[0].ValueDesc);
                        else
                            post.Year = DateTime.Now.Year;
                    }

                    if (post.Month == 0) {
                        data = client.GetLeadTimePICPerMonth(UserManager.User.UserToken, post.Year).ToList();
                    }
                    else
                        data = client.GetLeadTimePICPerWeek(UserManager.User.UserToken, post.Year, post.Month).ToList();

                    intTotalRecord = data.Count();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #region Graph
        [HttpPost, Route("GraphLeadTime")]
        public IHttpActionResult GraphLeadTime(PostDashboardView post)
        {
            try
            {
                List<ARSystemService.vmDashboardYear> years = new List<ARSystemService.vmDashboardYear>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    years = client.GetYear(UserManager.User.UserToken).ToList();
                }

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    if (post.Month == 0)
                    {
                        if (post.Year == 0)
                        {
                            if (years.Count > 0)
                                post.Year = int.Parse(years[0].ValueDesc);
                            else
                                post.Year = DateTime.Now.Year;
                        }
                        ARSystemService.vmDashboardGraphLeadTimePerMonth data = client.GetGraphLeadTimePICPerMonth(UserManager.User.UserToken, post.Year);
                        return Ok(data);
                    }
                    else
                    {
                        ARSystemService.vmDashboardGraphLeadTimePerWeek data = client.GetGraphLeadTimePICPerWeek(UserManager.User.UserToken, post.Year, post.Month);
                        return Ok(data);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        #endregion

        #region Invoice Summary

        #region Tower

        [HttpPost, Route("InvoiceTowerSummary")]
        public IHttpActionResult InvoiceTowerSummary(PostDashboardView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmDashboardYear> years = new List<ARSystemService.vmDashboardYear>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    years = client.GetYear(UserManager.User.UserToken).ToList();
                }

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    if (post.Year == 0)
                    {
                        if (years.Count > 0)
                            post.Year = int.Parse(years[0].ValueDesc);
                        else
                            post.Year = DateTime.Now.Year;
                    }

                    if (post.Month == 0)
                    {
                        List<ARSystemService.vmDashboardInvoiceTowerSummaryPerMonth> data = client.GetInvoiceTowerSummaryPerMonth(UserManager.User.UserToken, post.Year, post.Currency).ToList();
                        intTotalRecord = data.Count();
                        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                    }
                    else
                    {
                        List<ARSystemService.vmDashboardInvoiceTowerSummaryPerWeek> data = client.GetInvoiceTowerSummaryPerWeek(UserManager.User.UserToken, post.Year, post.Month, post.Currency).ToList();
                        intTotalRecord = data.Count();
                        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion

        #region Building

        [HttpPost, Route("InvoiceBuildingSummary")]
        public IHttpActionResult InvoiceBuildingSummary(PostDashboardView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmDashboardYear> years = new List<ARSystemService.vmDashboardYear>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    years = client.GetYear(UserManager.User.UserToken).ToList();
                }

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    if (post.Year == 0)
                    {
                        if (years.Count > 0)
                            post.Year = int.Parse(years[0].ValueDesc);
                        else
                            post.Year = DateTime.Now.Year;
                    }

                    if (post.Month == 0)
                    {
                        List<ARSystemService.vmDashboardInvoiceBuildingSummaryPerMonth> data = client.GetInvoiceBuildingSummaryPerMonth(UserManager.User.UserToken, post.Year).ToList();
                        intTotalRecord = data.Count();
                        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                    }
                    else
                    {
                        List<ARSystemService.vmDashboardInvoiceBuildingSummaryPerWeek> data = client.GetInvoiceBuildingSummaryPerWeek(UserManager.User.UserToken, post.Year, post.Month).ToList();
                        intTotalRecord = data.Count();
                        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion

        #endregion

        #region KPI Summary

        [HttpPost, Route("KPISummary")]
        public IHttpActionResult KPISummary(PostDashboardView post)
        {
            try
            {
                int intTotalRecord = 0;
                List<ARSystemService.vmDashboardYear> years = new List<ARSystemService.vmDashboardYear>();

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    if (post.Year == 0)
                    {
                        if (years.Count > 0)
                            post.Year = int.Parse(years[0].ValueDesc);
                        else
                            post.Year = DateTime.Now.Year;
                    }

                    if (post.Month == 0)
                    {
                        List<ARSystemService.vmDashboardKPISummaryPerMonth> data = client.GetKPISummaryPerMonth(UserManager.User.UserToken, post.Year).ToList();
                        intTotalRecord = data.Count();
                        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                    }
                    else
                    {
                        List<ARSystemService.vmDashboardKPISummaryPerWeek> data = client.GetKPISummaryPerWeek(UserManager.User.UserToken, post.Year, post.Month).ToList();
                        intTotalRecord = data.Count();
                        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion

        #region Detail

        #region Lead Time

        [Route("Detail/LeadTime")]
        public IHttpActionResult DetailLeadTime(PostDashboardDetailLeadTimeView post)
        {
            try
            {
                int intTotalRecord = 0;

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    List<ARSystemService.vmDashboardInvoiceTowerDetailLeadTime> data = client.GetDetailLeadTime(UserManager.User.UserToken, post.Year, post.Month, post.Week, post.PIC, post.LeadTime).ToList();
                    intTotalRecord = data.Count();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion

        #region SO Number

        [Route("Detail/SoNumber")]
        public IHttpActionResult DetailSoNumber(PostDashboardDetailSoNumberView post)
        {
            try
            {
                int intTotalRecord = 0;

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    List<ARSystemService.vmDashboardInvoiceTowerDetailSoNumber> data = client.GetDetailSONumber(UserManager.User.UserToken, post.Year, post.Month, post.Week, post.LeadTime, post.Currency).ToList();
                    intTotalRecord = data.Count();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion

        #region Building

        [HttpPost, Route("Detail/Building")]
        public IHttpActionResult GetInvoiceBuildingDetail(PostDashboardBuildingDetail post)
        {
            try
            {
                int intTotalRecord = 0;

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    List<ARSystemService.vmDashboardInvoiceBuildingDetail> data = client.GetInvoiceBuildingDetail(UserManager.User.UserToken, post.Year, post.Month, post.Week, post.LeadTime).ToList();
                    intTotalRecord = data.Count();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion

        #endregion

        #endregion

        #region AR Ratio Dashboard

        [HttpPost, Route("ARRatio")]
        public IHttpActionResult ARRatio(PostDashboardView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmDashboardYear> years = new List<ARSystemService.vmDashboardYear>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    years = client.GetYear(UserManager.User.UserToken).ToList();
                }

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    if (post.Year == 0)
                    {
                        if (years.Count > 0)
                            post.Year = int.Parse(years[0].ValueDesc);
                        else
                            post.Year = DateTime.Now.Year;
                    }

                    if (post.Month == 0)
                    {
                        List<ARSystemService.vmDashboardARRatioPerMonth> data = client.GetARRatioPerMonth(UserManager.User.UserToken, post.Year).ToList();
                        intTotalRecord = data.Count();
                        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                    }
                    else
                    {
                        List<ARSystemService.vmDashboardARRatioPerWeek> data = client.GetARRatioPerWeek(UserManager.User.UserToken, post.Year, post.Month).ToList();
                        intTotalRecord = data.Count();
                        return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #region Graph

        [HttpPost, Route("GraphARRatio")]
        public IHttpActionResult GraphARRatio(PostDashboardView post)
        {
            try
            {
                List<ARSystemService.vmDashboardYear> years = new List<ARSystemService.vmDashboardYear>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    years = client.GetYear(UserManager.User.UserToken).ToList();
                }

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    if (post.Year == 0)
                    {
                        if (years.Count > 0)
                            post.Year = int.Parse(years[0].ValueDesc);
                        else
                            post.Year = DateTime.Now.Year;
                    }

                    if (post.Month == 0)
                    {
                        ARSystemService.vmDashboardGraphARRatioPerMonth data = client.GetGraphARRatioPerMonth(UserManager.User.UserToken, post.Year);
                        return Ok(data);
                    }
                    else
                    {
                        ARSystemService.vmDashboardGraphARRatioPerWeek data = client.GetGraphARRatioPerWeek(UserManager.User.UserToken, post.Year, post.Month);
                        return Ok(data);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion

        #endregion

        #region AR Monitoring Aging

        #region Summary
        [HttpPost, Route("ARMonitoringAging/Summary")]
        public IHttpActionResult ARMonitoringAgingSummary(PostDashboardARMonitoringAgingView post)
        {
            try
            {
                int intTotalRecord = 0;

                if (post.CompanyId == null)
                    post.CompanyId = "All";
                if (post.OperatorId == null)
                    post.OperatorId = "All";
                if (post.InvoiceType == null)
                    post.InvoiceType = "All";
                if (post.AmountType == null)
                    post.AmountType = "Gross";

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    List<ARSystemService.vmDashboardARMonitoringAgingSummary> data = client.GetARMonitoringAgingSummary(UserManager.User.UserToken,post.CompanyId,post.EndDate,post.OperatorId,post.InvoiceType,post.AmountType,post.vPKP).ToList();
                    intTotalRecord = data.Count();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        #region Detail
        [HttpPost, Route("ARMonitoringAging/Detail")]
        public IHttpActionResult ARMonitoringAgingDetail(PostDashboardARMonitoringAgingView post)
        {
            try
            {
                int intTotalRecord = 0;
                

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    post.EndDate = post.EndDate.Replace("-","");
                    List<ARSystemService.vmDashboardARMonitoringAgingDetail> data = client.GetARMonitoringAgingDetail(UserManager.User.UserToken, post.CompanyId, post.EndDate, post.OperatorId, post.InvoiceType, post.AmountType, post.Status, post.vPKP).ToList();
                    intTotalRecord = data.Count();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion

        #endregion

        #region Dashboard Parameters

        [HttpPost, Route("GetYear")]
        public IHttpActionResult GetYear()
        {
            try
            {
                List<ARSystemService.vmDashboardYear> data = new List<ARSystemService.vmDashboardYear>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    data = client.GetYear(UserManager.User.UserToken).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetMonth")]
        public IHttpActionResult GetMonth()
        {
            try
            {
                List<ARSystemService.vmDashboardMonth> data = new List<ARSystemService.vmDashboardMonth>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    data = client.GetMonth(UserManager.User.UserToken).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetCompany")]
        public IHttpActionResult GetCompany()
        {
            try
            {
                List<ARSystemService.vmDashboardCompany> data = new List<ARSystemService.vmDashboardCompany>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    data = client.GetCompany(UserManager.User.UserToken).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetOperator")]
        public IHttpActionResult GetOperator()
        {
            try
            {
                List<ARSystemService.vmDashboardOperator> data = new List<ARSystemService.vmDashboardOperator>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    data = client.GetOperator(UserManager.User.UserToken).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetInvoiceType")]
        public IHttpActionResult GetInvoiceType()
        {
            try
            {
                List<ARSystemService.vmDashboardInvoiceType> data = new List<ARSystemService.vmDashboardInvoiceType>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    data = client.GetInvoiceType(UserManager.User.UserToken).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetAmountType")]
        public IHttpActionResult GetAmountType()
        {
            try
            {
                List<ARSystemService.vmDashboardAmountType> data = new List<ARSystemService.vmDashboardAmountType>();
                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    data = client.GetAmountType(UserManager.User.UserToken).ToList();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        #endregion

        #region Reconcile History
        [HttpPost, Route("ReconcileHistory")]
        public IHttpActionResult ReconcileHistory(PostTrxBapsDataView post)
        {
            try
            {
                int intTotalRecord = 0;
                string strWhereClause = GetWhereClause(post);

                using (var client = new ARSystemService.DashboardServiceClient())
                {
                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    List<ARSystemService.vwReconcileHistory> data = client.ReconcileHistory(UserManager.User.UserToken, strWhereClause,strOrderBy).ToList();
                    intTotalRecord = data.Count();
                    return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private string GetWhereClause(PostTrxBapsDataView post)
        {
            string strWhereClause = "";
            if (!string.IsNullOrWhiteSpace(post.strCompanyId))
            {
                strWhereClause += "CompanyInvoice = '" + post.strCompanyId + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strOperator))
            {
                strWhereClause += "CustomerInvoice = '" + post.strOperator + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strStatusBAPS))
            {
                strWhereClause += "Status = '" + post.strStatusBAPS + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPeriodInvoice))
            {
                strWhereClause += "PeriodInvoice = '" + post.strPeriodInvoice + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strInvoiceType))
            {
                strWhereClause += "InvoiceType = '" + post.strInvoiceType + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCurrency))
            {
                strWhereClause += "Currency = '" + post.strCurrency + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPONumber))
            {
                strWhereClause += "PONumber LIKE '%" + post.strPONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strBAPSNumber))
            {
                strWhereClause += "BAPSNumber LIKE '%" + post.strBAPSNumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSONumber))
            {
                strWhereClause += "SoNumber LIKE '%" + post.strSONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strBapsType))
            {
                strWhereClause += "TowerTypeID LIKE '%" + post.strBapsType + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteIdOld))
            {
                strWhereClause += "SiteID LIKE '%" + post.strSiteIdOld + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strStartPeriod))
            {
                strWhereClause += "CONVERT(VARCHAR, StartInvoiceDate, 106) LIKE '%" + post.strStartPeriod.Replace('-', ' ') + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strEndPeriod))
            {
                strWhereClause += "CONVERT(VARCHAR, EndInvoiceDate, 106) LIKE '%" + post.strEndPeriod.Replace('-', ' ') + "%' AND ";
                //strWhereClause += "EndDateInvoice <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCreatedBy))
            {
                strWhereClause += "CreatedBy LIKE '%" + post.strCreatedBy + "%' AND ";
            }
            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

            return strWhereClause;
        }
        #endregion

        #region Monitoring Aging Executive
        [HttpPost, Route("MonitoringAgingExecutive/summary")]
        public IHttpActionResult MonitoringAgingExecutiveSummary(vmMonitoringAgingExecutive param)
        {
            try
            {

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmMonitoringAgingExecutiveSummary data = new vmMonitoringAgingExecutiveSummary(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    int intTotalRecord = 0;

                    List<vmMonitoringAgingExecutiveSummary> result = _agingExecutiveService.GetSummary(userCredential.UserID, param);
                    intTotalRecord = result.Count();

                    return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        #region Monitoring CN Invoice
        [HttpGet, Route("Operator")]
        public IHttpActionResult GetOperatorToList()
        {
            try
            {
                List<ARSystemService.mstOperator> Operator = new List<ARSystemService.mstOperator>();
                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    Operator = client.GetOperatorToList(UserManager.User.UserToken, "Operator ASC").ToList();
                }

                return Ok(Operator);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("MonitoringCNInvoiceList")]
        public IHttpActionResult MonitoringCNInvoiceList(PostDashboardMonitoringCNInvoiceView post)
        {
            try
            {
                var token = UserManager.User.UserToken;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(token);
                string strWhereClause = $"1=1";
                if (!string.IsNullOrEmpty(post.CustomerID))
                {
                    strWhereClause += $" AND CustomerID = '{post.CustomerID}'";
                }
                if (!string.IsNullOrEmpty(post.CompanyID))
                {
                    strWhereClause += $" AND CompanyID = '{post.CompanyID}'";
                }
                if (!string.IsNullOrEmpty(post.vPKP))
                {
                    strWhereClause += $" AND CompanyID != 'pkp'";
                }

                int intTotalRecord = 0;
                //intTotalRecord = _monitoringCNService.GetMonitoringCNInvoiceListCount(token, userCredential, strWhereClause);
                intTotalRecord = _monitoringCNService.GetMonitoringCNInvoiceListCount(token, userCredential, /*post.CustomerID, post.CompanyID, */strWhereClause);
                string strOrderBy = "";
                if (post.order != null)
                    strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                var dataList = _monitoringCNService.GetMonitoringCNInvoiceList(token, userCredential, /*post.CustomerID, post.CompanyID,*/ strWhereClause, strOrderBy, post.start, post.length);
                //var dataList = _monitoringCNService.GetMonitoringCNInvoiceList(token, userCredential, strWhereClause, strOrderBy, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("MonitoringCNInvoiceListDetail")]
        public IHttpActionResult MonitoringCNInvoiceListDetail(PostDashboardMonitoringCNInvoiceDetailView post)
        {
            try
            {
                var token = UserManager.User.UserToken;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(token);
                string strWhereClause = $"1=1";
                if (!string.IsNullOrEmpty(post.CustomerID))
                {
                    strWhereClause += $" AND CustomerID = '{post.CustomerID}'";
                }
                if (!string.IsNullOrEmpty(post.CompanyID))
                {
                    strWhereClause += $" AND CompanyID = '{post.CompanyID}'";
                }
                if (!string.IsNullOrEmpty(post.vPKP))
                {
                    strWhereClause += $" AND CompanyID != 'pkp'";
                }
                if (!string.IsNullOrEmpty(post.Range))
                {
                    if (post.Range == "13") strWhereClause += $" AND OD_13 = '1'";
                    if (post.Range == "46") strWhereClause += $" AND OD_46 = '1'";
                    if (post.Range == "79") strWhereClause += $" AND OD_79 = '1'";
                    if (post.Range == "9s") strWhereClause += $" AND OD_9s = '1'";
                }

                int intTotalRecord = 0;
                intTotalRecord = _monitoringCNService.GetMonitoringCNInvoiceListDetailCount(token, userCredential, strWhereClause);

                string strOrderBy = "InvNumber asc";
                if (post.order != null)
                    strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                var dataList = _monitoringCNService.GetMonitoringCNInvoiceListDetail(token, userCredential, strWhereClause, strOrderBy, post.start, post.length);

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dataList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

    }
}