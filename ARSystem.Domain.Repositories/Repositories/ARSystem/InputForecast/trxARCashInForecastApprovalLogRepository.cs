
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystemFrontEnd.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystemFrontEnd.Domain.Repositories
{
	public class trxARCashInForecastApprovalLogRepository : BaseRepository<trxARCashInForecastApprovalLog>
	{
		private DbContext _context;
		public trxARCashInForecastApprovalLogRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxARCashInForecastApprovalLog> GetApprovalLogByPeriod(int? year, int? quarter, int? month, string forecastType)
		{
			return pGetApprovalLogByPeriod(year, quarter, month, forecastType);
		}

		public List<trxARCashInForecastApprovalLog> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxARCashInForecastApprovalLog GetByPK(int iD)
		{
			return pGetByPK(iD);
		}

		public trxARCashInForecastApprovalLog Create(trxARCashInForecastApprovalLog data)
		{
			return pCreate(data);
		}

		public List<trxARCashInForecastApprovalLog> CreateBulky(List<trxARCashInForecastApprovalLog> data)
		{
			return pCreateBulky(data);
		}

		public trxARCashInForecastApprovalLog Update(trxARCashInForecastApprovalLog data)
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
				command.CommandText = "dbo.usptrxARCashInForecastApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxARCashInForecastApprovalLog> pGetApprovalLogByPeriod(int? year, int? quarter, int? month, string forecastType)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetApprovalLogbyOperatorAndPeriod"));
				command.Parameters.Add(command.CreateParameter("@vTransactionFor", forecastType));
				command.Parameters.Add(command.CreateParameter("@vYear", year));
				command.Parameters.Add(command.CreateParameter("@vQuarter", quarter));
                command.Parameters.Add(command.CreateParameter("@vMonth", month));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxARCashInForecastApprovalLog> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxARCashInForecastApprovalLog pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxARCashInForecastApprovalLog pCreate(trxARCashInForecastApprovalLog data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxARCashInForecastApprovalLog>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxARCashInForecastApprovalLog> pCreateBulky(List<trxARCashInForecastApprovalLog> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxARCashInForecastApprovalLog>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxARCashInForecastApprovalLog pUpdate(trxARCashInForecastApprovalLog data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxARCashInForecastApprovalLog>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARCashInForecastApprovalLog";

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
				command.CommandText = "dbo.usptrxARCashInForecastApprovalLog";

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
				command.CommandText = "dbo.usptrxARCashInForecastApprovalLog";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}