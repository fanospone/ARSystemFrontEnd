using System;
using System.Collections.Generic;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;
using System.Threading.Tasks;

namespace ARSystem.Service
{
    public class RADashboardMonitoringBAPSDoneService
    {
        #region MonitoringBapsDone
        public async Task<List<vwMonitoringBapsDoneHeader>> GetMonitoringBapsDoneHeaderList(string UserID, MonitoringBapsDoneHeaderParam param, int RowSkip, int PageSize = 10)
        {
            List<vwMonitoringBapsDoneHeader> list = new List<vwMonitoringBapsDoneHeader>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwMonitoringBapsDoneHeaderRepository(context);

            try
            {

                list = await repo.GetList(param, RowSkip, PageSize);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwMonitoringBapsDoneHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardService", "GetBapsReportHeaderList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }

        }

        public async Task<int> GetMonitoringBapsDoneHeaderCount(string UserID, MonitoringBapsDoneHeaderParam param)
        {
            List<vwMonitoringBapsDoneHeader> list = new List<vwMonitoringBapsDoneHeader>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwMonitoringBapsDoneHeaderRepository(context);

            try
            {

                return repo.GetCount(param);
            }
            catch (Exception ex)
            {
                list.Add(new vwMonitoringBapsDoneHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardService", "GetBapsReportHeaderCount", UserID)));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public async Task<List<vwMonitoringBapsDoneDetail>> GetMonitoringBAPSDoneDetailList(string UserID, vwMonitoringBapsDoneDetail model, string GroupBy, string BapsType, int rowSkip, int pageSize)
        {
            List<vwMonitoringBapsDoneDetail> list = new List<vwMonitoringBapsDoneDetail>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwMonitoringBapsDoneDetailRepository(context);

            try
            {

                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(model.SoNumber))
                    strWhereClause += "SoNumber LIKE '%" + model.SoNumber.TrimStart().TrimEnd() + "%' AND ";

                if (model.ProductID != null && model.ProductID != 0)
                    strWhereClause += "ProductID = '" + model.ProductID + "' AND ";


                if (model.STIPID != null && model.STIPID != 0)
                    strWhereClause += "STIPID = '" + model.STIPID + "' AND ";

                if (model.Year != null && model.Year != 0)
                    strWhereClause += "Year = '" + model.Year + "' AND ";

                if (model.Month != null && model.Month != 0)
                    strWhereClause += "Month = '" + model.Month + "' AND ";

                if (!string.IsNullOrWhiteSpace(model.CustomerId))
                    strWhereClause += "CustomerID = '" + model.CustomerId.TrimStart().TrimEnd() + "' AND ";

                if (!string.IsNullOrWhiteSpace(model.CompanyInvoiceId))
                    strWhereClause += "CompanyInvoiceId = '" + model.CompanyInvoiceId.TrimStart().TrimEnd() + "' AND ";

                if (model.RegionID != null && model.RegionID != 0)
                    strWhereClause += "RegionID = '" + model.RegionID + "' AND ";

                if (model.ProvinceID != null && model.ProvinceID != 0)
                    strWhereClause += "ProvinceID = '" + model.ProvinceID + "' AND ";

                if (BapsType.ToUpper() == "NEW")
                {
                    strWhereClause += "StipSiro  = 0 AND ";
                }
                else if (BapsType.ToUpper() == "SIRO")
                {
                    strWhereClause += "StipSiro  > 0 AND ";
                }
                else if (BapsType.ToUpper() == "Power")
                {
                    strWhereClause += "BapsType = 'Power' AND ";
                    if (model.PowerTypeID != null && model.PowerTypeID != 0)
                        strWhereClause += "PowerTypeID = '" + model.PowerTypeID + "' AND ";
                }
                //Added by ASE
                if (!string.IsNullOrWhiteSpace(model.SiteID))
                    strWhereClause += "SiteID LIKE '%" + model.SiteID.TrimStart().TrimEnd() + "%' AND ";
                if (!string.IsNullOrWhiteSpace(model.SiteName))
                    strWhereClause += "SiteName LIKE '%" + model.SiteName.TrimStart().TrimEnd() + "%' AND ";
                if (!string.IsNullOrWhiteSpace(model.CustomerSiteID))
                    strWhereClause += "CustomerSiteID LIKE '%" + model.CustomerSiteID.TrimStart().TrimEnd() + "%' AND ";
                if (!string.IsNullOrWhiteSpace(model.CustomerSiteName))
                    strWhereClause += "CustomerSiteName LIKE '%" + model.CustomerSiteName.TrimStart().TrimEnd() + "%' AND ";
                //End

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                list = repo.GetPaged(strWhereClause, "SoNumber", rowSkip, pageSize);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwMonitoringBapsDoneDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardService", "GetMonitoringBAPSDoneList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }

        }

        public async Task<int> GetMonitoringBAPSDoneDetailCount(string UserID, vwMonitoringBapsDoneDetail model, string GroupBy, string BapsType)
        {
            List<vwMonitoringBapsDoneDetail> list = new List<vwMonitoringBapsDoneDetail>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwMonitoringBapsDoneDetailRepository(context);

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(model.SoNumber))
                    strWhereClause += "SoNumber LIKE '%" + model.SoNumber.TrimStart().TrimEnd() + "%' AND ";

                if (model.ProductID != null && model.ProductID != 0)
                    strWhereClause += "ProductID = '" + model.ProductID + "' AND ";

                if (model.STIPID != null && model.STIPID != 0)
                    strWhereClause += "STIPID = '" + model.STIPID + "' AND ";

                if (model.Year != null && model.Year != 0)
                    strWhereClause += "Year = '" + model.Year + "' AND ";

                if (model.Month != null && model.Month != 0)
                    strWhereClause += "Month = '" + model.Month + "' AND ";

                if (!string.IsNullOrWhiteSpace(model.CustomerId))
                    strWhereClause += "CustomerID = '" + model.CustomerId.TrimStart().TrimEnd() + "' AND ";

                if (!string.IsNullOrWhiteSpace(model.CompanyInvoiceId))
                    strWhereClause += "CompanyInvoiceId = '" + model.CompanyInvoiceId.TrimStart().TrimEnd() + "' AND ";

                if (model.RegionID != null && model.RegionID != 0)
                    strWhereClause += "RegionID = '" + model.RegionID + "' AND ";

                if (model.ProvinceID != null && model.ProvinceID != 0)
                    strWhereClause += "ProvinceID = '" + model.ProvinceID + "' AND ";

                if (BapsType.ToUpper() == "NEW")
                {
                    strWhereClause += "StipSiro  = 0 AND ";
                }
                else if (BapsType.ToUpper() == "SIRO")
                {
                    strWhereClause += "StipSiro  > 0 AND ";
                }
                else if (BapsType.ToUpper() == "Power")
                {
                    strWhereClause += "BapsType = 'Power' AND ";
                    if (model.PowerTypeID != null && model.PowerTypeID != 0)
                        strWhereClause += "PowerTypeID = '" + model.PowerTypeID + "' AND ";
                }

                //Added by ASE
                if (!string.IsNullOrWhiteSpace(model.SiteID))
                    strWhereClause += "SiteID LIKE '%" + model.SiteID.TrimStart().TrimEnd() + "%' AND ";
                if (!string.IsNullOrWhiteSpace(model.SiteName))
                    strWhereClause += "SiteName LIKE '%" + model.SiteName.TrimStart().TrimEnd() + "%' AND ";
                if (!string.IsNullOrWhiteSpace(model.CustomerSiteID))
                    strWhereClause += "CustomerSiteID LIKE '%" + model.CustomerSiteID.TrimStart().TrimEnd() + "%' AND ";
                if (!string.IsNullOrWhiteSpace(model.CustomerSiteName))
                    strWhereClause += "CustomerSiteName LIKE '%" + model.CustomerSiteName.TrimStart().TrimEnd() + "%' AND ";
                //End

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                list.Add(new vwMonitoringBapsDoneDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardService", "GetMonitoringBAPSDoneCount", UserID)));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion
    }
}
