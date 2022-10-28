using System.Collections.Generic;
using System.Data;
using System.Linq;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
    public class trxInvoiceNonRevenueRepository : BaseRepository<trxInvoiceNonRevenue>
    {
        protected DbContext _context;
        public trxInvoiceNonRevenueRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<trxInvoiceNonRevenue> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<trxInvoiceNonRevenue> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public trxInvoiceNonRevenue GetByPK(int trxInvoiceNonRevenueID)
        {
            return pGetByPK(trxInvoiceNonRevenueID);
        }

        public trxInvoiceNonRevenue Create(trxInvoiceNonRevenue data)
        {
            return pCreate(data);
        }

        public List<trxInvoiceNonRevenue> CreateBulky(List<trxInvoiceNonRevenue> data)
        {
            return pCreateBulky(data);
        }

        public List<trxInvoiceNonRevenue> MergeBulky(List<trxInvoiceNonRevenue> data)
        {
            return pMergeBulky(data);
        }

        public trxInvoiceNonRevenue Update(trxInvoiceNonRevenue data)
        {
            return pUpdate(data);
        }

        public bool DeleteByPK(int trxInvoiceNonRevenueID)
        {
            pDeleteByPK(trxInvoiceNonRevenueID);
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
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<trxInvoiceNonRevenue> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<trxInvoiceNonRevenue> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private trxInvoiceNonRevenue pGetByPK(int trxInvoiceNonRevenueID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@trxInvoiceNonRevenueID", trxInvoiceNonRevenueID));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private trxInvoiceNonRevenue pCreate(trxInvoiceNonRevenue data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxInvoiceNonRevenue>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                data.trxInvoiceNonRevenueID = this.WriteTransaction(command);

                return data;
            }
        }
        private List<trxInvoiceNonRevenue> pCreateBulky(List<trxInvoiceNonRevenue> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxInvoiceNonRevenue>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private List<trxInvoiceNonRevenue> pMergeBulky(List<trxInvoiceNonRevenue> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxInvoiceNonRevenue>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "MergeBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private trxInvoiceNonRevenue pUpdate(trxInvoiceNonRevenue data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxInvoiceNonRevenue>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "Update"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@trxInvoiceNonRevenueID", data.trxInvoiceNonRevenueID));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private void pDeleteByPK(int trxInvoiceNonRevenueID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
                command.Parameters.Add(command.CreateParameter("@trxInvoiceNonRevenueID", trxInvoiceNonRevenueID));

                command.ExecuteNonQuery();
            }
        }
        private void pDeleteByFilter(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxInvoiceNonRevenue";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
}