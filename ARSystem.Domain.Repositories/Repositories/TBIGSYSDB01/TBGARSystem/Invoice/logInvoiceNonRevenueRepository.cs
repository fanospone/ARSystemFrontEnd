
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
	public class logInvoiceNonRevenueRepository : BaseRepository<logInvoiceNonRevenue>
	{
		private DbContext _context;
		public logInvoiceNonRevenueRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<logInvoiceNonRevenue> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<logInvoiceNonRevenue> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public logInvoiceNonRevenue GetByPK(int trxInvoiceNonRevenueLogID)
		{
			return pGetByPK(trxInvoiceNonRevenueLogID);
		}

		public logInvoiceNonRevenue Create(logInvoiceNonRevenue data)
		{
			return pCreate(data);
		}

		public List<logInvoiceNonRevenue> CreateBulky(List<logInvoiceNonRevenue> data)
		{
			return pCreateBulky(data);
		}

		public logInvoiceNonRevenue Update(logInvoiceNonRevenue data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxInvoiceNonRevenueLogID)
		{
			pDeleteByPK(trxInvoiceNonRevenueLogID);
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
				command.CommandText = "dbo.usplogInvoiceNonRevenue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<logInvoiceNonRevenue> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceNonRevenue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<logInvoiceNonRevenue> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceNonRevenue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private logInvoiceNonRevenue pGetByPK(int trxInvoiceNonRevenueLogID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceNonRevenue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceNonRevenueLogID", trxInvoiceNonRevenueLogID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private logInvoiceNonRevenue pCreate(logInvoiceNonRevenue data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceNonRevenue>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceNonRevenue";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxInvoiceNonRevenueLogID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<logInvoiceNonRevenue> pCreateBulky(List<logInvoiceNonRevenue> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<logInvoiceNonRevenue>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceNonRevenue";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private logInvoiceNonRevenue pUpdate(logInvoiceNonRevenue data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceNonRevenue>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceNonRevenue";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceNonRevenueLogID", data.trxInvoiceNonRevenueLogID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxInvoiceNonRevenueLogID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceNonRevenue";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceNonRevenueLogID", trxInvoiceNonRevenueLogID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceNonRevenue";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}