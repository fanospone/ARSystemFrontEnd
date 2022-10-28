using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using System.Data;

namespace ARSystem.Domain.Repositories
{
  public  class RTIAchievementDetailRepository : BaseRepository<RTINOverdueDetailModel>
    {
        private DbContext _context;
        public RTIAchievementDetailRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<RTINOverdueDetailModel>> GetRTIAchivementDetailByCustomer(string customerId, int month, int year, string whereClause, int rowSkip, int pageSize, string type, string category, string orderBy)
        {
            return await pGetRTIAchivementDetailByCustomer(customerId,month, year, whereClause, rowSkip, pageSize,type, category, orderBy);
        }

        public async Task<List<RTINOverdueDetailModel>> GetRTIAchivementDetailByGroup(string groupBy, int year, int month, int rowSkip, int pageSize, string type)
        {
            return await pGetRTIAchivementDetailByGroup(groupBy, year, month, rowSkip, pageSize,type);
        }

        public int GetCountRTIAchivementDetailByGroup(string groupBy, int year, int month)
        {
            return pGetCountRTIAchivementDetailByGroup(groupBy, year, month);
        }

        public int GetCountRTIAchivementDetailByCustomer(string customerId, int month, int year, string vWhereClause, string category)
        {
            return pGetCountRTIAchivementDetailByCustomer(customerId, month, year,  vWhereClause, category);
        }
        private async Task<List<RTINOverdueDetailModel>> pGetRTIAchivementDetailByCustomer(string customerId, int month, int year, string whereClause, int rowSkip, int pageSize, string type, string category, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardAchivementRTIDetail]";
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", customerId));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                
                command.Parameters.Add(command.CreateParameter("@vMonth", month));
                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vwhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vCategory", category));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                return this.ReadTransaction(command).ToList();
            }
        }

        private int pGetCountRTIAchivementDetailByCustomer(string customerId, int month, int year, string vWhereClause, string category)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardAchivementRTIDetail]";
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", customerId));
                command.Parameters.Add(command.CreateParameter("@vMonth", month));
                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vwhereClause", vWhereClause));
                command.Parameters.Add(command.CreateParameter("@vCategory", category));
                return this.CountTransaction(command);
            }
        }

        private async Task<List<RTINOverdueDetailModel>> pGetRTIAchivementDetailByGroup(string groupBy, int year, int month, int rowSkip, int pageSize, string type)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardAchivementRTIDetail]";
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", groupBy));
                command.Parameters.Add(command.CreateParameter("@vMonth", month));
                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                return this.ReadTransaction(command).ToList();
            }
        }

        private int pGetCountRTIAchivementDetailByGroup(string groupBy, int year, int month)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardAchivementRTIDetail]";
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", groupBy));
                command.Parameters.Add(command.CreateParameter("@vMonth", month));
                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));

                return this.CountTransaction(command);
            }
        }
    }
}
