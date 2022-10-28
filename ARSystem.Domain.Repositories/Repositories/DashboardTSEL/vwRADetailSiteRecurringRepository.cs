
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
	public class vwRADetailSiteRecurringRepository : BaseRepository<vwRADetailSiteRecurring>
	{
		private DbContext _context;
		public vwRADetailSiteRecurringRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<vwRADetailSiteRecurring> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<vwRADetailSiteRecurring> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}
        
        #region Target
        public List<vwRADetailSiteRecurring> GetTargetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRADetailSiteTargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetTargetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        public List<vwRADetailSiteRecurring> GetTargetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRADetailSiteTargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }

        public int GetTargetCount(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRADetailSiteTargetRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        #endregion

        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRADetailSiteRecurring";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<vwRADetailSiteRecurring> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRADetailSiteRecurring";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<vwRADetailSiteRecurring> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRADetailSiteRecurring";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		#endregion

	}
}