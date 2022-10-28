using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

using System.Threading.Tasks;

namespace ARSystem.Service
{
    public class RTILeadTimeService
    {
        #region public methode
        public async Task<List<RTILeadTimeModel>> GetDataLeadTime(int Year, string GroupBy)
        {
            return await pGetDataLeadTime(Year, GroupBy);
        }

        public async Task<List<RTINOverdueDetailModel>> GetDataLeadTimeDetail(string LeadTime, int Year, string CustomerID, int RowSkip, int PageSize)
        {
            return await pGetDataLeadTimeDetail(LeadTime, Year, CustomerID, RowSkip, PageSize);
        }

        public int GetCountLeadTimeDetail(string LeadTime, int Year, string CustomerID)
        {
            return pGetCountLeadTimeDetail(LeadTime, Year, CustomerID);
        }

        public async Task<List<RTILeadTimeModel>> GetDataLeadTimeAvg(int Year, string GroupBy)
        {
            return await pGetDataLeadTimeAvg(Year, GroupBy);
        }

        public async Task<List<RTILeadTimeModel>> GetStatusReconcile(string CustomerID, int Year)
        {
            return await pGetStatusReconcile(CustomerID, Year);
        }

        public async Task<List<RTINOverdueDetailModel>> GetListStatusReconcileDetail(string CustomerID, int Year, string currentStatus, int rowSkip, int pageSize)
        {
            return await pGetListStatusReconcileDetail(CustomerID, Year, currentStatus, rowSkip, pageSize);
        }

        public int GetCountStatusReconcileDetail(string CustomerID, int Year, string currentStatus)
        {
            return pGetCountStatusReconcileDetail(CustomerID, Year, currentStatus);
        }

        #endregion

        #region private methode

        private async Task<List<RTILeadTimeModel>> pGetDataLeadTime(int Year, string GroupBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTILeadTimeRepository(context);
            List<RTILeadTimeModel> list = new List<RTILeadTimeModel>();
            try
            {

                list = await repo.GetDataLeadeTime(Year, GroupBy);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new RTILeadTimeModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataLeadeTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        private async Task<List<RTILeadTimeModel>> pGetDataLeadTimeAvg(int Year, string GroupBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTILeadTimeRepository(context);
            List<RTILeadTimeModel> list = new List<RTILeadTimeModel>();
            try
            {

                list = await repo.GetDataLeadeTimeAvg(Year, GroupBy);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new RTILeadTimeModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataLeadeTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        private async Task<List<RTILeadTimeModel>> pGetStatusReconcile(string CustomerID, int Year)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTILeadTimeRepository(context);
            List<RTILeadTimeModel> list = new List<RTILeadTimeModel>();
            try
            {

                list = await repo.GetStatusReconcile(CustomerID, Year);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new RTILeadTimeModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataLeadeTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        private async Task<List<RTINOverdueDetailModel>> pGetDataLeadTimeDetail(string LeadTime, int Year, string CustomerID, int RowSkip, int PageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTILeadTimeDetailRepository(context);
            List<RTINOverdueDetailModel> list = new List<RTINOverdueDetailModel>();
            try
            {
                if (PageSize > 0)
                    list = await repo.GetPaged(LeadTime, Year, CustomerID, RowSkip, PageSize);
                else
                    list = await repo.GetList(LeadTime, Year, CustomerID);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new RTINOverdueDetailModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataLeadeTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        private int pGetCountLeadTimeDetail(string LeadTime, int Year, string CustomerID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTILeadTimeDetailRepository(context);
            try
            {
                return repo.GetCount(LeadTime, Year, CustomerID);
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

        private async Task<List<RTINOverdueDetailModel>> pGetListStatusReconcileDetail(string CustomerID, int Year, string currentStatus, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTILeadTimeDetailRepository(context);
            List<RTINOverdueDetailModel> list = new List<RTINOverdueDetailModel>();
            try
            {
                if (pageSize > 0)
                    list = await repo.GetPagedStatusReconcile(CustomerID, Year, currentStatus, "GetPaged", rowSkip, pageSize);
                else
                    list = await repo.GetPagedStatusReconcile(CustomerID, Year, currentStatus, "GetList", rowSkip, pageSize);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new RTINOverdueDetailModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataLeadeTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        private int pGetCountStatusReconcileDetail(string CustomerID, int Year, string currentStatus)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTILeadTimeDetailRepository(context);
            try
            {

                return repo.GetCountStatusReconcile(CustomerID, Year, currentStatus);
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


        #endregion

    }
}
