
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
	public class trxRAPurchaseOrderRepository : BaseRepository<trxRAPurchaseOrder>
	{
		private DbContext _context;
		public trxRAPurchaseOrderRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxRAPurchaseOrder> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxRAPurchaseOrder> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxRAPurchaseOrder GetByPK(long iD)
		{
			return pGetByPK(iD);
		}

		public trxRAPurchaseOrder Create(trxRAPurchaseOrder data, List<trxRAPurchaseOrderDetail> PoDetail)
		{
			return pCreate(data,PoDetail);
		}

		public List<trxRAPurchaseOrder> CreateBulky(List<trxRAPurchaseOrder> data)
		{
			return pCreateBulky(data);
		}

		public trxRAPurchaseOrder Update(trxRAPurchaseOrder data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(long iD)
		{
			pDeleteByPK(iD);
			return true;
		}

		public bool DeleteByFilter(string whereClause)
		{
			pDeleteByFilter(whereClause);
			return true;
		}

        public int UpdateNextActivity(List<trxRAPurchaseOrder> ID, int Activity)
        {
            return pUpdateNextActivity(ID, Activity);
        }

        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrder";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxRAPurchaseOrder> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrder";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxRAPurchaseOrder> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrder";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxRAPurchaseOrder pGetByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrder";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxRAPurchaseOrder pCreate(trxRAPurchaseOrder data, List<trxRAPurchaseOrderDetail> PoDetail)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxRAPurchaseOrder>(data);
                string xmlDetail = Helper.XmlSerializer<List<trxRAPurchaseOrderDetail>>(PoDetail);

                command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrder";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@vXmlDetail", xmlDetail));

                data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxRAPurchaseOrder> pCreateBulky(List<trxRAPurchaseOrder> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxRAPurchaseOrder>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrder";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxRAPurchaseOrder pUpdate(trxRAPurchaseOrder data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxRAPurchaseOrder>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrder";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ID", data.ID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrder";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAPurchaseOrder";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
        private int pUpdateNextActivity(List<trxRAPurchaseOrder> ID,int Activity)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxRAPurchaseOrder>>(ID);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRAPurchaseOrder";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateNextActivity"));
                command.Parameters.Add(command.CreateParameter("@Activity", Activity));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                return command.ExecuteNonQuery();
            }
        }

        #endregion

    }
}