    
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models;

namespace ARSystem.Domain.Repositories
{
	public class trxReactiveStopAccrueHeaderRepository : BaseRepository<trxReactiveStopAccrueHeader>
	{
		private DbContext _context;
		public trxReactiveStopAccrueHeaderRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxReactiveStopAccrueHeader> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxReactiveStopAccrueHeader> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

        public trxReactiveStopAccrueHeader Create(trxReactiveStopAccrueHeader data)
        {
            return pCreate(data);
        }

        public trxReactiveStopAccrueHeader GetByPK(long iD)
        {
            return pGetByPK(iD);
        }

        public trxReactiveStopAccrueHeader UpdateHeader(trxReactiveStopAccrueHeader data)
        {
            return pUpdateHeader(data);
        }

        public trxReactiveStopAccrueHeader CreateDraft(trxReactiveStopAccrueHeader data)
        {
            return pCreateDraft(data);
        }

        public trxReactiveStopAccrueHeader CheckHeaderID(trxReactiveStopAccrueHeader data)
        {
            return pCheckHeaderID(data);
        }

        public List<trxReactiveStopAccrueHeader> CreateBulky(List<trxReactiveStopAccrueHeader> data)
        {
            return pCreateBulky(data);
        }

        public trxReactiveStopAccrueHeader Update(trxReactiveStopAccrueHeader data)
        {
            return pUpdate(data);
        }

        public trxReactiveStopAccrueHeader UpdateStatus(trxReactiveStopAccrueHeader data)
        {
            return pUpdateStatus(data);
        }

        public trxReactiveStopAccrueHeader UpdateStatusSubmitDoc(trxReactiveStopAccrueHeader data)
        {
            return pUpdateStatusSubmitDoc(data);
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
				command.CommandText = "dbo.uspvwtrxReactiveStopAccrueHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxReactiveStopAccrueHeader> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwtrxReactiveStopAccrueHeader";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxReactiveStopAccrueHeader> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo. ";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
        private trxReactiveStopAccrueHeader pCreate(trxReactiveStopAccrueHeader data)
        {
            string type = "CreateReactiveHoldAccrue";
            if (data.RequestType == "STOP" && data.Remarks != "DRAFT")
            {
                type = "CreateReactiveStopAccrue";
            }
            else if (data.Remarks == "DRAFT")
            {
                type = "CreateStopAccrueDraft";
            }


            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwtrxReactiveStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                data.ID = this.WriteTransaction(command);

                return data;

            }
        }

        private trxReactiveStopAccrueHeader pGetByPK(long iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }

        private trxReactiveStopAccrueHeader pUpdateHeader(trxReactiveStopAccrueHeader data)
        {
            string type = "UpdateHeader";

            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                data.ID = this.WriteTransaction(command);

                return data;
            }
        }

        private trxReactiveStopAccrueHeader pCheckHeaderID(trxReactiveStopAccrueHeader data)
        {
            string type = "CheckIdHeader";
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                data.ID = this.WriteTransaction(command);

            }
            return data;
        }

        private trxReactiveStopAccrueHeader pCreateDraft(trxReactiveStopAccrueHeader data)
        {
            string type = "CreateHoldAccrue";
            if (data.RequestType == "STOP")
                type = "CreateStopAccrue";

            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwtrxStopAccrueHeaderDraft";

                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                data.ID = this.WriteTransaction(command);

                return data;

            }
        }

        private trxReactiveStopAccrueHeader pCreate2(trxReactiveStopAccrueHeader data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                this.WriteTransaction(command);

                return data;
            }
        }

        private List<trxReactiveStopAccrueHeader> pCreateBulky(List<trxReactiveStopAccrueHeader> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReactiveStopAccrueHeader>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxReactiveStopAccrueHeader pUpdateStatus(trxReactiveStopAccrueHeader data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateStatus"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxReactiveStopAccrueHeader pUpdateStatusSubmitDoc(trxReactiveStopAccrueHeader data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateStatusSubmitDoc"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxReactiveStopAccrueHeader pUpdate(trxReactiveStopAccrueHeader data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

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
                command.CommandText = "dbo.usptrxStopAccrueHeader";

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
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }

        #endregion

    }
}