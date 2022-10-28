using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystem.Service
{
    public class DashboardBAUKService
    {
        public List<vmDashboardBAUKActivity> GetDashboardActivity(string userID, vmDashboardBAUKFilter filter)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var actRepo = new DashboardBAUKActivityRepository(context);
            var actData = new List<vmDashboardBAUKActivity>();

            try
            {
                var cnvFilter = ConcatFilter(filter);

                string strWhereClause = pWhereClauseFilterDashboardBAUKActivity(cnvFilter);

                actData = actRepo.GetList(strWhereClause, filter.Month, filter.strMonth, filter.Year, filter.AmountMode, filter.GroupBy);

                int TotalAllBAUK = 0;
                decimal TotalAmountBAUK = 0;
                foreach (var row in actData)
                {
                    if (filter.AmountMode)
                    {
                        row.AmountTotal = row.AmountBAUKApproved + row.AmountBAUKRejected + row.AmountBAUKSubmitted;
                        TotalAmountBAUK += row.AmountTotal;
                    }
                    else
                    {
                        row.Total = row.BAUKApproved + row.BAUKRejected + row.BAUKSubmitted;
                        TotalAllBAUK += row.Total;
                    }
                }

                foreach (var row in actData)
                {
                    decimal rowTot;
                    decimal allTot;

                    if (filter.AmountMode)
                    {
                        rowTot = Convert.ToDecimal(row.AmountTotal);
                        allTot = Convert.ToDecimal(TotalAmountBAUK);
                    }
                    else
                    {
                        rowTot = Convert.ToDecimal(row.Total);
                        allTot = Convert.ToDecimal(TotalAllBAUK);
                    }

                    row.PercentTotal = allTot == 0 ? 0 : (rowTot / allTot) * 100;
                }

                return actData;
            }
            catch (Exception ex)
            {
                actData.Add(new vmDashboardBAUKActivity((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardBAUKService", "GetDashboardActivity", userID)));
                return actData;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardBAUKDetail> GetDashboardActivityDetail(string userID, vmDashboardBAUKDetail filter)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var frcRepo = new DashboardBAUKActivityDetailRepository(context);
            var list = new List<vmDashboardBAUKDetail>();

            try
            {
                var cnvFilter = ConcatFilterDetail(filter);

                list = frcRepo.GetList(cnvFilter);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vmDashboardBAUKDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardBAUKService", "GetDashboardActivityDetail", userID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardBAUKForecast> GetDashboardForecast(string userID, vmDashboardBAUKFilter filter)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var frcRepo = new DashboardBAUKForecastRepository(context);
            var list = new List<vmDashboardBAUKForecast>();

            try
            {
                var cnvFilter = ConcatFilter(filter);

                list = frcRepo.GetList(cnvFilter);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vmDashboardBAUKForecast((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardBAUKService", "GetDashboardForecast", userID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardBAUKDetail> GetDashboardForecastDetail(string userID, vmDashboardBAUKDetail filter)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var frcRepo = new DashboardBAUKForecastDetailRepository(context);
            var list = new List<vmDashboardBAUKDetail>();

            try
            {
                var cnvFilter = ConcatFilterDetail(filter);

                list = frcRepo.GetList(cnvFilter);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vmDashboardBAUKDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardBAUKService", "GetDashboardForecastDetail", userID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardBAUKAchievement> GetDashboardAchievement(string userID, vmDashboardBAUKFilter filter)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var frcRepo = new DashboardBAUKAchievementRepository(context);
            var list = new List<vmDashboardBAUKAchievement>();

            try
            {
                var cnvFilter = ConcatFilter(filter);

                list = frcRepo.GetList(cnvFilter);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vmDashboardBAUKAchievement((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardBAUKService", "GetDashboardAchievement", userID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardBAUKDetail> GetDashboardAchievementDetail(string userID, vmDashboardBAUKDetail filter)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var frcRepo = new DashboardBAUKAchievementDetailRepository(context);
            var list = new List<vmDashboardBAUKDetail>();

            try
            {
                var cnvFilter = ConcatFilterDetail(filter);

                list = frcRepo.GetList(cnvFilter);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vmDashboardBAUKDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardBAUKService", "GetDashboardAchievementDetail", userID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardBAUKReject> GetDashboardReject(string userID, vmDashboardBAUKFilter filter)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var rejectRepo = new DashboardBAUKRejectRepository(context);
            var listRejectSum = new List<vmDashboardBAUKReject>();

            try
            {
                var cnvFilter = ConcatFilter(filter);

                string strWhereClause = pWhereClauseFilterDashboardBAUKReject(cnvFilter);

                listRejectSum = rejectRepo.GetList(strWhereClause, filter.GroupBy);

                int TotalAllBAUK = 0;
                foreach (var row in listRejectSum)
                {
                    TotalAllBAUK += row.Total;
                }

                foreach (var row in listRejectSum)
                {
                    decimal rowTot = Convert.ToDecimal(row.Total);
                    decimal allTot = Convert.ToDecimal(TotalAllBAUK);
                    row.PercentTotal = (rowTot / allTot) * 100;
                }

                return listRejectSum;
            }
            catch (Exception ex)
            {
                listRejectSum.Add(new vmDashboardBAUKReject((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardBAUKService", "GetDashboardReject", userID)));
                return listRejectSum;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetCountBAUKRejectDetail(string userID, vwBAUKRejectDocumentDetail filter, List<int> months)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var rejectRepo = new vwBAUKRejectDocumentDetailRepository(context);

            try
            {
                string strWhereClause = "";

                if (!string.IsNullOrWhiteSpace(filter.CustomerID))
                {
                    strWhereClause += "CustomerID = '" + filter.CustomerID + "'";
                }
                else
                {
                    strWhereClause += "STIPID = " + filter.STIPID + "";
                }

                string strMonth = JoinIntConcat(months);

                strWhereClause += " AND Month IN (" + strMonth + ") AND Year = " + filter.Year + " AND CheckType = " + filter.CheckType;

                int listCount = rejectRepo.GetCount(strWhereClause);

                return listCount;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwBAUKRejectDocumentDetail> GetListBAUKRejectDetail(string userID, vwBAUKRejectDocumentDetail filter, List<int> months)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var rejectRepo = new vwBAUKRejectDocumentDetailRepository(context);
            var list = new List<vwBAUKRejectDocumentDetail>();

            try
            {
                string strWhereClause = "";

                if (!string.IsNullOrWhiteSpace(filter.CustomerID))
                {
                    strWhereClause += "CustomerID = '" + filter.CustomerID + "'";
                }
                if (filter.STIPID != null || filter.STIPID == 0)
                {
                    strWhereClause += "STIPID = " + filter.STIPID + "";
                }

                string strMonth = JoinIntConcat(months);

                strWhereClause += " AND Month IN (" + strMonth + ") AND Year = " + filter.Year + " AND CheckType = " + filter.CheckType;

                list = rejectRepo.GetList(strWhereClause);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwBAUKRejectDocumentDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardBAUKService", "GetBAUKRejectDetail", userID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        #region Private
        private string pWhereClauseFilterDashboardBAUKActivity(vmDashboardBAUKFilter filter)
        {
            string strWhereClause = "";

            if (!string.IsNullOrWhiteSpace(filter.strCompany))
            {
                strWhereClause += " AND CompanyID IN (" + filter.strCompany + ")";
            }
            if (!string.IsNullOrWhiteSpace(filter.strCustomer))
            {
                strWhereClause += " AND CustomerID IN (" + filter.strCustomer + ")";
            }
            if (!string.IsNullOrWhiteSpace(filter.strProduct))
            {
                strWhereClause += " AND ProductID IN (" + filter.strProduct + ")";
            }
            if (!string.IsNullOrWhiteSpace(filter.strSTIP))
            {
                strWhereClause += " AND STIPID IN (" + filter.strSTIP + ")";
            }

            return strWhereClause;
        }

        private string pWhereClauseFilterDashboardBAUKReject(vmDashboardBAUKFilter filter)
        {
            string strWhereClause = "Month IN (" + filter.strMonth + ") AND Year = " + filter.Year;

            if (!string.IsNullOrWhiteSpace(filter.strCompany))
            {
                strWhereClause += " AND CompanyID IN (" + filter.strCompany + ")";
            }
            if (!string.IsNullOrWhiteSpace(filter.strCustomer))
            {
                strWhereClause += " AND CustomerID IN (" + filter.strCustomer + ")";
            }
            if (!string.IsNullOrWhiteSpace(filter.strProduct))
            {
                strWhereClause += " AND ProductID IN (" + filter.strProduct + ")";
            }
            if (!string.IsNullOrWhiteSpace(filter.strSTIP))
            {
                strWhereClause += " AND STIPID IN (" + filter.strSTIP + ")";
            }

            return strWhereClause;
        }

        private vmDashboardBAUKFilter ConcatFilter(vmDashboardBAUKFilter filter)
        {
            if (filter.CustomerIDs != null)
                filter.strCustomer = JoinStringConcat(filter.CustomerIDs);
            if (filter.CompanyIDs != null)
                filter.strCompany = JoinStringConcat(filter.CompanyIDs);
            if (filter.STIPIDs != null)
                filter.strSTIP = JoinIntConcat(filter.STIPIDs);
            if (filter.ProductIDs != null)
                filter.strProduct = JoinIntConcat(filter.ProductIDs);
            if (filter.Months != null)
                filter.strMonth = JoinIntConcat(filter.Months);

            return filter;
        }

        private vmDashboardBAUKDetail ConcatFilterDetail(vmDashboardBAUKDetail filter)
        {
            if (filter.CustomerIDs != null)
                filter.strCustomer = JoinStringConcat(filter.CustomerIDs);
            if (filter.CompanyIDs != null)
                filter.strCompany = JoinStringConcat(filter.CompanyIDs);
            if (filter.STIPIDs != null)
                filter.strSTIP = JoinIntConcat(filter.STIPIDs);
            if (filter.ProductIDs != null)
                filter.strProduct = JoinIntConcat(filter.ProductIDs);
            if (filter.Months != null)
                filter.strMonth = JoinIntConcat(filter.Months);

            return filter;
        }

        private string JoinStringConcat(List<string> listStr)
        {
            List<string> temp = new List<string>();
            listStr.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x))
                    temp.Add("'" + x + "'");
            });
            return String.Join(",", temp);
        }

        private string JoinIntConcat(List<int> listInt)
        {
            List<string> temp = new List<string>();
            listInt.ForEach(x =>
            {
                if (x != 0)
                    temp.Add(x.ToString());
            });
            return String.Join(",", temp);
        }
        #endregion
    }
}
