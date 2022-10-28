
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
	public class vwRAReceiveDocumentBAUKRepository : BaseRepository<vwRAReceiveDocumentBAUK>
	{
		private DbContext _context;
		public vwRAReceiveDocumentBAUKRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<vwRAReceiveDocumentBAUK> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

        public List<vwRAReceiveDocumentBAUK> GetListCheckNext(string whereClause = "", string orderBy = "", string Action = "")
        {
            return pGetListCheckNext(whereClause, orderBy, Action);
        }

        public List<vwRAReceiveDocumentBAUK> GetListNotComplete(string whereClause = "", string orderBy = "", string Action = "")
        {
            return pGetListNotComplete(whereClause, orderBy, Action);
        }

        public List<vwRAReceiveDocumentBAUK> GetListComplete(string whereClause = "", string orderBy = "", string Action = "")
        {
            return pGetListComplete(whereClause, orderBy, Action);
        }

        public List<vwRAReceiveDocumentBAUK> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		#region Private

		private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRAReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<vwRAReceiveDocumentBAUK> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRAReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
			}
		}
        private List<vwRAReceiveDocumentBAUK> pGetListCheckNext(string whereClause, string orderBy, string action)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRAReceiveDocumentBAUK";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListCheckNext"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vAction", action));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwRAReceiveDocumentBAUK> pGetListNotComplete(string whereClause, string orderBy, string action)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRAReceiveDocumentBAUK";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListNotComplete"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vAction", action));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwRAReceiveDocumentBAUK> pGetListComplete(string whereClause, string orderBy, string action)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRAReceiveDocumentBAUK";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListComplete"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vAction", action));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwRAReceiveDocumentBAUK> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRAReceiveDocumentBAUK";

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