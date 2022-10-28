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
  public  class RTIAchievementRepository : BaseRepository<RTIAchievementModel>
    {
        private DbContext _context;
        public RTIAchievementRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<RTIAchievementModel>> GetRTIAchivementByCustomer(string customerID, int year, string vWhereClause)
        {
            return await pGetRTIAchivementByCustomer(customerID, year, vWhereClause);
        }

        public async Task<List<RTIAchievementModel>> GetRTIAchivementByGroup(string groupBy, int year, int? month, string vWhereClause)
        {
            return await pGetRTIAchivementByGroup(groupBy, year, month, vWhereClause);
        }

        private async Task<List<RTIAchievementModel>> pGetRTIAchivementByCustomer(string customerID, int year, string vWhereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardAchivementRTI]";
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", vWhereClause));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", customerID));
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<RTIAchievementModel>> pGetRTIAchivementByCustomer2(string customerId, int year)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspAchivementRTI]";
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", customerId));
                return this.ReadTransaction(command).ToList();
            }
        }


        private async Task<List<RTIAchievementModel>> pGetRTIAchivementByGroup(string groupBy, int year, int? month, string vWhereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardAchivementRTI]";
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", groupBy));
                command.Parameters.Add(command.CreateParameter("@vMonth", month));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", vWhereClause));
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<RTIAchievementModel>> pGetRTIAchivementByGroup2(string groupBy, int year, int? month)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspAchivementRTI]";
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", groupBy));
                command.Parameters.Add(command.CreateParameter("@vMonth", month));
                return this.ReadTransaction(command).ToList();
            }
        }
    }
}
