
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
    public class DashboardBAUKActivityRepository : BaseRepository<vmDashboardBAUKActivity>
    {
        private DbContext _context;
        public DashboardBAUKActivityRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<vmDashboardBAUKActivity> GetList(string whereClause = "", int month = 0, string strMonth = "", int year = 0, bool amountMode = false, string groupBy = "")
        {
            return pGetList(whereClause, month, strMonth, year, amountMode, groupBy);
        }

        #region Private

        private List<vmDashboardBAUKActivity> pGetList(string whereClause, int month, string strMonth, int year, bool amountMode, string groupBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardBAUKActivity";

                command.Parameters.Add(command.CreateParameter("@vLastSelectedMonth", month));
                command.Parameters.Add(command.CreateParameter("@vMonth", strMonth));
                command.Parameters.Add(command.CreateParameter("@vYear", year));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vAmountMode", amountMode));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", groupBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        #endregion
    }
}