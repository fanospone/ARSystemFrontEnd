
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
	public class vwRABapsSiteRepository : BaseRepository<vwRABapsSite>
	{
		private DbContext _context;
		public vwRABapsSiteRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "", string BapsType="", string PowerType="")
		{
			return pGetCount(whereClause, BapsType, PowerType);
		}

		public List<vwRABapsSite> GetList(string whereClause = "", string orderBy = "", string BapsType="", string PowerType="")
		{
			return pGetList(whereClause, orderBy, BapsType, PowerType);
		}

		public List<vwRABapsSite> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize, string BapsType, string PowerType)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize, BapsType, PowerType);
		}

		#region Private

		private int pGetCount(string whereClause, string BapsType, string PowerType)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRABapsSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.CountTransaction(command);
			}
		}
		private List<vwRABapsSite> pGetList(string whereClause, string orderBy, string BapsType, string PowerType)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRABapsSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));
                

                return this.ReadTransaction(command).ToList();
			}
		}
		private List<vwRABapsSite> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize, string BapsType, string PowerType)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwRABapsSite";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.ReadTransaction(command).ToList();
			}
		}

        #endregion

        #region TAB NEW BAPS
        public int pGetNewBapsCount(string whereClause, string BapsType, string PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetNewBapsCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.CountTransaction(command);
            }
        }
        public List<vwRABapsSite> pGetNewBapsList(string whereClause, string orderBy, string BapsType, string PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetNewBapsList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.ReadTransaction(command).ToList();
            }
        }
        public List<vwRABapsSite> pGetNewBapsPaged(string whereClause, string orderBy, int rowSkip, int pageSize, string BapsType, string PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";
                ///here
                command.Parameters.Add(command.CreateParameter("@vType", "GetNewBapsPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.ReadTransaction(command).ToList();
            }
        }
        public List<string> GetListNewBapsId(string BapsType, string PowerType, string strWhereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListNewBapsID"));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", strWhereClause));
                var record = command.ExecuteReader();
                List<string> list = new List<string>();
                while (record.Read())
                {
                    list.Add(record["SONumber"].ToString());
                }
                record.Close();
                return list;
            }
        }

        #endregion


        public int pGetHistoryCount(string whereClause, string BapsType, string PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetHistoryCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.CountTransaction(command);
            }
        }

        public List<vwRABapsSite> pGetHistoryPaged(string whereClause, string orderBy, int rowSkip, int pageSize, string BapsType, string PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetHistoryPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<vwRABapsSite> pGetHistoryList(string whereClause, string orderBy, string BapsType, string PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetHistoryList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.ReadTransaction(command).ToList();
            }
        }


        #region TAB NEW PRODUCT
        public int GetNewProductCount(string whereClause, string BapsType, string PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCountNewProduct"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.CountTransaction(command);
            }
        }
        public List<vwRABapsSite> GetNewProductList(string whereClause, string orderBy, string BapsType, string PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListNewProduct"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.ReadTransaction(command).ToList();
            }
        }
        public List<vwRABapsSite> GetNewProductPaged(string whereClause, string orderBy, int rowSkip, int pageSize, string BapsType, string PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPagedNewProduct"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.ReadTransaction(command).ToList();
            }
        }
        public List<string> GetListNewProductId(string BapsType, string PowerType, string strWhereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListNewProductID"));
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

        #endregion
        #region update history input target
        public vwRABapsSite GetReccuringHistoryInputTargetByPK(long targetID)
        {
            return pGetReccuringHistoryInputTargetByPK(targetID);
        }
        private vwRABapsSite pGetReccuringHistoryInputTargetByPK(long targetID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetRecurringHistoryByTargetID"));
                command.Parameters.Add(command.CreateParameter("@targetID", targetID));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        public vwRABapsSite GetNewBapsHistoryInputTargetByPK(long targetID)
        {
            return pGetNewBapsHistoryInputTargetByPK(targetID);
        }
        private vwRABapsSite pGetNewBapsHistoryInputTargetByPK(long targetID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRABapsSite";

                command.Parameters.Add(command.CreateParameter("@vType", "GetNewBapsHistoryByTargetID"));
                command.Parameters.Add(command.CreateParameter("@targetID", targetID));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        #endregion
    }
}