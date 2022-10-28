
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System.Collections;

namespace ARSystem.Domain.Repositories
{
	public class vwStopAccrueDashboardHeaderRepository : BaseRepository<vwStopAccrueDashboardHeader>
	{
		private DbContext _context;
		public vwStopAccrueDashboardHeaderRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<vwStopAccrueDashboardHeader> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<vwStopAccrueDashboardHeader> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

        public List<vwStopAccrueDashboardHeader> GetListDashboardHeader(string submissionFrom, string submissionTo, string departmentName, int deptOrdetailcase, string detailCase, string status, string DirectorateCode, int rowSkip, int pageSize, string RequestNumber)
        {
            return pGetListDashboardHeader(submissionFrom, submissionTo, departmentName,deptOrdetailcase, detailCase, status, DirectorateCode, rowSkip, pageSize, RequestNumber);
        }

        public List<vwStopAccrueDashboardHeader> GetCountByDepartment(string submissionFrom, string submissionTo, string directorateCode, string status, string AppHeader)
        {
            return pGetCountDataByDepartment(submissionFrom, submissionTo, directorateCode, status, AppHeader);
        }
        public List<vwStopAccrueDashboardHeader> GetCountByDepartmentFinish(string submissionFrom, string submissionTo, string directorateCode, string status, string AppHeader)
        {
            return pGetCountDataByDepartmentFinish(submissionFrom, submissionTo, directorateCode, status, AppHeader);
        }
        public List<vwStopAccrueDashboardHeader> GetCountByDetailCase(string submissionFrom, string submissionTo, string directorateCode, string status, string AppHeader)
        {
            return pGetCountDataByDetailCase(submissionFrom, submissionTo, directorateCode, status, AppHeader);
        }
        public List<vwStopAccrueDashboardHeader> GetCountByDetailCaseFinish(string submissionFrom, string submissionTo, string directorateCode, string status, string AppHeader)
        {
            return pGetCountDataByDetailCaseFinish(submissionFrom, submissionTo, directorateCode, status, AppHeader);
        }
        public List<vwStopAccrueDashboardHeader> GetAppHeaderID(string submissionFrom, string submissionTo, string directorateCode, string status)
        {
            return pGetAppHeaderID(submissionFrom, submissionTo, directorateCode, status);
        }
        #region Private

		private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwStopAccrueDashboardHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<vwStopAccrueDashboardHeader> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwStopAccrueDashboardHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<vwStopAccrueDashboardHeader> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwStopAccrueDashboardHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}

        private List<vwStopAccrueDashboardHeader> pGetListDashboardHeader(string submissionFrom, string submissionTo, string departmentName, int deptOrdetailcase, string detailCase ,string status, string DirectorateCode, int rowSkip, int pageSize, string RequestNumber)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwStopAccrueDashboard";

                command.Parameters.Add(command.CreateParameter("@v_Type", "DashboardHeader"));
                command.Parameters.Add(command.CreateParameter("@submissionDateFrom", submissionFrom));
                command.Parameters.Add(command.CreateParameter("@submissionDateTp", submissionTo));
                command.Parameters.Add(command.CreateParameter("@departName", departmentName));
                command.Parameters.Add(command.CreateParameter("@detailCase", departmentName));
                command.Parameters.Add(command.CreateParameter("@v_Status", status));
                command.Parameters.Add(command.CreateParameter("@deptOrdetailcase", deptOrdetailcase));
                //command.Parameters.Add(command.CreateParameter("@listAppHeader", AppHeader));
                command.Parameters.Add(command.CreateParameter("@directorateCode", DirectorateCode));
                command.Parameters.Add(command.CreateParameter("@start", rowSkip));
                command.Parameters.Add(command.CreateParameter("@lenght", pageSize));
                command.Parameters.Add(command.CreateParameter("@RequestNumber", RequestNumber));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<vwStopAccrueDashboardHeader> pGetCountDataByDepartment(string submissionFrom, string submissionTo, string directorateCode, string status, string AppHeader)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwStopAccrueDashboard";

                command.Parameters.Add(command.CreateParameter("@v_Type", "GetCountByDepartment"));
                command.Parameters.Add(command.CreateParameter("@submissionDateFrom", submissionFrom));
                command.Parameters.Add(command.CreateParameter("@submissionDateTp", submissionTo));
                command.Parameters.Add(command.CreateParameter("@directorateCode", directorateCode));
                command.Parameters.Add(command.CreateParameter("@v_Status", status));
                command.Parameters.Add(command.CreateParameter("@listAppHeader", AppHeader));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<vwStopAccrueDashboardHeader> pGetCountDataByDepartmentFinish(string submissionFrom, string submissionTo, string directorateCode, string status, string AppHeader)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwStopAccrueDashboard";

                command.Parameters.Add(command.CreateParameter("@v_Type", "GetCountByDepartmentFinish"));
                command.Parameters.Add(command.CreateParameter("@submissionDateFrom", submissionFrom));
                command.Parameters.Add(command.CreateParameter("@submissionDateTp", submissionTo));
                command.Parameters.Add(command.CreateParameter("@directorateCode", directorateCode));
                command.Parameters.Add(command.CreateParameter("@v_Status", status));
                command.Parameters.Add(command.CreateParameter("@listAppHeader", AppHeader));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<vwStopAccrueDashboardHeader> pGetCountDataByDetailCase(string submissionFrom, string submissionTo, string directorateCode, string status, string AppHeader)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwStopAccrueDashboard";

                command.Parameters.Add(command.CreateParameter("@v_Type", "GetCountByDetailCase"));
                command.Parameters.Add(command.CreateParameter("@submissionDateFrom", submissionFrom));
                command.Parameters.Add(command.CreateParameter("@submissionDateTp", submissionTo));
                command.Parameters.Add(command.CreateParameter("@directorateCode", directorateCode));
                command.Parameters.Add(command.CreateParameter("@v_Status", status));
                command.Parameters.Add(command.CreateParameter("@listAppHeader", AppHeader));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<vwStopAccrueDashboardHeader> pGetCountDataByDetailCaseFinish(string submissionFrom, string submissionTo, string directorateCode, string status, string AppHeader)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwStopAccrueDashboard";

                command.Parameters.Add(command.CreateParameter("@v_Type", "GetCountByDetailCaseFinish"));
                command.Parameters.Add(command.CreateParameter("@submissionDateFrom", submissionFrom));
                command.Parameters.Add(command.CreateParameter("@submissionDateTp", submissionTo));
                command.Parameters.Add(command.CreateParameter("@directorateCode", directorateCode));
                command.Parameters.Add(command.CreateParameter("@v_Status", status));
                command.Parameters.Add(command.CreateParameter("@listAppHeader", AppHeader));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<vwStopAccrueDashboardHeader> pGetAppHeaderID(string submissionFrom, string submissionTo, string directorateCode, string status)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwStopAccrueDashboard";

                command.Parameters.Add(command.CreateParameter("@v_Type", "GetCountByDetailCase"));
                command.Parameters.Add(command.CreateParameter("@submissionDateFrom", submissionFrom));
                command.Parameters.Add(command.CreateParameter("@submissionDateTp", submissionTo));
                command.Parameters.Add(command.CreateParameter("@directorateCode", directorateCode));
                command.Parameters.Add(command.CreateParameter("@v_Status", status));

                return this.ReadTransaction(command).ToList();
            }
        }
        #endregion

    }
}