using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;


namespace ARSystem.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrxBAPSData" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TrxBAPSData.svc or TrxBAPSData.svc.cs at the Solution Explorer and start debugging.
    public class DashboardTSELLeadTimeService
    {
        public List<vwRADetailSiteRecurring> GetDataAverageLeadTime(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwGetDashboardLeadTimeRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetAverageLeadTime(whereClause);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELLeadTimeService", "GetDataLeadTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwRADetailSiteRecurring> GetDataAverageSection(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwGetDashboardLeadTimeRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetAverageSection(whereClause);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELLeadTimeService", "GetDataLeadTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwRADetailSiteRecurring> GetDataAchievement(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwGetDashboardLeadTimeRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetAchievement(whereClause, "MonthBill");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELLeadTimeService", "GetDataLeadTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwRADetailSiteRecurring> GetDataAchievementLeadTime(string strWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwGetDashboardLeadTimeRepository(context);
            List<vwRADetailSiteRecurring> list = new List<vwRADetailSiteRecurring>();
            try
            {
                string whereClause = strWhereClause;
                list = repo.GetAchievementLeadTime(whereClause, "MonthBill");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRADetailSiteRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELLeadTimeService", "GetDataLeadTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}