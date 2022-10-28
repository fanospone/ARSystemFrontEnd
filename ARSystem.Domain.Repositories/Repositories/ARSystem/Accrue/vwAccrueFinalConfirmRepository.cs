
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
	public class vwAccrueFinalConfirmRepository : BaseRepository<vwAccrueFinalConfirm>
	{
		private DbContext _context;
		public vwAccrueFinalConfirmRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<vwAccrueFinalConfirm> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<vwAccrueFinalConfirm> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}
        //Custom 
        public List<vwAccrueFinalConfirm> GetListFinalConfirmSummary(int Week, int Month, int Year, int AccrueStatusID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspAccrueGetFinalConfirmSummary";

                command.Parameters.Add(command.CreateParameter("@Week", Week));
                command.Parameters.Add(command.CreateParameter("@Month", Month));
                command.Parameters.Add(command.CreateParameter("@Year", Year));
                command.Parameters.Add(command.CreateParameter("@AccrueStatusID", AccrueStatusID));

                return this.ReadTransaction(command).ToList();
            }
        }
        public DataTable GetListFinalConfirmSummaryDataTable(int Week, int Month, int Year, int AccrueStatusID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspAccrueGetFinalConfirmSummary";

                command.Parameters.Add(command.CreateParameter("@Week", Week));
                command.Parameters.Add(command.CreateParameter("@Month", Month));
                command.Parameters.Add(command.CreateParameter("@Year", Year));
                command.Parameters.Add(command.CreateParameter("@AccrueStatusID", AccrueStatusID));

                return command.ExecuteReader().ToDictionaryDataTable();
            }
        }
        public DataTable GetListFinalConfirmAttendanceUser(int Week, int Month, int Year, int AccrueStatusID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspAccrueAttendanceUser";

                command.Parameters.Add(command.CreateParameter("@Week", Week));
                command.Parameters.Add(command.CreateParameter("@Month", Month));
                command.Parameters.Add(command.CreateParameter("@Year", Year));
                command.Parameters.Add(command.CreateParameter("@AccrueStatusID", AccrueStatusID));

                return command.ExecuteReader().ToDictionaryDataTable();
            }
        }
        public DataTable GetListFinalConfirmSummaryEmailFinanceDataTable(string ID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspAccrueGetFinalConfirmEmail";

                //command.Parameters.Add(command.CreateParameter("@Week", Week));
                //command.Parameters.Add(command.CreateParameter("@Month", Month));
                //command.Parameters.Add(command.CreateParameter("@Year", Year));
                command.Parameters.Add(command.CreateParameter("@ParamID", ID));

                return command.ExecuteReader().ToDictionaryDataTable();
            }
        }
        public DataTable GetDataTableDynamic(string ProcedureName, int Week, int Month, int Year)
        {
            DataTable dt = new DataTable();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = ProcedureName;
                command.Parameters.Clear();

                command.Parameters.Add(command.CreateParameter("@Week", Week));
                command.Parameters.Add(command.CreateParameter("@Month", Month));
                command.Parameters.Add(command.CreateParameter("@Year", Year));

                return command.ExecuteReader().ToDictionaryDataTable();
            }
        }
        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwAccrueFinalConfirm";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<vwAccrueFinalConfirm> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwAccrueFinalConfirm";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<vwAccrueFinalConfirm> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwAccrueFinalConfirm";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		#endregion

	}
}