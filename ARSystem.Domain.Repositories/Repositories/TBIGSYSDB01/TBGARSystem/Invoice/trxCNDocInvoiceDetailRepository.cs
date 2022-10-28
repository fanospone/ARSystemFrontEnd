
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
	public class trxCNDocInvoiceDetailRepository : BaseRepository<trxCNDocInvoiceDetail>
	{
		private DbContext _context;
		public trxCNDocInvoiceDetailRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxCNDocInvoiceDetail> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxCNDocInvoiceDetail> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxCNDocInvoiceDetail GetByPK(int trxCNDocInvoiceDetailID)
		{
			return pGetByPK(trxCNDocInvoiceDetailID);
		}

		public trxCNDocInvoiceDetail Create(trxCNDocInvoiceDetail data)
		{
			return pCreate(data);
		}

		public List<trxCNDocInvoiceDetail> CreateBulky(List<trxCNDocInvoiceDetail> data)
		{
			return pCreateBulky(data);
		}

		public trxCNDocInvoiceDetail Update(trxCNDocInvoiceDetail data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxCNDocInvoiceDetailID)
		{
			pDeleteByPK(trxCNDocInvoiceDetailID);
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
				command.CommandText = "dbo.usptrxCNDocInvoiceDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxCNDocInvoiceDetail> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNDocInvoiceDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxCNDocInvoiceDetail> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNDocInvoiceDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxCNDocInvoiceDetail pGetByPK(int trxCNDocInvoiceDetailID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNDocInvoiceDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCNDocInvoiceDetailID", trxCNDocInvoiceDetailID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxCNDocInvoiceDetail pCreate(trxCNDocInvoiceDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCNDocInvoiceDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNDocInvoiceDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxCNDocInvoiceDetailID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxCNDocInvoiceDetail> pCreateBulky(List<trxCNDocInvoiceDetail> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxCNDocInvoiceDetail>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNDocInvoiceDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxCNDocInvoiceDetail pUpdate(trxCNDocInvoiceDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxCNDocInvoiceDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNDocInvoiceDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxCNDocInvoiceDetailID", data.trxCNDocInvoiceDetailID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxCNDocInvoiceDetailID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNDocInvoiceDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxCNDocInvoiceDetailID", trxCNDocInvoiceDetailID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxCNDocInvoiceDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}