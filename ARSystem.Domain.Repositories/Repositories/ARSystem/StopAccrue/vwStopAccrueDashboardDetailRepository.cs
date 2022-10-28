
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
	public class vwStopAccrueDashboardDetailRepository : BaseRepository<vwStopAccrueDashboardDetail>
	{
		private DbContext _context;
		public vwStopAccrueDashboardDetailRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<vwStopAccrueDashboardDetail> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<vwStopAccrueDashboardDetail> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

        public List<vwStopAccrueDashboardDetail> GetListdashboardDetail(long? HeaderID, int rowSkip, int pageSize, string detailCase, string departName, string deptOrdetailcase, string requestNumber)
        {
            return pGetListDashboardDetail(HeaderID, rowSkip, pageSize, detailCase, departName, deptOrdetailcase, requestNumber);
        }

        public List<vwStopAccrueDashboardDetail> GetListExportAllDashboardDetail(string submissionDateFrom, string submissionDateTo, string directorateCode, string AccrueType, string listAppHeaderID, string Activity)
        {
            return pGetListExportAllDashboardDetail(submissionDateFrom, submissionDateTo, directorateCode, AccrueType, listAppHeaderID, Activity);
        }

        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwStopAccrueDashboardDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<vwStopAccrueDashboardDetail> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwStopAccrueDashboardDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<vwStopAccrueDashboardDetail> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwStopAccrueDashboardDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}

        private List<vwStopAccrueDashboardDetail> pGetListDashboardDetail(long? HeaderID, int rowSkip, int pageSize, string detailCase, string departName, string deptOrdetailcase, string requestNumber)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwStopAccrueDashboard";

                command.Parameters.Add(command.CreateParameter("@v_Type", "DashboardDetail"));
                command.Parameters.Add(command.CreateParameter("@headerID", HeaderID ));
                command.Parameters.Add(command.CreateParameter("@start", rowSkip));
                command.Parameters.Add(command.CreateParameter("@lenght", pageSize));
                command.Parameters.Add(command.CreateParameter("@deptOrdetailcase", deptOrdetailcase));
                if (deptOrdetailcase == "1")
                {
                    command.Parameters.Add(command.CreateParameter("@departName", departName));
                }
                else
                {
                    command.Parameters.Add(command.CreateParameter("@detailCase", departName));
                }
                command.Parameters.Add(command.CreateParameter("@RequestNumber", requestNumber));

                return this.ReadTransaction(command).ToList();
            }
        }
        
        public List<vwStopAccrueDashboardDetail> pGetListExportAllDashboardDetail(string submissionDateFrom, string submissionDateTo, string directorateCode, string AccrueType, string listAppHeaderID, string Activity)
        {
            if (directorateCode == "" || directorateCode == "null")
            {
                directorateCode = "undefined";
            }

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwStopAccrueDashboard";

                command.Parameters.Add(command.CreateParameter("@v_Type", "ExportAllDashboardDetail"));
                command.Parameters.Add(command.CreateParameter("@listAppHeader", listAppHeaderID));
                command.Parameters.Add(command.CreateParameter("@submissionDateFrom", submissionDateFrom));
                command.Parameters.Add(command.CreateParameter("@submissionDateTp", submissionDateTo));
                command.Parameters.Add(command.CreateParameter("@directorateCode", directorateCode));
                command.Parameters.Add(command.CreateParameter("@v_Status", AccrueType));
                command.Parameters.Add(command.CreateParameter("@Activity", Activity));
                //command.Parameters.Add(command.CreateParameter("@GroupBy", GroupBy));

                return this.ReadTransaction(command).ToList();
            }
        }

        #endregion

    }
}