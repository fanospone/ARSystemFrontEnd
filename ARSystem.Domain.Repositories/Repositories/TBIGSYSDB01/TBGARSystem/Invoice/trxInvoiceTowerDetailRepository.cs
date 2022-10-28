
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
	public class trxInvoiceTowerDetailRepository : BaseRepository<trxInvoiceTowerDetail>
	{
		private DbContext _context;
		public trxInvoiceTowerDetailRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxInvoiceTowerDetail> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxInvoiceTowerDetail> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxInvoiceTowerDetail GetByPK(int trxInvoiceTowerDetailId)
		{
			return pGetByPK(trxInvoiceTowerDetailId);
		}

		public trxInvoiceTowerDetail Create(trxInvoiceTowerDetail data)
		{
			return pCreate(data);
		}

		public List<trxInvoiceTowerDetail> CreateBulky(List<trxInvoiceTowerDetail> data)
		{
			return pCreateBulky(data);
		}

		public trxInvoiceTowerDetail Update(trxInvoiceTowerDetail data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxInvoiceTowerDetailId)
		{
			pDeleteByPK(trxInvoiceTowerDetailId);
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
				command.CommandText = "dbo.usptrxInvoiceTowerDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxInvoiceTowerDetail> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxInvoiceTowerDetail> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxInvoiceTowerDetail pGetByPK(int trxInvoiceTowerDetailId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceTowerDetailId", trxInvoiceTowerDetailId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxInvoiceTowerDetail pCreate(trxInvoiceTowerDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoiceTowerDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxInvoiceTowerDetailId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxInvoiceTowerDetail> pCreateBulky(List<trxInvoiceTowerDetail> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxInvoiceTowerDetail>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxInvoiceTowerDetail pUpdate(trxInvoiceTowerDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoiceTowerDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceTowerDetailId", data.trxInvoiceTowerDetailId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxInvoiceTowerDetailId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceTowerDetailId", trxInvoiceTowerDetailId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}