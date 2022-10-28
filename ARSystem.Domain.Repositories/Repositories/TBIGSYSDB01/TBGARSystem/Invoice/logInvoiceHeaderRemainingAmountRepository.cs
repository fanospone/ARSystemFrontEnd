
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
	public class logInvoiceHeaderRemainingAmountRepository : BaseRepository<logInvoiceHeaderRemainingAmount>
	{
		private DbContext _context;
		public logInvoiceHeaderRemainingAmountRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<logInvoiceHeaderRemainingAmount> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<logInvoiceHeaderRemainingAmount> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public logInvoiceHeaderRemainingAmount GetByPK(int trxInvoiceHeaderLogRemainingAmountID)
		{
			return pGetByPK(trxInvoiceHeaderLogRemainingAmountID);
		}

		public logInvoiceHeaderRemainingAmount Create(logInvoiceHeaderRemainingAmount data)
		{
			return pCreate(data);
		}

		public List<logInvoiceHeaderRemainingAmount> CreateBulky(List<logInvoiceHeaderRemainingAmount> data)
		{
			return pCreateBulky(data);
		}

		public logInvoiceHeaderRemainingAmount Update(logInvoiceHeaderRemainingAmount data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxInvoiceHeaderLogRemainingAmountID)
		{
			pDeleteByPK(trxInvoiceHeaderLogRemainingAmountID);
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
				command.CommandText = "dbo.usplogInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<logInvoiceHeaderRemainingAmount> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<logInvoiceHeaderRemainingAmount> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private logInvoiceHeaderRemainingAmount pGetByPK(int trxInvoiceHeaderLogRemainingAmountID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceHeaderLogRemainingAmountID", trxInvoiceHeaderLogRemainingAmountID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private logInvoiceHeaderRemainingAmount pCreate(logInvoiceHeaderRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceHeaderRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxInvoiceHeaderLogRemainingAmountID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<logInvoiceHeaderRemainingAmount> pCreateBulky(List<logInvoiceHeaderRemainingAmount> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<logInvoiceHeaderRemainingAmount>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private logInvoiceHeaderRemainingAmount pUpdate(logInvoiceHeaderRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceHeaderRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceHeaderLogRemainingAmountID", data.trxInvoiceHeaderLogRemainingAmountID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxInvoiceHeaderLogRemainingAmountID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceHeaderLogRemainingAmountID", trxInvoiceHeaderLogRemainingAmountID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeaderRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}