
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
	public class vwRABapsDoneRepository : BaseRepository<vwRABapsDone>
	{
		private DbContext _context;
		public vwRABapsDoneRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

        public int GetCountView(string whereClause, string startDate, string endDate, string towerType)
        {
            return pGetCountView(whereClause, startDate, endDate.Replace("00:00:00", "23:59:59"), towerType);
        }

        public List<vwRABapsDone> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<vwRABapsDone> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

        public List<vwRABapsDone> GetTreeViewList(string whereClause = "", string startDate = "", string endDate = "", string towerType = "TOWER", string orderBy = "")
        {
            return pGetTreeViewList(whereClause, startDate, endDate, towerType, orderBy);
        }

        public List<vwRABapsDone> GetGridViewList(string whereClause = "", string startDate = "", string endDate = "", string towerType = "TOWER", string orderBy = "")
        {
            return pGetGridViewList(whereClause, startDate, endDate, towerType, orderBy);
        }

        public List<vwRABapsDone> GetGridViewPaged(string whereClause, string startDate, string endDate, string towerType, string orderBy, int rowSkip, int pageSize)
        {
            return pGetGridViewPaged(whereClause, startDate, endDate.Replace("00:00:00", "23:59:59"), towerType, orderBy, rowSkip, pageSize);
        }


        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRABapsDone";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<vwRABapsDone> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRABapsDone";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<vwRABapsDone> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRABapsDone";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}

        private int pGetCountView(string whereClause, string startDate, string endDate, string towerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsDoneHO";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCountView"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vTowerType", towerType));
                command.Parameters.Add(command.CreateParameter("@vStartDateHO", startDate));
                command.Parameters.Add(command.CreateParameter("@vEndDateHO", endDate));

                return this.CountTransaction(command);
            }
        }
        private List<vwRABapsDone> pGetGridViewPaged(string whereClause, string startDate, string endDate, string towerType, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsDoneHO";

                command.Parameters.Add(command.CreateParameter("@vType", "GetGridViewPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vTowerType", towerType));
                command.Parameters.Add(command.CreateParameter("@vStartDateHO", startDate));
                command.Parameters.Add(command.CreateParameter("@vEndDateHO", endDate));
               // command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwRABapsDone> pGetGridViewList(string whereClause, string startDate, string endDate, string towerType, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsDoneHO";

                command.Parameters.Add(command.CreateParameter("@vType", "GetGridViewList"));
                command.Parameters.Add(command.CreateParameter("@vTowerType", towerType));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vStartDateHO", startDate));
                command.Parameters.Add(command.CreateParameter("@vEndDateHO", endDate));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwRABapsDone> pGetTreeViewList(string whereClause, string startDate, string endDate, string towerType, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsDoneHO";

                command.Parameters.Add(command.CreateParameter("@vType", "GetTreeViewList"));
                command.Parameters.Add(command.CreateParameter("@vTowerType", towerType));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vStartDateHO", startDate));
                command.Parameters.Add(command.CreateParameter("@vEndDateHO", endDate));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }

    


        #endregion

    }
}