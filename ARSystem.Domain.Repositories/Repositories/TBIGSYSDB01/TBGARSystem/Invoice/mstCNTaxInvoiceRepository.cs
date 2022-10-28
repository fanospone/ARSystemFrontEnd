
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
	public class mstCNTaxInvoiceRepository : BaseRepository<mstCNTaxInvoice>
	{
		private DbContext _context;
		public mstCNTaxInvoiceRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<mstCNTaxInvoice> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<mstCNTaxInvoice> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public mstCNTaxInvoice GetByPK(int cNTaxInvoiceID)
		{
			return pGetByPK(cNTaxInvoiceID);
		}

		public mstCNTaxInvoice Create(mstCNTaxInvoice data)
		{
			return pCreate(data);
		}

		public List<mstCNTaxInvoice> CreateBulky(List<mstCNTaxInvoice> data)
		{
			return pCreateBulky(data);
		}

		public mstCNTaxInvoice Update(mstCNTaxInvoice data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int cNTaxInvoiceID)
		{
			pDeleteByPK(cNTaxInvoiceID);
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
				command.CommandText = "dbo.uspmstCNTaxInvoice";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<mstCNTaxInvoice> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstCNTaxInvoice";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<mstCNTaxInvoice> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstCNTaxInvoice";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private mstCNTaxInvoice pGetByPK(int cNTaxInvoiceID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstCNTaxInvoice";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@CNTaxInvoiceID", cNTaxInvoiceID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private mstCNTaxInvoice pCreate(mstCNTaxInvoice data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstCNTaxInvoice>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstCNTaxInvoice";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.CNTaxInvoiceID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<mstCNTaxInvoice> pCreateBulky(List<mstCNTaxInvoice> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<mstCNTaxInvoice>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstCNTaxInvoice";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private mstCNTaxInvoice pUpdate(mstCNTaxInvoice data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstCNTaxInvoice>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstCNTaxInvoice";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@CNTaxInvoiceID", data.CNTaxInvoiceID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int cNTaxInvoiceID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstCNTaxInvoice";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@CNTaxInvoiceID", cNTaxInvoiceID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstCNTaxInvoice";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}