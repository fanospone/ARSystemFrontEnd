
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
	public class logInvoiceHeaderRepository : BaseRepository<logInvoiceHeader>
	{
		private DbContext _context;
		public logInvoiceHeaderRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<logInvoiceHeader> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<logInvoiceHeader> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public logInvoiceHeader GetByPK(int trxInvoiceHeaderLogID)
		{
			return pGetByPK(trxInvoiceHeaderLogID);
		}

		public logInvoiceHeader Create(logInvoiceHeader data)
		{
			return pCreate(data);
		}

		public List<logInvoiceHeader> CreateBulky(List<logInvoiceHeader> data)
		{
			return pCreateBulky(data);
		}

		public logInvoiceHeader Update(logInvoiceHeader data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxInvoiceHeaderLogID)
		{
			pDeleteByPK(trxInvoiceHeaderLogID);
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
				command.CommandText = "dbo.usplogInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<logInvoiceHeader> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<logInvoiceHeader> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private logInvoiceHeader pGetByPK(int trxInvoiceHeaderLogID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceHeaderLogID", trxInvoiceHeaderLogID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private logInvoiceHeader pCreate(logInvoiceHeader data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceHeader>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxInvoiceHeaderLogID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<logInvoiceHeader> pCreateBulky(List<logInvoiceHeader> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<logInvoiceHeader>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private logInvoiceHeader pUpdate(logInvoiceHeader data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceHeader>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceHeaderLogID", data.trxInvoiceHeaderLogID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxInvoiceHeaderLogID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceHeaderLogID", trxInvoiceHeaderLogID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}