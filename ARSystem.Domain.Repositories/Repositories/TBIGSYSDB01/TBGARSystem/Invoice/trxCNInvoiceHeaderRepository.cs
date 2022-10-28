
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
	public class trxCNInvoiceHeaderRepository : BaseRepository<trxCNInvoiceHeader>
	{
		private DbContext _context;
		public trxCNInvoiceHeaderRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxCNInvoiceHeader> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxCNInvoiceHeader> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxCNInvoiceHeader GetByPK(int trxCNInvoiceHeaderID)
		{
			return pGetByPK(trxCNInvoiceHeaderID);
		}

		public trxCNInvoiceHeader Create(trxCNInvoiceHeader data)
		{
			return pCreate(data);
		}

		public List<trxCNInvoiceHeader> CreateBulky(List<trxCNInvoiceHeader> data)
		{
			return pCreateBulky(data);
		}

		public trxCNInvoiceHeader Update(trxCNInvoiceHeader data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxCNInvoiceHeaderID)
		{
			pDeleteByPK(trxCNInvoiceHeaderID);
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
				command.CommandText = "dbo.usptrxCNInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxCNInvoiceHeader> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxCNInvoiceHeader> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxCNInvoiceHeader pGetByPK(int trxCNInvoiceHeaderID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCNInvoiceHeaderID", trxCNInvoiceHeaderID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxCNInvoiceHeader pCreate(trxCNInvoiceHeader data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCNInvoiceHeader>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxCNInvoiceHeaderID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxCNInvoiceHeader> pCreateBulky(List<trxCNInvoiceHeader> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxCNInvoiceHeader>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxCNInvoiceHeader pUpdate(trxCNInvoiceHeader data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCNInvoiceHeader>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxCNInvoiceHeaderID", data.trxCNInvoiceHeaderID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxCNInvoiceHeaderID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCNInvoiceHeaderID", trxCNInvoiceHeaderID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}