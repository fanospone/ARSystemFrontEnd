
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
    public class trxStopAccrueHeaderRepository : BaseRepository<trxStopAccrueHeader>
    {
        private DbContext _context;
        public trxStopAccrueHeaderRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<trxStopAccrueHeader> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<trxStopAccrueHeader> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public trxStopAccrueHeader GetByPK(long iD)
        {
            return pGetByPK(iD);
        }

        public trxStopAccrueHeader Create(trxStopAccrueHeader data)
        {
            return pCreate(data);
        }
        public trxStopAccrueHeader UpdateHeader(trxStopAccrueHeader data)
        {
            return pUpdateHeader(data);
        }
        public trxStopAccrueHeader CreateDraft(trxStopAccrueHeader data)
        {
            return pCreateDraft(data);
        }
        public trxStopAccrueHeader CheckHeaderID(trxStopAccrueHeader data)
        {
            return pCheckHeaderID(data);
        }
        public List<trxStopAccrueHeader> CreateBulky(List<trxStopAccrueHeader> data)
        {
            return pCreateBulky(data);
        }

        public trxStopAccrueHeader Update(trxStopAccrueHeader data)
        {
            return pUpdate(data);
        }

        public trxStopAccrueHeader UpdateStatus(trxStopAccrueHeader data)
        {
            return pUpdateStatus(data);
        }

        public trxStopAccrueHeader UpdateStatusSubmitDoc(trxStopAccrueHeader data)
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
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<trxStopAccrueHeader> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<trxStopAccrueHeader> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private trxStopAccrueHeader pGetByPK(long iD)
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
        private trxStopAccrueHeader pCreate(trxStopAccrueHeader data)
        {
            string type = "CreateHoldAccrue";
            if (data.RequestType == "STOP" && data.Remarks != "DRAFT")
            {
                type = "CreateStopAccrue";
            }
            else if (data.Remarks == "DRAFT")
            {
                type = "CreateStopAccrueDraft";
            }
            

            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                data.ID = this.WriteTransaction(command);

                return data;

            }
        }
        private trxStopAccrueHeader pUpdateHeader(trxStopAccrueHeader data)
        {
            string type = "UpdateHeader";

            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                data.ID = this.WriteTransaction(command);

                return data;
            }
        }
        private trxStopAccrueHeader pCheckHeaderID(trxStopAccrueHeader data)
        {
            string type = "CheckIdHeader";
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                data.ID = this.WriteTransaction(command);

            }
            return data;
        }

        private trxStopAccrueHeader pCreateDraft(trxStopAccrueHeader data)
        {
            string type = "CreateHoldAccrue";
            if (data.RequestType == "STOP")
                type = "CreateStopAccrue";

            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwtrxStopAccrueHeaderDraft";

                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                data.ID = this.WriteTransaction(command);

                return data;

            }
        }
        private trxStopAccrueHeader pCreate2(trxStopAccrueHeader data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                this.WriteTransaction(command);

                return data;
            }
        }
        private List<trxStopAccrueHeader> pCreateBulky(List<trxStopAccrueHeader> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxStopAccrueHeader>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private trxStopAccrueHeader pUpdateStatus(trxStopAccrueHeader data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateStatus"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxStopAccrueHeader pUpdateStatusSubmitDoc(trxStopAccrueHeader data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxStopAccrueHeader>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueHeader";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateStatusSubmitDoc"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxStopAccrueHeader pUpdate(trxStopAccrueHeader data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxStopAccrueHeader>(data);

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