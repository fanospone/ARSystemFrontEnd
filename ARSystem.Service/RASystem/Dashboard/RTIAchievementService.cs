using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystem.Service
{
    public class RTIAchievementService
    {
        public async Task<List<RTIAchievementModel>> GetRTIAchivementByCustomer(string customerID, int year, string vWhereClause)
        { 
            return await pGetRTIAchivementByCustomer(customerID, year, vWhereClause);
        }

        public async Task<List<RTIAchievementModel>> GetRTIAchivementByGroup(string groupBy, int year, int? month, string vWhereClause)
        {
            return await pGetRTIAchivementByGroup(groupBy, year, month, vWhereClause);
        }

        public async Task<List<RTINOverdueDetailModel>> GetRTIAchivementDetailByCustomer(List<string> customerId, int month, int year, string category, List<string> departmentCode, int rowSkip, int pageSize, string orderBy)
        {
            return await pGetRTIAchivementDetailByCustomer(customerId, month, year, category, departmentCode, rowSkip, pageSize, orderBy);
        }
        public async Task<List<RTINOverdueDetailModel>> GetRTIAchivementDetailByGroup(string groupBy, int year, int month, int rowSkip, int pageSize)
        {
            return await pGetRTIAchivementDetailByGroup(groupBy, year, month, rowSkip, pageSize);
        }
        public int GetCountRTIAchivementDetailByGroup(string groupBy, int year, int month)
        {
            return pGetCountRTIAchivementDetailByGroup(groupBy, year, month);
        }
       public int GetCountRTIAchivementDetailByCustomer(List<string> customerId, int month, int year, string category, List<string> departmentCode)
        {
            return pGetCountRTIAchivementDetailByCustomer(customerId, month, year, category, departmentCode);
        }

        public string GetWhereClauseRTIAchivementDetail(List<string> customerId, int month, int year,  string category, List<string> departmentCode)
        {

            return pGetWhereClauseRTIAchivementDetail(customerId, month, year, category, departmentCode);
        }
        private string pGetWhereClauseRTIAchivementDetail(List<string> customerId, int month, int year,  string category, List<string> departmentCode)
        {
            string strWhereClause = "1=1";
            var custIds = String.Empty;
            if (customerId != null && customerId.Count() > 0)
            {
                custIds = string.Join(",", customerId);
                var customerIds = string.Join(", ", customerId.Select(cust => "'" + cust + "'"));
                if (!customerId.Any(cust => cust == "ALL"))
                {
                    strWhereClause += " AND CustomerID in (" + customerIds + ")";
                }
            }
            if (departmentCode != null && departmentCode.Count() > 0)
            {
                var deptCodes = string.Join(", ", departmentCode.Select(dept => "'" + dept + "'"));
                strWhereClause += " AND DepartmentCode in (" + deptCodes + ")";
            }
            if(category == "Target")
            {
                if(month > 0)
                {
                    strWhereClause += " AND MonthPeriod = " + month ;
                }
                if (year > 0)
                {
                    strWhereClause += " AND YearPeriod = " + year;
                }
            }
            if(category == "Achivement")
            {
                if (month > 0)
                {
                    strWhereClause += " AND Month(BapsConfirmDate) = " + month;
                }
                if (year > 0)
                {
                    strWhereClause += " AND Year(BapsConfirmDate) = " + year;
                }
            }
            return strWhereClause;
        }





        private async Task<List<RTIAchievementModel>> pGetRTIAchivementByCustomer(string customerID, int year, string vWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTIAchievementRepository(context);
            List<RTIAchievementModel> dataRTIList = new List<RTIAchievementModel>();

            try
            {
                dataRTIList = await repo.GetRTIAchivementByCustomer(customerID, year, vWhereClause);
                return dataRTIList;
            }
            catch (Exception ex)
            {
                dataRTIList.Add(new RTIAchievementModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataChart", "")));
                return dataRTIList;

            }
            finally
            {
                context.Dispose();
            }
        }

        private async Task<List<RTIAchievementModel>> pGetRTIAchivementByGroup(string groupBy, int year, int? month, string vWhereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTIAchievementRepository(context);
            List<RTIAchievementModel> dataRTIList = new List<RTIAchievementModel>();

            try
            {
              //  month = month == 0 ? null : month;
                dataRTIList = await repo.GetRTIAchivementByGroup(groupBy, year, month, vWhereClause);
                return dataRTIList;
            }
            catch (Exception ex)
            {
                dataRTIList.Add(new RTIAchievementModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataChart", "")));
                return dataRTIList;

            }
            finally
            {
                context.Dispose();
            }
        }

        private async Task<List<RTINOverdueDetailModel>> pGetRTIAchivementDetailByCustomer(List<string> customerId, int month, int year, string category, List<string> departmentCode, int rowSkip, int pageSize, string orderBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTIAchievementDetailRepository(context);
            List<RTINOverdueDetailModel> dataRTIList = new List<RTINOverdueDetailModel>();
            var vWhereClause = pGetWhereClauseRTIAchivementDetail(customerId, month, year, category, departmentCode);

            try
            {
                var customerIds = String.Join(",", customerId);
                if (pageSize > 0)
                    dataRTIList = await repo.GetRTIAchivementDetailByCustomer(customerIds, month, year, vWhereClause, rowSkip, pageSize, "GetPaged", category, orderBy);
                else
                    dataRTIList = await repo.GetRTIAchivementDetailByCustomer(customerIds, month, year, vWhereClause, rowSkip, pageSize, "GetList", category, orderBy);
                return dataRTIList;
            }
            catch (Exception ex)
            {
                dataRTIList.Add(new RTINOverdueDetailModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataChart", "")));
                return dataRTIList;

            }
            finally
            {
                context.Dispose();
            }
        }

        private async Task<List<RTINOverdueDetailModel>> pGetRTIAchivementDetailByGroup(string groupBy, int year, int month, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTIAchievementDetailRepository(context);
            List<RTINOverdueDetailModel> dataRTIList = new List<RTINOverdueDetailModel>();

            try
            {
                if (pageSize > 0)
                    dataRTIList = await repo.GetRTIAchivementDetailByGroup(groupBy, year, month, rowSkip, pageSize, "GetPaged");
                else
                    dataRTIList = await repo.GetRTIAchivementDetailByGroup(groupBy, year, month, rowSkip, pageSize, "GetList");
                return dataRTIList;
            }
            catch (Exception ex)
            {
                dataRTIList.Add(new RTINOverdueDetailModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataChart", "")));
                return dataRTIList;

            }
            finally
            {
                context.Dispose();
            }
        }

        private int pGetCountRTIAchivementDetailByGroup(string groupBy, int year, int month)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTIAchievementDetailRepository(context);
            List<RTINOverdueDetailModel> dataRTIList = new List<RTINOverdueDetailModel>();

            try
            {
                return repo.GetCountRTIAchivementDetailByGroup(groupBy, year, month);
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

        private int pGetCountRTIAchivementDetailByCustomer(List<string> customerId, int month, int year, string category, List<string> departmentCode)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTIAchievementDetailRepository(context);
            List<RTINOverdueDetailModel> dataRTIList = new List<RTINOverdueDetailModel>();
            var vWhereClause = pGetWhereClauseRTIAchivementDetail(customerId, month, year, category, departmentCode);
            try
            {
                var customerIds = String.Join(",", customerId);
                return repo.GetCountRTIAchivementDetailByCustomer(customerIds, month, year, vWhereClause, category);
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
    }
}
