
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
	public class trxCNInvoiceNonRevenueSiteRepository : BaseRepository<trxCNInvoiceNonRevenueSite>
	{
		private DbContext _context;
		public trxCNInvoiceNonRevenueSiteRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxCNInvoiceNonRevenueSite> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxCNInvoiceNonRevenueSite> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxCNInvoiceNonRevenueSite GetByPK(int trxCNInvoiceNonRevenueSiteID)
		{
			return pGetByPK(trxCNInvoiceNonRevenueSiteID);
		}

		public trxCNInvoiceNonRevenueSite Create(trxCNInvoiceNonRevenueSite data)
		{
			return pCreate(data);
		}

		public List<trxCNInvoiceNonRevenueSite> CreateBulky(List<trxCNInvoiceNonRevenueSite> data)
		{
			return pCreateBulky(data);
		}

		public trxCNInvoiceNonRevenueSite Update(trxCNInvoiceNonRevenueSite data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxCNInvoiceNonRevenueSiteID)
		{
			pDeleteByPK(trxCNInvoiceNonRevenueSiteID);
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
				command.CommandText = "dbo.usptrxCNInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxCNInvoiceNonRevenueSite> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxCNInvoiceNonRevenueSite> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxCNInvoiceNonRevenueSite pGetByPK(int trxCNInvoiceNonRevenueSiteID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCNInvoiceNonRevenueSiteID", trxCNInvoiceNonRevenueSiteID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxCNInvoiceNonRevenueSite pCreate(trxCNInvoiceNonRevenueSite data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCNInvoiceNonRevenueSite>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxCNInvoiceNonRevenueSiteID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxCNInvoiceNonRevenueSite> pCreateBulky(List<trxCNInvoiceNonRevenueSite> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxCNInvoiceNonRevenueSite>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxCNInvoiceNonRevenueSite pUpdate(trxCNInvoiceNonRevenueSite data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCNInvoiceNonRevenueSite>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxCNInvoiceNonRevenueSiteID", data.trxCNInvoiceNonRevenueSiteID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxCNInvoiceNonRevenueSiteID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCNInvoiceNonRevenueSiteID", trxCNInvoiceNonRevenueSiteID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}