
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
    public class GetListIdRepository : BaseRepository<int>
    {
        private DbContext _context;
        public GetListIdRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        /// <summary>
        /// for Getting List of Id--> used for Checklist all data
        /// </summary>
        /// <param name="GetListIdRepository"></param>
        /// <returns></returns>
        public List<int> GetListId(string ColumnNameId, string SourceTableName, string strWhereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetListId";

                command.Parameters.Add(command.CreateParameter("@vIdName", ColumnNameId));
                command.Parameters.Add(command.CreateParameter("@vSourceTableName", SourceTableName));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", strWhereClause));
                var record = command.ExecuteReader();
                List<int> list = new List<int>();
                while (record.Read())
                {
                    list.Add(int.Parse(record[ColumnNameId].ToString()));
                }
                record.Close();
                return list;
            }
        }
        public List<string> GetListString(string ColumnNameId, string SourceTableName, string strWhereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetListId";

                command.Parameters.Add(command.CreateParameter("@vIdName", ColumnNameId));
                command.Parameters.Add(command.CreateParameter("@vSourceTableName", SourceTableName));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", strWhereClause));
                var record = command.ExecuteReader();
                List<string> list = new List<string>();
                while (record.Read())
                {
                    list.Add(record[ColumnNameId].ToString());
                }
                record.Close();
                return list;
            }
        }
        public List<string> GetListTargetRecurringId(string BapsType, string PowerType, string strWhereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListID"));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", strWhereClause));
                var record = command.ExecuteReader();
                List<string> list = new List<string>();
                while (record.Read())
                {
                    list.Add(String.Format("{0}_{1}", record["MstBapsId"].ToString(), record["sStartInvoiceDate"].ToString()));
                }
                record.Close();
                return list;
            }
        }
    }
}