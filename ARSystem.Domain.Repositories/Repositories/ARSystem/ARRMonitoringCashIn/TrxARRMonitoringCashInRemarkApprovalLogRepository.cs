
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
	public class TrxARRMonitoringCashInRemarkApprovalLogRepository : BaseRepository<TrxARRMonitoringCashInRemarkApprovalLog>
	{
		private DbContext _context;
		public TrxARRMonitoringCashInRemarkApprovalLogRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<TrxARRMonitoringCashInRemarkApprovalLog> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<TrxARRMonitoringCashInRemarkApprovalLog> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public TrxARRMonitoringCashInRemarkApprovalLog GetByPK(int iD)
		{
			return pGetByPK(iD);
		}

		public TrxARRMonitoringCashInRemarkApprovalLog Create(TrxARRMonitoringCashInRemarkApprovalLog data)
		{
			return pCreate(data);
		}

		public List<TrxARRMonitoringCashInRemarkApprovalLog> CreateBulky(List<TrxARRMonitoringCashInRemarkApprovalLog> data)
		{
			return pCreateBulky(data);
		}

		public TrxARRMonitoringCashInRemarkApprovalLog Update(TrxARRMonitoringCashInRemarkApprovalLog data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int iD)
		{
			pDeleteByPK(iD);
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
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<TrxARRMonitoringCashInRemarkApprovalLog> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<TrxARRMonitoringCashInRemarkApprovalLog> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private TrxARRMonitoringCashInRemarkApprovalLog pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private TrxARRMonitoringCashInRemarkApprovalLog pCreate(TrxARRMonitoringCashInRemarkApprovalLog data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<TrxARRMonitoringCashInRemarkApprovalLog>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<TrxARRMonitoringCashInRemarkApprovalLog> pCreateBulky(List<TrxARRMonitoringCashInRemarkApprovalLog> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<TrxARRMonitoringCashInRemarkApprovalLog>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private TrxARRMonitoringCashInRemarkApprovalLog pUpdate(TrxARRMonitoringCashInRemarkApprovalLog data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<TrxARRMonitoringCashInRemarkApprovalLog>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ID", data.ID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}