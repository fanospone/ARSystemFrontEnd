

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.HTBGDWH01.TBGARSystem;
using ARSystem.Domain.Repositories.HTBGDWH01.TBGARSystem;

namespace ARSystem.Domain.Repositories.Repositories.RepositoryCustoms
{
    public class uspvwMonitoringCNInvoiceCustomRepository : vwDashBoardMonitoringCNInvoiceRepository
    {
        public uspvwMonitoringCNInvoiceCustomRepository(DbContext context) : base(context)
        {

        }
        public int GetCountMonitoring(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRMonitoringCNCustom";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        public List<vwDashBoardMonitoringCNInvoice> GetListMonitoring(string strWhereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRMonitoringCNCustom";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", strWhereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
                #region test
                //var record = command.ExecuteReader();
                //List<vwDashBoardMonitoringCNInvoice> list = new List<vwDashBoardMonitoringCNInvoice>();
                //while (record.Read())
                //{
                //    //list.Add(String.Format("{0}_{1}", record["MstBapsId"].ToString(), record["sStartInvoiceDate"].ToString()));
                //    list.Add(new vwDashBoardMonitoringCNInvoice(
                //        record.GetString(record.GetOrdinal("CustomerID")),
                //        record.GetString(record.GetOrdinal("CustomerName")),
                //        record.GetString(record.GetOrdinal("CompanyID")),
                //        record.GetString(record.GetOrdinal("CompanyName")),
                //        record.GetInt32(record.GetOrdinal("OD_13")),
                //        record.GetInt32(record.GetOrdinal("OD_46")),
                //        record.GetInt32(record.GetOrdinal("OD_79")),
                //        record.GetInt32(record.GetOrdinal("OD_9s")),
                //        record.GetInt32(record.GetOrdinal("GrandTotal"))
                //    ));
                //}
                //record.Close();
                //return list;
                #endregion
            }
        }

        public List<vwDashBoardMonitoringCNInvoice> GetPagedMonitoring(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRMonitoringCNCustom";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
    }
}
