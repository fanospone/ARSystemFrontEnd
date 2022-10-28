using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;
using System.Data;

namespace ARSystem.Service
{
    public class SummaryDataAPService
    {
        public int GetCountSummaryDataAP()
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var repo = new vwDwhSummaryDataAPRepository(context);

            try
            {
                return repo.GetCount("");
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

        public List<vwDwhSummaryDataAP> GetListSummaryDataAP()
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var repo = new vwDwhSummaryDataAPRepository(context);
            List<vwDwhSummaryDataAP> list = new List<vwDwhSummaryDataAP>();
            try
            {
                string whereClause = "";
                list = repo.GetList(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwDwhSummaryDataAP((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SummaryDataAPService", "GetListSummaryDataAP", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public DataTable GetGroupSummaryDataAP()
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var repo = new vwDwhSummaryDataAPRepository(context);
            List<vwDwhSummaryDataAP> list = new List<vwDwhSummaryDataAP>();
            try
            {
                string WhereClause = "";
                var data = repo.pGetGroupTable(WhereClause, "");
                return data;
            }
            catch (Exception ex)
            {
                list.Add(new vwDwhSummaryDataAP((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SummaryDataAPService", "GetSummaryDataAP", "")));
                return null;
            }
            finally
            {
                context.Dispose();
            }
        }

        public DataTable GetSummaryDataAP(string WhereClause)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var repo = new vwDwhSummaryDataAPRepository(context);
            List<vwDwhSummaryDataAP> list = new List<vwDwhSummaryDataAP>();
            try
            {
                var data = repo.pGetDataTable(WhereClause, "");
                return data;
            }
            catch (Exception ex)
            {
                list.Add(new vwDwhSummaryDataAP((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SummaryDataAPService", "GetSummaryDataAP", "")));
                return null;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetCountSummaryDataAPTenant(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var repo = new vwSummaryAPTenantRepository(context);
            List<vwSummaryAPTenant> list = new List<vwSummaryAPTenant>();
            try
            {
                var dt = repo.GetCount(whereClause);
                return dt;
            }
            catch (Exception ex)
            {
                list.Add(new vwSummaryAPTenant((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SummaryDataAPService", "GetListSummaryDataAPTenant", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwSummaryAPTenant> GetListSummaryDataAPTenant(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var repo = new vwSummaryAPTenantRepository(context);
            List<vwSummaryAPTenant> list = new List<vwSummaryAPTenant>();
            try
            {
                list = repo.GetList(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwSummaryAPTenant((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SummaryDataAPService", "GetListSummaryDataAPTenant", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwSummaryAPTenant> GetPageSummaryDataAPTenant(string whereClause,int RowSkip,int PageSize)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var repo = new vwSummaryAPTenantRepository(context);
            List<vwSummaryAPTenant> list = new List<vwSummaryAPTenant>();
            try
            {
                list = repo.GetPaged(whereClause, "", RowSkip, PageSize);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwSummaryAPTenant((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SummaryDataAPService", "GetPageSummaryDataAPTenant", "SummaryAP")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwDwhAPTenantCost> GetListDataAPTenantCost(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var repo = new vwDwhAPTenantCostRepository(context);
            List<vwDwhAPTenantCost> list = new List<vwDwhAPTenantCost>();
            try
            {
                if (!string.IsNullOrEmpty(whereClause))
                    whereClause = " SONumber = '" + whereClause + "'";

                list = repo.GetList(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwDwhAPTenantCost((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SummaryDataAPService", "GetListDataAPTenantCost", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwDwhAPTenantRevenue> GetListDataAPTenantRevenue(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("DWH"));
            var repo = new vwDwhAPTenantRevenueRepository(context);
            List<vwDwhAPTenantRevenue> list = new List<vwDwhAPTenantRevenue>();
            try
            {
                if (!string.IsNullOrEmpty(whereClause))
                    whereClause = " SONumber = '" + whereClause + "'";

                list = repo.GetList(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwDwhAPTenantRevenue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SummaryDataAPService", "GetListDataAPTenantRevenue", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
