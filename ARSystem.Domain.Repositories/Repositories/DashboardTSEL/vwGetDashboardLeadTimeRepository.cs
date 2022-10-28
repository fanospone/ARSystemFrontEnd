
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
	public class vwGetDashboardLeadTimeRepository : BaseRepository<vwRADetailSiteRecurring>
	{
		private DbContext _context;
		public vwGetDashboardLeadTimeRepository(DbContext context) : base(context)
		{
			_context = context;
		}

        public List<vwRADetailSiteRecurring> GetAverageLeadTime(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELLeadTime";

                command.Parameters.Add(command.CreateParameter("@vType", "GetAverageLeadTime"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vwRADetailSiteRecurring> GetAverageSection(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELLeadTime";

                command.Parameters.Add(command.CreateParameter("@vType", "GetAverageSection"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vwRADetailSiteRecurring> GetAchievement(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELLeadTime";

                command.Parameters.Add(command.CreateParameter("@vType", "GetAchievement"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vwRADetailSiteRecurring> GetAchievementLeadTime(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELLeadTime";

                command.Parameters.Add(command.CreateParameter("@vType", "GetLeadTime"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }

    }
}