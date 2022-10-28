
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
    public class DashboardBAUKRejectRepository : BaseRepository<vmDashboardBAUKReject>
    {
        private DbContext _context;
        public DashboardBAUKRejectRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<vmDashboardBAUKReject> GetList(string whereClause = "", string groupBy = "")
        {
            return pGetList(whereClause, groupBy);
        }

        #region Private

        private List<vmDashboardBAUKReject> pGetList(string whereClause, string groupBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardBAUKRejectSummary";

                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", groupBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        #endregion
    }
}