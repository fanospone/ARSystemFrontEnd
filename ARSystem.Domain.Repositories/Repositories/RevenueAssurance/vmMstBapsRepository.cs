using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.Models.RevenueAssurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Repositories.Repositories.RevenueAssurance
{
    public class vmMstBapsRepository : BaseRepository<mstBaps>
    {
        private DbContext _context;
        public vmMstBapsRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<mstBaps> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<mstBaps> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public mstBaps GetByPK(int iD)
        {
            return pGetByPK(iD);
        }

        public mstBaps Create(mstBaps data, string strAction)
        {
            return pCreate(data, strAction);
        }

        public List<mstBaps> CreateBulky(List<mstBaps> data)
        {
            return pCreateBulky(data);
        }

        public mstBaps Update(mstBaps data, string strAction)
        {
            return pUpdate(data, strAction);
        }

        public mstBaps Approve(mstBaps data, string strAction)
        {
            return pApprove(data, strAction);
        }

        public bool DeleteByPK(int iD)
        {
            pDeleteByPK(iD);
            return true;
        }

        public bool DeleteByFilter(string whereClause)
        {
            pDeleteByFilter(whereClause);
            return true;
        }

        public mstBaps BapsDone(mstBaps data, int SplitBill)
        {
            return pBapsDone(data, SplitBill);
        }

        public mstBaps BapsReject(mstBaps data)
        {
            return pBapsReject(data);
        }
        public List<mstBaps> UploadBAPSValidation(string userID, List<mstBaps> data)
        {
            return pUploadBAPSValidation(userID, data);
        }

        #region Private

        private int pGetCount(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<mstBaps> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<mstBaps> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private mstBaps pGetByPK(int iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private mstBaps pCreate(mstBaps data, string strAction)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstBaps>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vAction", strAction));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                data.ID = this.WriteTransaction(command);

                return data;
            }
        }

        private List<mstBaps> pUploadBAPSValidation(string userID, List<mstBaps> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<mstBaps>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.[usptrxUploadBAPSValidation]";

                command.Parameters.Add(command.CreateParameter("@vXmlMstBaps", xml));
                command.Parameters.Add(command.CreateParameter("@userID", userID));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private List<mstBaps> pCreateBulky(List<mstBaps> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<mstBaps>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private mstBaps pUpdate(mstBaps data, string strAction)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstBaps>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "Update"));
                command.Parameters.Add(command.CreateParameter("@vAction", strAction));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private mstBaps pApprove(mstBaps data, string strAction)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstBaps>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "Approve"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private void pDeleteByPK(int iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

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
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }

        private mstBaps pBapsDone(mstBaps data, int SplitBill)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstBaps>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "BapsDone"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@SplitBill", SplitBill));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private mstBaps pBapsReject(mstBaps data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstBaps>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstBaps";

                command.Parameters.Add(command.CreateParameter("@vType", "BapsReject"));
                command.Parameters.Add(command.CreateParameter("@UserID", data.UpdatedBy));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }
        #endregion

        #region Check
        public mstBaps CheckBaps(string SONumber, int StipSiro, int BapsType, int PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspCheckBaps";

                command.Parameters.Add(command.CreateParameter("@SoNumber", SONumber));
                command.Parameters.Add(command.CreateParameter("@StipSiro", StipSiro));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        #endregion
    }
}
