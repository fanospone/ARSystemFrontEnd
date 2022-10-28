
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;

namespace ARSystem.Domain.Repositories
{
    public class DashboardBAUKAchievementRepository : BaseRepository<vmDashboardBAUKAchievement>
    {
        private DbContext _context;
        public DashboardBAUKAchievementRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<vmDashboardBAUKAchievement> GetList(vmDashboardBAUKFilter filter)
        {
            return pGetList(filter);
        }

        #region Private

        private List<vmDashboardBAUKAchievement> pGetList(vmDashboardBAUKFilter filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardBAUKAchievement";

                command.Parameters.Add(command.CreateParameter("@vYear", filter.Year));
                command.Parameters.Add(command.CreateParameter("@vCustomer", filter.strCustomer == null ? "" : filter.strCustomer));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.strCompany == null ? "" : filter.strCompany));
                command.Parameters.Add(command.CreateParameter("@vSTIP", filter.strSTIP == null ? "" : filter.strSTIP));
                command.Parameters.Add(command.CreateParameter("@vProduct", filter.strProduct == null ? "" : filter.strProduct));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", filter.GroupBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        #endregion
    }
}