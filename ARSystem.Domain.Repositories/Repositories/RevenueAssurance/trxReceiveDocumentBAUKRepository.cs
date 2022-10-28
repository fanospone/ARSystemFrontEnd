
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
	public class trxReceiveDocumentBAUKRepository : BaseRepository<trxReceiveDocumentBAUK>
	{
		private DbContext _context;
		public trxReceiveDocumentBAUKRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxReceiveDocumentBAUK> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxReceiveDocumentBAUK> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxReceiveDocumentBAUK GetByPK(int trxReceiveDocID)
		{
			return pGetByPK(trxReceiveDocID);
		}

		public trxReceiveDocumentBAUK Create(trxReceiveDocumentBAUK data)
		{
			return pCreate(data);
		}

		public List<trxReceiveDocumentBAUK> CreateBulky(List<trxReceiveDocumentBAUK> data)
		{
			return pCreateBulky(data);
		}

		public trxReceiveDocumentBAUK Update(trxReceiveDocumentBAUK data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxReceiveDocID)
		{
			pDeleteByPK(trxReceiveDocID);
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
				command.CommandText = "dbo.usptrxReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxReceiveDocumentBAUK> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxReceiveDocumentBAUK> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxReceiveDocumentBAUK pGetByPK(int trxReceiveDocID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxReceiveDocID", trxReceiveDocID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxReceiveDocumentBAUK pCreate(trxReceiveDocumentBAUK data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxReceiveDocumentBAUK>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxReceiveDocID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxReceiveDocumentBAUK> pCreateBulky(List<trxReceiveDocumentBAUK> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxReceiveDocumentBAUK>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxReceiveDocumentBAUK pUpdate(trxReceiveDocumentBAUK data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxReceiveDocumentBAUK>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxReceiveDocID", data.trxReceiveDocID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxReceiveDocID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxReceiveDocID", trxReceiveDocID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxReceiveDocumentBAUK";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}