using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;

namespace ARSystem.Service
{
    public class DashboardTSELOutstandingService
    {
        public DataTable GetDashboardTSELOutstandingSummaryList(string Type, int? STIPDate, int? RFIDate, int? SectionID, int? SOWID, int? ProductID, int? STIPID, int? RegionalID, string CompanyID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardTSELOverdueRepository(context);
            DataTable dt = new DataTable();
            try
            {
                return Repo.GetDashboardTSELOutstandingSummaryList(Type, STIPDate, RFIDate, SectionID, SOWID, ProductID, STIPID, RegionalID, CompanyID);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "DashboardTSELOutstandingService", "GetDashboardTSELOutstandingSummaryList", "");
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
            var repo = new dwhRAOutstandingTSELGenerateDetailRepository(context);
            List<dwhRAOutstandingTSELGenerateDetail> list = new List<dwhRAOutstandingTSELGenerateDetail>();
            try
            {
                var dt = repo.GetCount(whereClause);
                return dt;
            }
            catch (Exception ex)
            {
                list.Add(new dwhRAOutstandingTSELGenerateDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELOutstandingService", "GetDetailCount", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<dwhRAOutstandingTSELGenerateDetail> GetDetailList(string whereClause, int RowSkip, int PageSize, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new dwhRAOutstandingTSELGenerateDetailRepository(context);
            List<dwhRAOutstandingTSELGenerateDetail> list = new List<dwhRAOutstandingTSELGenerateDetail>();
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
                list.Add(new dwhRAOutstandingTSELGenerateDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELOutstandingService", "GetDetailList", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
