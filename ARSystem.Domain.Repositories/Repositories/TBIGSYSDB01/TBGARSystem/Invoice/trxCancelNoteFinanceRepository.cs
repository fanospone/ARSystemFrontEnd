
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
	public class trxCancelNoteFinanceRepository : BaseRepository<trxCancelNoteFinance>
	{
		private DbContext _context;
		public trxCancelNoteFinanceRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxCancelNoteFinance> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxCancelNoteFinance> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxCancelNoteFinance GetByPK(int trxCancelNoteFinanceID)
		{
			return pGetByPK(trxCancelNoteFinanceID);
		}

		public trxCancelNoteFinance Create(trxCancelNoteFinance data)
		{
			return pCreate(data);
		}

		public List<trxCancelNoteFinance> CreateBulky(List<trxCancelNoteFinance> data)
		{
			return pCreateBulky(data);
		}

		public trxCancelNoteFinance Update(trxCancelNoteFinance data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxCancelNoteFinanceID)
		{
			pDeleteByPK(trxCancelNoteFinanceID);
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
				command.CommandText = "dbo.usptrxCancelNoteFinance";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxCancelNoteFinance> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCancelNoteFinance";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxCancelNoteFinance> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCancelNoteFinance";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxCancelNoteFinance pGetByPK(int trxCancelNoteFinanceID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCancelNoteFinance";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCancelNoteFinanceID", trxCancelNoteFinanceID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxCancelNoteFinance pCreate(trxCancelNoteFinance data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCancelNoteFinance>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCancelNoteFinance";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxCancelNoteFinanceID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxCancelNoteFinance> pCreateBulky(List<trxCancelNoteFinance> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxCancelNoteFinance>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCancelNoteFinance";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxCancelNoteFinance pUpdate(trxCancelNoteFinance data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCancelNoteFinance>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCancelNoteFinance";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxCancelNoteFinanceID", data.trxCancelNoteFinanceID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxCancelNoteFinanceID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCancelNoteFinance";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCancelNoteFinanceID", trxCancelNoteFinanceID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCancelNoteFinance";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}