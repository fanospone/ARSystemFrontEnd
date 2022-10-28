using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.Models.ARSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Repositories.Repositories.ARSystem
{
    public class vwRAReconcileRepository : BaseRepository<vwRAReconcile>
    {
        private DbContext _context;
        public vwRAReconcileRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "", string sp = "", List<vwRAReconcile> filter = null)
        {
            return pGetCount(whereClause, sp, filter);
        }

        public List<vwRAReconcile> GetList(string whereClause = "", string orderBy = "", string sp = "", List<vwRAReconcile> filter = null)
        {
            return pGetList(whereClause, orderBy, sp, filter);
        }

        public List<vwRAReconcile> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize, string sp = "", List<vwRAReconcile> filter = null)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize, sp, filter);
        }

        public List<vwRAReconcile> GetData(string WhereClause, string Filter)
        {
            return pGetData(WhereClause, Filter);
        }

        public List<vwRAReconcile> GetData(string whereClause, string orderBy, int rowSkip, int pageSize, List<vwRAReconcile> filter = null)
        {
            return pGetData(whereClause, orderBy, rowSkip, pageSize, filter);
        }
        public int CountData(string whereClause = "", string sp = "", List<vwRAReconcile> filter = null)
        {
            return pCountData(whereClause, sp, filter);
        }

        public List<vwRAReconcile> GetDataRegional(string whereClause, string orderBy)
        {
            return pGetDataRegional(whereClause, orderBy);
        }

        public List<vwRAReconcile> GetSiteDataRegional(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetSiteDataRegional(whereClause, orderBy, rowSkip, pageSize);
        }

        public trxReconcileDocument CreateDocument(trxReconcileDocument data)
        {
            return pCreateDocument(data);
        }

        #region Private
        private int pCountData(string whereClause, string sp = "", List<vwRAReconcile> filter = null)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRAReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "CountData"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                if (filter != null && filter.Count > 0)
                {
                    string xml = Helper.XmlSerializer<List<vwRAReconcile>>(filter);
                    command.Parameters.Add(command.CreateParameter("@vXml", xml));
                }

                return this.CountTransaction(command);
            }
        }
        private List<vwRAReconcile> pGetDataRegional(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "GetData"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwRAReconcile> pGetSiteDataRegional(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "GetSiteData"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwRAReconcile> pGetData(string whereClause, string orderBy, int rowSkip, int pageSize, List<vwRAReconcile> filter = null)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRAReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "GetData"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                if (filter != null && filter.Count > 0)
                {
                    string xml = Helper.XmlSerializer<List<vwRAReconcile>>(filter);
                    command.Parameters.Add(command.CreateParameter("@vXml", xml));
                }

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwRAReconcile> pGetData(string WhereClause, string Filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwRADataRTI";

                command.Parameters.Add(command.CreateParameter("@Type", Filter));
                command.Parameters.Add(command.CreateParameter("@WhereClause", WhereClause));

                return this.ReadTransaction(command).ToList();
            }
        }
        private int pGetCount(string whereClause, string sp = "", List<vwRAReconcile> filter = null)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sp;//"dbo.uspvwOPMRASow";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                if (filter != null && filter.Count > 0)
                {
                    //filter = new List<vwRAReconcile>();
                    //filter.Add(new vwRAReconcile
                    //{
                    //    SONumber = "-X"
                    //});
                    string xml = Helper.XmlSerializer<List<vwRAReconcile>>(filter);
                    command.Parameters.Add(command.CreateParameter("@vXml", xml));
                }

                return this.CountTransaction(command);
            }
        }
        private List<vwRAReconcile> pGetList(string whereClause, string orderBy, string sp = "", List<vwRAReconcile> filter = null)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sp;//"dbo.uspvwOPMRASow";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                if (filter != null && filter.Count > 0)
                {
                    //filter = new List<vwRAReconcile>();
                    //filter.Add(new vwRAReconcile
                    //{
                    //    SONumber = "-X"
                    //});
                    string xml = Helper.XmlSerializer<List<vwRAReconcile>>(filter);
                    command.Parameters.Add(command.CreateParameter("@vXml", xml));
                }

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwRAReconcile> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize, string sp = "", List<vwRAReconcile> filter = null)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sp;//"dbo.uspvwOPMRASow";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                if (filter != null && filter.Count > 0)
                {
                    //filter = new List<vwRAReconcile>();
                    //filter.Add(new vwRAReconcile
                    //{
                    //    SONumber = "-X"
                    //});
                    string xml = Helper.XmlSerializer<List<vwRAReconcile>>(filter);
                    command.Parameters.Add(command.CreateParameter("@vXml", xml));
                }


                return this.ReadTransaction(command).ToList();
            }
        }

        private trxReconcileDocument pCreateDocument(trxReconcileDocument data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReconcileDocument>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcileDocument";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                data.ID = this.WriteTransaction(command);

                return data;
            }
        }
        #endregion

    }
}
