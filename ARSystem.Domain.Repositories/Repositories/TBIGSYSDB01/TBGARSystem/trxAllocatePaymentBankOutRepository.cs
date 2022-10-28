
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
	public class trxAllocatePaymentBankOutRepository : BaseRepository<trxAllocatePaymentBankOut>
	{
		private DbContext _context;
		public trxAllocatePaymentBankOutRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxAllocatePaymentBankOut> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxAllocatePaymentBankOut> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxAllocatePaymentBankOut GetByPK(long trxAllocatePaymentBankOutID)
		{
			return pGetByPK(trxAllocatePaymentBankOutID);
		}

		public trxAllocatePaymentBankOut Create(trxAllocatePaymentBankOut data)
		{
			return pCreate(data);
		}

		public List<trxAllocatePaymentBankOut> CreateBulky(List<trxAllocatePaymentBankOut> data)
		{
			return pCreateBulky(data);
		}

		public trxAllocatePaymentBankOut Update(trxAllocatePaymentBankOut data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(long trxAllocatePaymentBankOutID)
		{
			pDeleteByPK(trxAllocatePaymentBankOutID);
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
				command.CommandText = "dbo.usptrxAllocatePaymentBankOut";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxAllocatePaymentBankOut> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankOut";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxAllocatePaymentBankOut> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankOut";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxAllocatePaymentBankOut pGetByPK(long trxAllocatePaymentBankOutID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankOut";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxAllocatePaymentBankOutID", trxAllocatePaymentBankOutID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxAllocatePaymentBankOut pCreate(trxAllocatePaymentBankOut data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxAllocatePaymentBankOut>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankOut";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxAllocatePaymentBankOutID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxAllocatePaymentBankOut> pCreateBulky(List<trxAllocatePaymentBankOut> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxAllocatePaymentBankOut>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankOut";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxAllocatePaymentBankOut pUpdate(trxAllocatePaymentBankOut data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxAllocatePaymentBankOut>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankOut";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxAllocatePaymentBankOutID", data.trxAllocatePaymentBankOutID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(long trxAllocatePaymentBankOutID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankOut";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxAllocatePaymentBankOutID", trxAllocatePaymentBankOutID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankOut";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}