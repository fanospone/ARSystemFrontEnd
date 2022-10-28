
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
    public class FilterARRMonitoringCashInRemarkRepositoryCustoms :BaseRepository<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel>
    {
        private DbContext _context;

        public FilterARRMonitoringCashInRemarkRepositoryCustoms(DbContext context) : base(context)
        {
            _context = context;
        }
        public DataTable GetFilterList(string Type, int Year, int Month, int Week)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetFilterARRMonitoringCashInRemark";
                command.Parameters.Add(command.CreateParameter("@vType", Type));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vWeek", Week));
                var data = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                return dt;
            }
        }

        public DataTable SubmitMonth(string Type, int Year, int Month, string UserID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspSubmitARRMonitoringCashInRemark";
                command.Parameters.Add(command.CreateParameter("@vType", Type));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vUserID", UserID));
                
                var data = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                return dt;
            }
        }
        public DataTable ApproveMonth(string Type, int Year, int Month, string Userid, int Isapproval, string Remark)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspApprovalARRMonitoringCashInRemark";
                command.Parameters.Add(command.CreateParameter("@vType", Type));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vUserID", Userid));
                command.Parameters.Add(command.CreateParameter("@IsApproved", Isapproval));
                command.Parameters.Add(command.CreateParameter("@vRemark", Remark));
                var data = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                return dt;
            }
        }
        public DataTable SubmitWeek(string Type, int Year, int Month, int week, string UserID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspSubmitARRMonitoringCashInRemark";
                command.Parameters.Add(command.CreateParameter("@vType", Type));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vWeek", week));
                command.Parameters.Add(command.CreateParameter("@vUserID", UserID));

                var data = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                return dt;
            }
        }

        public DataTable SubmitQuarter(string Type, int Year, int Quarter, string UserID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspSubmitARRMonitoringCashInRemark";
                command.Parameters.Add(command.CreateParameter("@vType", Type));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vQuarter", Quarter));
        
                command.Parameters.Add(command.CreateParameter("@vUserID", UserID));

                var data = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                return dt;
            }
        }
        public DataTable ApproveWeek(string Type, int Year, int Month, int Week, string Userid, int Isapproval, string Remark)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspApprovalARRMonitoringCashInRemark";
                command.Parameters.Add(command.CreateParameter("@vType", Type));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vWeek", Week));
                command.Parameters.Add(command.CreateParameter("@vUserID", Userid));
                command.Parameters.Add(command.CreateParameter("@IsApproved", Isapproval));
                command.Parameters.Add(command.CreateParameter("@vRemark", Remark));
                var data = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                return dt;
            }
        }

        public DataTable ApproveQuarter(string Type, int Year, int Quarter, string Userid, int Isapproval, string Remark)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspApprovalARRMonitoringCashInRemark";
                command.Parameters.Add(command.CreateParameter("@vType", Type));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vQuarter", Quarter));
 
                command.Parameters.Add(command.CreateParameter("@vUserID", Userid));
                command.Parameters.Add(command.CreateParameter("@IsApproved", Isapproval));
                command.Parameters.Add(command.CreateParameter("@vRemark", Remark));
                var data = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                return dt;
            }
        }
    }
}
