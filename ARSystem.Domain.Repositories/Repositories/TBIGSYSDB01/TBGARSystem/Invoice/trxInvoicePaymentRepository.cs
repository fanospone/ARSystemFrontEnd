
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
	public class trxInvoicePaymentRepository : BaseRepository<trxInvoicePayment>
	{
		private DbContext _context;
		public trxInvoicePaymentRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxInvoicePayment> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxInvoicePayment> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxInvoicePayment GetByPK(int trxInvoicePaymentId)
		{
			return pGetByPK(trxInvoicePaymentId);
		}

		public trxInvoicePayment Create(trxInvoicePayment data)
		{
			return pCreate(data);
		}

		public List<trxInvoicePayment> CreateBulky(List<trxInvoicePayment> data)
		{
			return pCreateBulky(data);
		}

		public trxInvoicePayment Update(trxInvoicePayment data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxInvoicePaymentId)
		{
			pDeleteByPK(trxInvoicePaymentId);
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
				command.CommandText = "dbo.usptrxInvoicePayment";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxInvoicePayment> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoicePayment";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxInvoicePayment> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoicePayment";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxInvoicePayment pGetByPK(int trxInvoicePaymentId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoicePayment";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoicePaymentId", trxInvoicePaymentId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxInvoicePayment pCreate(trxInvoicePayment data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoicePayment>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoicePayment";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxInvoicePaymentId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxInvoicePayment> pCreateBulky(List<trxInvoicePayment> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxInvoicePayment>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoicePayment";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxInvoicePayment pUpdate(trxInvoicePayment data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxInvoicePayment>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoicePayment";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxInvoicePaymentId", data.trxInvoicePaymentId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxInvoicePaymentId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoicePayment";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxInvoicePaymentId", trxInvoicePaymentId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxInvoicePayment";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}