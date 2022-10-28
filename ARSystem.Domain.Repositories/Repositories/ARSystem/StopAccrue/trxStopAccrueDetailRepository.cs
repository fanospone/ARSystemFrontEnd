
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
	public class trxStopAccrueDetailRepository : BaseRepository<trxStopAccrueDetail>
	{
		private DbContext _context;
		public trxStopAccrueDetailRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxStopAccrueDetail> GetList(string whereClause = "", string orderBy="")
		{
			return pGetList(whereClause, orderBy);
		}
        public List<trxStopAccrueDetail> DeleteDraft(string whereClause = "")
        {
            return pDeleteDraft(whereClause);
        }


        public List<trxStopAccrueDetail> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxStopAccrueDetail GetByPK(long iD)
		{
			return pGetByPK(iD);
		}

		public trxStopAccrueDetail Create(trxStopAccrueDetail data)
		{
			return pCreate(data);
		}

		public List<trxStopAccrueDetail> CreateBulky(List<trxStopAccrueDetail> data)
		{
			return pCreateBulky(data);
		}
        public List<trxStopAccrueDetail> CreateBulkyDraft(List<trxStopAccrueDetail> data)
        {
            return pCreateBulkyDraft(data);
        }
        public List<trxStopAccrueDetail> CreateBulkyEdit(List<trxStopAccrueDetail> data)
        {
            return pCreateBulkyEdit(data);
        }
        public trxStopAccrueDetail Update(trxStopAccrueDetail data)
		{
			return pUpdate(data);
		}

        public trxStopAccrueDetail UpdateCapexRevenue(trxStopAccrueDetail data)
        {
            return pUpdateCapexRevenue(data);
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

		#region Private

		private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxStopAccrueDetail> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetail";

                if (orderBy == "Draft") {
                    command.Parameters.Add(command.CreateParameter("@vType", "GetListDraft"));
                    orderBy = "";
                }
                else { 
				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                }
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxStopAccrueDetail> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxStopAccrueDetail pGetByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxStopAccrueDetail pCreate(trxStopAccrueDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxStopAccrueDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxStopAccrueDetail> pCreateBulky(List<trxStopAccrueDetail> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxStopAccrueDetail>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
        private List<trxStopAccrueDetail> pCreateBulkyEdit(List<trxStopAccrueDetail> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxStopAccrueDetail>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueDetail";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulkyEdit"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private List<trxStopAccrueDetail> pCreateBulkyDraft(List<trxStopAccrueDetail> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxStopAccrueDetail>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueDetail";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulkyDraft"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private trxStopAccrueDetail pUpdateCapexRevenue(trxStopAccrueDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxStopAccrueDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "UpdateCapexRevenue"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ID", data.ID));

				command.ExecuteNonQuery();

				return data;
			}
		}

        private trxStopAccrueDetail pUpdate(trxStopAccrueDetail data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxStopAccrueDetail>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueDetail";

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
				command.CommandText = "dbo.usptrxStopAccrueDetail";

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
				command.CommandText = "dbo.usptrxStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
        private List<trxStopAccrueDetail> pDeleteDraft(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueDeleteDraft";

                command.Parameters.Add(command.CreateParameter("@Initiator", whereClause));

                return this.ReadTransaction(command).ToList();
            }
        }
        #endregion

    }
}