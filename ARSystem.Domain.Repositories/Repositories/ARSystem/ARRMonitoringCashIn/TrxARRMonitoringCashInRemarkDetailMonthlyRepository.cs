
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
    public class TrxARRMonitoringCashInRemarkDetailMonthlyRepository : BaseRepository<TrxARRMonitoringCashInRemarkDetailMonthly>
    {
        private DbContext _context;
        public TrxARRMonitoringCashInRemarkDetailMonthlyRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<TrxARRMonitoringCashInRemarkDetailMonthly> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<TrxARRMonitoringCashInRemarkDetailMonthly> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public TrxARRMonitoringCashInRemarkDetailMonthly GetByPK(string operatorID, int periode, int month)
        {
            return pGetByPK(operatorID, periode, month);
        }

        public TrxARRMonitoringCashInRemarkDetailMonthly Create(TrxARRMonitoringCashInRemarkDetailMonthly data)
        {
            return pCreate(data);
        }

        public List<TrxARRMonitoringCashInRemarkDetailMonthly> CreateBulky(List<TrxARRMonitoringCashInRemarkDetailMonthly> data)
        {
            return pCreateBulky(data);
        }

        public TrxARRMonitoringCashInRemarkDetailMonthly Update(TrxARRMonitoringCashInRemarkDetailMonthly data)
        {
            return pUpdate(data);
        }

        public bool DeleteByPK(string operatorID, int periode, int month)
        {
            pDeleteByPK(operatorID, periode, month);
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
                command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailMonthly";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<TrxARRMonitoringCashInRemarkDetailMonthly> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailMonthly";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<TrxARRMonitoringCashInRemarkDetailMonthly> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailMonthly";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private TrxARRMonitoringCashInRemarkDetailMonthly pGetByPK(string operatorID, int periode, int month)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailMonthly";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@OperatorID", operatorID));
                command.Parameters.Add(command.CreateParameter("@Periode", periode));
                command.Parameters.Add(command.CreateParameter("@Month", month));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private TrxARRMonitoringCashInRemarkDetailMonthly pCreate(TrxARRMonitoringCashInRemarkDetailMonthly data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<TrxARRMonitoringCashInRemarkDetailMonthly>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailMonthly";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                this.WriteTransaction(command);

                return data;
            }
        }
        private List<TrxARRMonitoringCashInRemarkDetailMonthly> pCreateBulky(List<TrxARRMonitoringCashInRemarkDetailMonthly> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<TrxARRMonitoringCashInRemarkDetailMonthly>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailMonthly";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private TrxARRMonitoringCashInRemarkDetailMonthly pUpdate(TrxARRMonitoringCashInRemarkDetailMonthly data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<TrxARRMonitoringCashInRemarkDetailMonthly>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailMonthly";

                command.Parameters.Add(command.CreateParameter("@vType", "Update"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@OperatorID", data.OperatorID));
                command.Parameters.Add(command.CreateParameter("@Periode", data.Periode));
                command.Parameters.Add(command.CreateParameter("@Month", data.Month));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private void pDeleteByPK(string operatorID, int periode, int month)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailMonthly";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
                command.Parameters.Add(command.CreateParameter("@OperatorID", operatorID));
                command.Parameters.Add(command.CreateParameter("@Periode", periode));
                command.Parameters.Add(command.CreateParameter("@Month", month));

                command.ExecuteNonQuery();
            }
        }
        private void pDeleteByFilter(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspTrxARRMonitoringCashInRemarkDetailMonthly";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
}