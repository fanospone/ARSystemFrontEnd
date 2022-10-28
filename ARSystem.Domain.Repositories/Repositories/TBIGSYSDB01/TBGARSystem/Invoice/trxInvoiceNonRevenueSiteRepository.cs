
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
	public class trxInvoiceNonRevenueSiteRepository : BaseRepository<trxInvoiceNonRevenueSite>
	{
		private DbContext _context;
		public trxInvoiceNonRevenueSiteRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxInvoiceNonRevenueSite> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxInvoiceNonRevenueSite> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxInvoiceNonRevenueSite GetByPK(int trxInvoiceNonRevenueSiteID)
		{
			return pGetByPK(trxInvoiceNonRevenueSiteID);
		}

		public trxInvoiceNonRevenueSite Create(trxInvoiceNonRevenueSite data)
		{
			return pCreate(data);
		}

		public List<trxInvoiceNonRevenueSite> CreateBulky(List<trxInvoiceNonRevenueSite> data)
		{
			return pCreateBulky(data);
		}

		public trxInvoiceNonRevenueSite Update(trxInvoiceNonRevenueSite data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxInvoiceNonRevenueSiteID)
		{
			pDeleteByPK(trxInvoiceNonRevenueSiteID);
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
				command.CommandText = "dbo.usptrxInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxInvoiceNonRevenueSite> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxInvoiceNonRevenueSite> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxInvoiceNonRevenueSite pGetByPK(int trxInvoiceNonRevenueSiteID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceNonRevenueSiteID", trxInvoiceNonRevenueSiteID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxInvoiceNonRevenueSite pCreate(trxInvoiceNonRevenueSite data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoiceNonRevenueSite>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxInvoiceNonRevenueSiteID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxInvoiceNonRevenueSite> pCreateBulky(List<trxInvoiceNonRevenueSite> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxInvoiceNonRevenueSite>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxInvoiceNonRevenueSite pUpdate(trxInvoiceNonRevenueSite data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoiceNonRevenueSite>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceNonRevenueSiteID", data.trxInvoiceNonRevenueSiteID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxInvoiceNonRevenueSiteID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoiceNonRevenueSiteID", trxInvoiceNonRevenueSiteID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoiceNonRevenueSite";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}