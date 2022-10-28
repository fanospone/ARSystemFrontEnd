
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
    public class vwtrxStopAccrueHeaderDraftRepository : BaseRepository<vwtrxStopAccrueHeaderDraft>
    {
        protected DbContext _context;
        public vwtrxStopAccrueHeaderDraftRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<vwtrxStopAccrueHeaderDraft> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<vwtrxStopAccrueHeaderDraft> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }
        public vwtrxStopAccrueHeaderDraft Create(vwtrxStopAccrueHeaderDraft data)
        {
            return pCreate(data);
        }
        #region Private

        private int pGetCount(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwtrxStopAccrueHeaderDraft";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<vwtrxStopAccrueHeaderDraft> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwtrxStopAccrueHeaderDraft";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwtrxStopAccrueHeaderDraft> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwtrxStopAccrueHeaderDraft";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private vwtrxStopAccrueHeaderDraft pCreate(vwtrxStopAccrueHeaderDraft data)
        {
            string type = "CreateHoldAccrueDraft";
            if (data.RequestType == "STOP")
                type = "CreateStopAccrueDraft";

            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<vwtrxStopAccrueHeaderDraft>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwtrxStopAccrueHeaderDraft";

                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                data.ID = this.WriteTransaction(command);

                return data;

            }
        }

        //public trxStopAccrueHeaderDraft Create(trxStopAccrueHeaderDraft headerDraft)
        //{
        //    string type = "CreateHoldAccrueDraft";
        //    using (var command = _context.CreateCommand())
        //    {
        //        string xml = Helper.XmlSerializer<trxStopAccrueHeaderDraft>(data);

        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "dbo.uspvwtrxStopAccrueHeaderDraft";

        //        command.Parameters.Add(command.CreateParameter("@vType", type));
        //        command.Parameters.Add(command.CreateParameter("@vXml", xml));
        //        data.ID = this.WriteTransaction(command);

        //        return data;

        //    }

        //}
        #endregion
    }
}