using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;

namespace ARSystem.Service
{
    public class DashboardPotentialRFITo1stBAPSBillingService
    {
        public DataTable GetDashboardSummaryList(string Type, string STIP, string year, string month, string desc)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardPotentialRFITo1stBAPSBillingRepository(context);
            DataTable dt = new DataTable();
            try
            {
                return Repo.GetDashboardSummaryList(Type, STIP, year, month, desc);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "DashboardPotentialRFITo1stBAPSBillingService", "GetDashboardSummaryList", "");
                return dt;
            }
            finally
            {
                context.Dispose();
            }
        }
        public int GetDetailCount(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new dwhDashboardPotentialRFITo1stBAPSBillingDetailRepository(context);
            List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> list = new List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel>();
            try
            {
                var dt = repo.GetCount(whereClause);
                return dt;
            }
            catch (Exception ex)
            {
                list.Add(new dwhDashboardPotentialRFITo1stBAPSBillingDetailModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardAllOperatorPotentialRecurringOutstandingService", "GetDetailCount", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> GetDetailList(string whereClause, int RowSkip, int PageSize, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new dwhDashboardPotentialRFITo1stBAPSBillingDetailRepository(context);
            List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> list = new List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel>();
            try
            {
                if (PageSize > 0)
                    list = repo.GetPaged(whereClause, strOrderBy, RowSkip, PageSize);
                else
                    list = repo.GetList(whereClause);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new dwhDashboardPotentialRFITo1stBAPSBillingDetailModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardAllOperatorPotentialRecurringOutstandingService", "GetDetailList", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
         
        public DataTable GetFilterList(string Type)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardPotentialRFITo1stBAPSBillingRepository(context);
            DataTable dt = new DataTable();
            try
            {
                return Repo.GetFilterList(Type);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "DashboardPotentialRFITo1stBAPSBillingService", "GetDashboardSummaryList", "");
                return dt;
            }
            finally
            {
                context.Dispose();
            }
        }



    }
}
