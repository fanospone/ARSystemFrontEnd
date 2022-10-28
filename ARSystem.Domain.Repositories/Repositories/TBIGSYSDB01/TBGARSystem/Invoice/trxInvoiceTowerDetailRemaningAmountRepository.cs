
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
	public class trxInvoiceTowerDetailRemaningAmountRepository : BaseRepository<trxInvoiceTowerDetailRemaningAmount>
	{
		private DbContext _context;
		public trxInvoiceTowerDetailRemaningAmountRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxInvoiceTowerDetailRemaningAmount> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxInvoiceTowerDetailRemaningAmount> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxInvoiceTowerDetailRemaningAmount GetByPK(int trxInvoiceTowerDetailRemaningAmountId)
		{
			return pGetByPK(trxInvoiceTowerDetailRemaningAmountId);
		}

		public trxInvoiceTowerDetailRemaningAmount Create(trxInvoiceTowerDetailRemaningAmount data)
		{
			return pCreate(data);
		}

		public List<trxInvoiceTowerDetailRemaningAmount> CreateBulky(List<trxInvoiceTowerDetailRemaningAmount> data)
		{
			return pCreateBulky(data);
		}

		public trxInvoiceTowerDetailRemaningAmount Update(trxInvoiceTowerDetailRemaningAmount data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxInvoiceTowerDetailRemaningAmountId)
		{
			pDeleteByPK(trxInvoiceTowerDetailRemaningAmountId);
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
				command.CommandText = "dbo.usptrxInvoiceTowerDetailRemaningAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxInvoiceTowerDetailRemaningAmount> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetailRemaningAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxInvoiceTowerDetailRemaningAmount> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetailRemaningAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxInvoiceTowerDetailRemaningAmount pGetByPK(int trxInvoiceTowerDetailRemaningAmountId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetailRemaningAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceTowerDetailRemaningAmountId", trxInvoiceTowerDetailRemaningAmountId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxInvoiceTowerDetailRemaningAmount pCreate(trxInvoiceTowerDetailRemaningAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoiceTowerDetailRemaningAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetailRemaningAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxInvoiceTowerDetailRemaningAmountId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxInvoiceTowerDetailRemaningAmount> pCreateBulky(List<trxInvoiceTowerDetailRemaningAmount> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxInvoiceTowerDetailRemaningAmount>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetailRemaningAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxInvoiceTowerDetailRemaningAmount pUpdate(trxInvoiceTowerDetailRemaningAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoiceTowerDetailRemaningAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetailRemaningAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceTowerDetailRemaningAmountId", data.trxInvoiceTowerDetailRemaningAmountId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxInvoiceTowerDetailRemaningAmountId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetailRemaningAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceTowerDetailRemaningAmountId", trxInvoiceTowerDetailRemaningAmountId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceTowerDetailRemaningAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}