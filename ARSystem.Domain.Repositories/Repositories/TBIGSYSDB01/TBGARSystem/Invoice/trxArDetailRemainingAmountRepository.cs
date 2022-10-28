
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
	public class trxArDetailRemainingAmountRepository : BaseRepository<trxArDetailRemainingAmount>
	{
		private DbContext _context;
		public trxArDetailRemainingAmountRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxArDetailRemainingAmount> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxArDetailRemainingAmount> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxArDetailRemainingAmount GetByPK(int trxArDetailRemainingAmountId)
		{
			return pGetByPK(trxArDetailRemainingAmountId);
		}

		public trxArDetailRemainingAmount Create(trxArDetailRemainingAmount data)
		{
			return pCreate(data);
		}

		public List<trxArDetailRemainingAmount> CreateBulky(List<trxArDetailRemainingAmount> data)
		{
			return pCreateBulky(data);
		}

		public trxArDetailRemainingAmount Update(trxArDetailRemainingAmount data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int trxArDetailRemainingAmountId)
		{
			pDeleteByPK(trxArDetailRemainingAmountId);
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
				command.CommandText = "dbo.usptrxArDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxArDetailRemainingAmount> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxArDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxArDetailRemainingAmount> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxArDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxArDetailRemainingAmount pGetByPK(int trxArDetailRemainingAmountId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxArDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@trxArDetailRemainingAmountId", trxArDetailRemainingAmountId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxArDetailRemainingAmount pCreate(trxArDetailRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxArDetailRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxArDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.trxArDetailRemainingAmountId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxArDetailRemainingAmount> pCreateBulky(List<trxArDetailRemainingAmount> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxArDetailRemainingAmount>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxArDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxArDetailRemainingAmount pUpdate(trxArDetailRemainingAmount data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxArDetailRemainingAmount>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxArDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@trxArDetailRemainingAmountId", data.trxArDetailRemainingAmountId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int trxArDetailRemainingAmountId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxArDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@trxArDetailRemainingAmountId", trxArDetailRemainingAmountId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxArDetailRemainingAmount";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}