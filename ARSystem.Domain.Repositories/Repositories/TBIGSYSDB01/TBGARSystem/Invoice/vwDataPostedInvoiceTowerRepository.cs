
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
	public class vwDataPostedInvoiceTowerRepository : BaseRepository<vwDataPostedInvoiceTower>
	{
		private DbContext _context;
		public vwDataPostedInvoiceTowerRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<vwDataPostedInvoiceTower> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<vwDataPostedInvoiceTower> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

        public string GetTerbilang(decimal amount, string currency)
        {
            return pGetTerbilang(amount,currency);
        }

        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwDataPostedInvoiceTower";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<vwDataPostedInvoiceTower> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwDataPostedInvoiceTower";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<vwDataPostedInvoiceTower> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwDataPostedInvoiceTower";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}

        private string pGetTerbilang(decimal amount, string currency)
        {
            
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "select dbo.ufnTerbilangWithCurrency(ROUND("+ amount+",0), '"+ currency + "') as Result";
                return command.ExecuteReader().DataToString();
            }
        }
		#endregion

	}
}