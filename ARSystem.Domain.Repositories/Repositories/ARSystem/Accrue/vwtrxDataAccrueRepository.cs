
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
	public class vwtrxDataAccrueRepository : BaseRepository<vwtrxDataAccrue>
	{
		private DbContext _context;
		public vwtrxDataAccrueRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<vwtrxDataAccrue> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<vwtrxDataAccrue> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}
        //Custom
        public decimal GetCalculateAmount(vwtrxDataAccrue data)
        {
            
            decimal sum = 0.00m;

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspAccrueCalculateAmmount";

                command.Parameters.Add(command.CreateParameter("@f_CustomerID", data.CustomerID));
                command.Parameters.Add(command.CreateParameter("@f_StartDate", data.StartDateAccrue));
                command.Parameters.Add(command.CreateParameter("@f_EndDate", data.EndDateAccrue));
                command.Parameters.Add(command.CreateParameter("@f_BaseLeasePrice", data.BaseLeasePrice));
                command.Parameters.Add(command.CreateParameter("@f_OMPrice", data.ServicePrice));

                sum = decimal.Parse(command.ExecuteScalar().ToString());
            }
            return sum;
        }
        public string GetAccrueWeekGetDate(DateTime dateInput, string flag)
        {
            string result = string.Empty;
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspAccrueWeekGetDate";
                command.Parameters.Add(command.CreateParameter("@dateParam", dateInput));
                command.Parameters.Add(command.CreateParameter("@flag", flag)); 
                result = command.ExecuteScalar().ToString();
            }
            return result;
        }
        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwtrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<vwtrxDataAccrue> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwtrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<vwtrxDataAccrue> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwtrxDataAccrue";

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