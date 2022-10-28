
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
	public class trxInvoiceHeaderRemainingAmountRepository : BaseRepository<trxInvoiceHeaderRemainingAmount>
	{
		private DbContext _context;
		public trxInvoiceHeaderRemainingAmountRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxInvoiceHeaderRemainingAmount> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxInvoiceHeaderRemainingAmount> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxInvoiceHeaderRemainingAmount GetByPK(int trxInvoiceHeaderRemainingAmountID)
		{
			return pGetByPK(trxInvoiceHeaderRemainingAmountID);
		}

		public trxInvoiceHeaderRemainingAmount Create(trxInvoiceHeaderRemainingAmount data)
		{
			return pCreate(data);
		}

		public List<trxInvoiceHeaderRemainingAmount> CreateBulky(List<trxInvoiceHeaderRemainingAmount> data)
		{
			return pCreateBulky(data);
		}

		public trxInvoiceHeaderRemainingAmount Update(trxInvoiceHeaderRemainingAmount data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxInvoiceHeaderRemainingAmountID)
		{
			pDeleteByPK(trxInvoiceHeaderRemainingAmountID);
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
				command.CommandText = "dbo.usptrxInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxInvoiceHeaderRemainingAmount> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxInvoiceHeaderRemainingAmount> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxInvoiceHeaderRemainingAmount pGetByPK(int trxInvoiceHeaderRemainingAmountID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceHeaderRemainingAmountID", trxInvoiceHeaderRemainingAmountID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxInvoiceHeaderRemainingAmount pCreate(trxInvoiceHeaderRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoiceHeaderRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxInvoiceHeaderRemainingAmountID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxInvoiceHeaderRemainingAmount> pCreateBulky(List<trxInvoiceHeaderRemainingAmount> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxInvoiceHeaderRemainingAmount>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxInvoiceHeaderRemainingAmount pUpdate(trxInvoiceHeaderRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoiceHeaderRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceHeaderRemainingAmountID", data.trxInvoiceHeaderRemainingAmountID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxInvoiceHeaderRemainingAmountID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceHeaderRemainingAmountID", trxInvoiceHeaderRemainingAmountID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}