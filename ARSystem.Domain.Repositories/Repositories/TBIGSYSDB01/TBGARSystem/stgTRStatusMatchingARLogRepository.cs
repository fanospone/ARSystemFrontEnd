
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
	public class stgTRStatusMatchingARLogRepository : BaseRepository<stgTRStatusMatchingARLog>
	{
		private DbContext _context;
		public stgTRStatusMatchingARLogRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<stgTRStatusMatchingARLog> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<stgTRStatusMatchingARLog> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public stgTRStatusMatchingARLog GetByPK(long iD)
		{
			return pGetByPK(iD);
		}

		public stgTRStatusMatchingARLog Create(stgTRStatusMatchingARLog data)
		{
			return pCreate(data);
		}

		public List<stgTRStatusMatchingARLog> CreateBulky(List<stgTRStatusMatchingARLog> data)
		{
			return pCreateBulky(data);
		}

		public stgTRStatusMatchingARLog Update(stgTRStatusMatchingARLog data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(long iD)
		{
			pDeleteByPK(iD);
			return true;
		}

		public bool DeleteByFilter(string whereClause)
		{
			pDeleteByFilter(whereClause);
			return true;
		}

		#region Private

		private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<stgTRStatusMatchingARLog> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<stgTRStatusMatchingARLog> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private stgTRStatusMatchingARLog pGetByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private stgTRStatusMatchingARLog pCreate(stgTRStatusMatchingARLog data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<stgTRStatusMatchingARLog>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLog";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<stgTRStatusMatchingARLog> pCreateBulky(List<stgTRStatusMatchingARLog> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<stgTRStatusMatchingARLog>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLog";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private stgTRStatusMatchingARLog pUpdate(stgTRStatusMatchingARLog data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<stgTRStatusMatchingARLog>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLog";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ID", data.ID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLog";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLog";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}