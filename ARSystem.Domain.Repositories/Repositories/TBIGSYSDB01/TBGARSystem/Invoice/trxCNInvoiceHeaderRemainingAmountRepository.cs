
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
	public class trxCNInvoiceHeaderRemainingAmountRepository : BaseRepository<trxCNInvoiceHeaderRemainingAmount>
	{
		private DbContext _context;
		public trxCNInvoiceHeaderRemainingAmountRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxCNInvoiceHeaderRemainingAmount> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxCNInvoiceHeaderRemainingAmount> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxCNInvoiceHeaderRemainingAmount GetByPK(int trxCNInvoiceHeaderRemainingAmountID)
		{
			return pGetByPK(trxCNInvoiceHeaderRemainingAmountID);
		}

		public trxCNInvoiceHeaderRemainingAmount Create(trxCNInvoiceHeaderRemainingAmount data)
		{
			return pCreate(data);
		}

		public List<trxCNInvoiceHeaderRemainingAmount> CreateBulky(List<trxCNInvoiceHeaderRemainingAmount> data)
		{
			return pCreateBulky(data);
		}

		public trxCNInvoiceHeaderRemainingAmount Update(trxCNInvoiceHeaderRemainingAmount data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxCNInvoiceHeaderRemainingAmountID)
		{
			pDeleteByPK(trxCNInvoiceHeaderRemainingAmountID);
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
				command.CommandText = "dbo.usptrxCNInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxCNInvoiceHeaderRemainingAmount> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxCNInvoiceHeaderRemainingAmount> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxCNInvoiceHeaderRemainingAmount pGetByPK(int trxCNInvoiceHeaderRemainingAmountID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCNInvoiceHeaderRemainingAmountID", trxCNInvoiceHeaderRemainingAmountID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxCNInvoiceHeaderRemainingAmount pCreate(trxCNInvoiceHeaderRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCNInvoiceHeaderRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxCNInvoiceHeaderRemainingAmountID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxCNInvoiceHeaderRemainingAmount> pCreateBulky(List<trxCNInvoiceHeaderRemainingAmount> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxCNInvoiceHeaderRemainingAmount>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxCNInvoiceHeaderRemainingAmount pUpdate(trxCNInvoiceHeaderRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCNInvoiceHeaderRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxCNInvoiceHeaderRemainingAmountID", data.trxCNInvoiceHeaderRemainingAmountID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxCNInvoiceHeaderRemainingAmountID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCNInvoiceHeaderRemainingAmountID", trxCNInvoiceHeaderRemainingAmountID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}