
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
	public class logCounterInvoiceHeaderRepository : BaseRepository<logCounterInvoiceHeader>
	{
		private DbContext _context;
		public logCounterInvoiceHeaderRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<logCounterInvoiceHeader> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<logCounterInvoiceHeader> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public logCounterInvoiceHeader GetByPK(long logId)
		{
			return pGetByPK(logId);
		}

		public logCounterInvoiceHeader Create(logCounterInvoiceHeader data)
		{
			return pCreate(data);
		}

		public List<logCounterInvoiceHeader> CreateBulky(List<logCounterInvoiceHeader> data)
		{
			return pCreateBulky(data);
		}

		public logCounterInvoiceHeader Update(logCounterInvoiceHeader data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(long logId)
		{
			pDeleteByPK(logId);
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
				command.CommandText = "dbo.usplogCounterInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<logCounterInvoiceHeader> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<logCounterInvoiceHeader> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private logCounterInvoiceHeader pGetByPK(long logId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@LogId", logId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private logCounterInvoiceHeader pCreate(logCounterInvoiceHeader data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logCounterInvoiceHeader>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.LogId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<logCounterInvoiceHeader> pCreateBulky(List<logCounterInvoiceHeader> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<logCounterInvoiceHeader>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private logCounterInvoiceHeader pUpdate(logCounterInvoiceHeader data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logCounterInvoiceHeader>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@LogId", data.LogId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(long logId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@LogId", logId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}