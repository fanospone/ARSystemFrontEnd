
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
	public class TrxARRMonitoringCashInRemarkDetailQuarterlyRepository : BaseRepository<TrxARRMonitoringCashInRemarkDetailQuarterly>
	{
		private DbContext _context;
		public TrxARRMonitoringCashInRemarkDetailQuarterlyRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<TrxARRMonitoringCashInRemarkDetailQuarterly> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<TrxARRMonitoringCashInRemarkDetailQuarterly> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public TrxARRMonitoringCashInRemarkDetailQuarterly GetByPK(string operatorID, int periode, int quarter)
		{
			return pGetByPK(operatorID, periode, quarter);
		}

		public TrxARRMonitoringCashInRemarkDetailQuarterly Create(TrxARRMonitoringCashInRemarkDetailQuarterly data)
		{
			return pCreate(data);
		}

		public List<TrxARRMonitoringCashInRemarkDetailQuarterly> CreateBulky(List<TrxARRMonitoringCashInRemarkDetailQuarterly> data)
		{
			return pCreateBulky(data);
		}

		public TrxARRMonitoringCashInRemarkDetailQuarterly Update(TrxARRMonitoringCashInRemarkDetailQuarterly data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(string operatorID, int periode, int quarter)
		{
			pDeleteByPK(operatorID, periode, quarter);
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
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailQuarterly";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<TrxARRMonitoringCashInRemarkDetailQuarterly> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailQuarterly";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<TrxARRMonitoringCashInRemarkDetailQuarterly> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailQuarterly";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private TrxARRMonitoringCashInRemarkDetailQuarterly pGetByPK(string operatorID, int periode, int quarter)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailQuarterly";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@OperatorID", operatorID));
				command.Parameters.Add(command.CreateParameter("@Periode", periode));
				command.Parameters.Add(command.CreateParameter("@Quarter", quarter));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private TrxARRMonitoringCashInRemarkDetailQuarterly pCreate(TrxARRMonitoringCashInRemarkDetailQuarterly data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<TrxARRMonitoringCashInRemarkDetailQuarterly>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailQuarterly";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				this.WriteTransaction(command);

				return data;
			}
		}
		private List<TrxARRMonitoringCashInRemarkDetailQuarterly> pCreateBulky(List<TrxARRMonitoringCashInRemarkDetailQuarterly> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<TrxARRMonitoringCashInRemarkDetailQuarterly>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailQuarterly";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private TrxARRMonitoringCashInRemarkDetailQuarterly pUpdate(TrxARRMonitoringCashInRemarkDetailQuarterly data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<TrxARRMonitoringCashInRemarkDetailQuarterly>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailQuarterly";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@OperatorID", data.OperatorID));
				command.Parameters.Add(command.CreateParameter("@Periode", data.Periode));
				command.Parameters.Add(command.CreateParameter("@Quarter", data.Quarter));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(string operatorID, int periode, int quarter)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailQuarterly";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@OperatorID", operatorID));
				command.Parameters.Add(command.CreateParameter("@Periode", periode));
				command.Parameters.Add(command.CreateParameter("@Quarter", quarter));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailQuarterly";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}