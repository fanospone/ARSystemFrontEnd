
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
	public class trxAllocatePaymentBankInRepository : BaseRepository<trxAllocatePaymentBankIn>
	{
		private DbContext _context;
		public trxAllocatePaymentBankInRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxAllocatePaymentBankIn> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxAllocatePaymentBankIn> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxAllocatePaymentBankIn GetByPK(long trxAllocatePaymentBankInID)
		{
			return pGetByPK(trxAllocatePaymentBankInID);
		}

		public trxAllocatePaymentBankIn Create(trxAllocatePaymentBankIn data)
		{
			return pCreate(data);
		}

		public List<trxAllocatePaymentBankIn> CreateBulky(List<trxAllocatePaymentBankIn> data)
		{
			return pCreateBulky(data);
		}

		public trxAllocatePaymentBankIn Update(trxAllocatePaymentBankIn data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(long trxAllocatePaymentBankInID)
		{
			pDeleteByPK(trxAllocatePaymentBankInID);
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
				command.CommandText = "dbo.usptrxAllocatePaymentBankIn";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxAllocatePaymentBankIn> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankIn";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxAllocatePaymentBankIn> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankIn";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxAllocatePaymentBankIn pGetByPK(long trxAllocatePaymentBankInID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankIn";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxAllocatePaymentBankInID", trxAllocatePaymentBankInID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxAllocatePaymentBankIn pCreate(trxAllocatePaymentBankIn data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxAllocatePaymentBankIn>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankIn";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxAllocatePaymentBankInID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxAllocatePaymentBankIn> pCreateBulky(List<trxAllocatePaymentBankIn> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxAllocatePaymentBankIn>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankIn";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxAllocatePaymentBankIn pUpdate(trxAllocatePaymentBankIn data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxAllocatePaymentBankIn>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankIn";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxAllocatePaymentBankInID", data.trxAllocatePaymentBankInID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(long trxAllocatePaymentBankInID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankIn";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxAllocatePaymentBankInID", trxAllocatePaymentBankInID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxAllocatePaymentBankIn";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}