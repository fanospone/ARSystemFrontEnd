
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
	public class logInvoiceTowerDetailRemainingAmountRepository : BaseRepository<logInvoiceTowerDetailRemainingAmount>
	{
		private DbContext _context;
		public logInvoiceTowerDetailRemainingAmountRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<logInvoiceTowerDetailRemainingAmount> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<logInvoiceTowerDetailRemainingAmount> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public logInvoiceTowerDetailRemainingAmount GetByPK(int trxInvoiceTowerDetailLogRemainingAmountId)
		{
			return pGetByPK(trxInvoiceTowerDetailLogRemainingAmountId);
		}

		public logInvoiceTowerDetailRemainingAmount Create(logInvoiceTowerDetailRemainingAmount data)
		{
			return pCreate(data);
		}

		public List<logInvoiceTowerDetailRemainingAmount> CreateBulky(List<logInvoiceTowerDetailRemainingAmount> data)
		{
			return pCreateBulky(data);
		}

		public logInvoiceTowerDetailRemainingAmount Update(logInvoiceTowerDetailRemainingAmount data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxInvoiceTowerDetailLogRemainingAmountId)
		{
			pDeleteByPK(trxInvoiceTowerDetailLogRemainingAmountId);
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
				command.CommandText = "dbo.usplogInvoiceTowerDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<logInvoiceTowerDetailRemainingAmount> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceTowerDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<logInvoiceTowerDetailRemainingAmount> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceTowerDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private logInvoiceTowerDetailRemainingAmount pGetByPK(int trxInvoiceTowerDetailLogRemainingAmountId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceTowerDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceTowerDetailLogRemainingAmountId", trxInvoiceTowerDetailLogRemainingAmountId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private logInvoiceTowerDetailRemainingAmount pCreate(logInvoiceTowerDetailRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceTowerDetailRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceTowerDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxInvoiceTowerDetailLogRemainingAmountId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<logInvoiceTowerDetailRemainingAmount> pCreateBulky(List<logInvoiceTowerDetailRemainingAmount> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<logInvoiceTowerDetailRemainingAmount>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceTowerDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private logInvoiceTowerDetailRemainingAmount pUpdate(logInvoiceTowerDetailRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceTowerDetailRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceTowerDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceTowerDetailLogRemainingAmountId", data.trxInvoiceTowerDetailLogRemainingAmountId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxInvoiceTowerDetailLogRemainingAmountId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceTowerDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceTowerDetailLogRemainingAmountId", trxInvoiceTowerDetailLogRemainingAmountId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceTowerDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}