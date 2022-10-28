using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System.Data;

namespace ARSystem.Domain.Repositories
{
    public class vwDashboardInputTargetDetailRepository : BaseRepository<vwDashboardInputTargetDetail>
    {
        private DbContext _context;
        public vwDashboardInputTargetDetailRepository(DbContext context) : base(context)
        {
            _context = context;
        }
        public int GetCount(int year,  string whereClause = "")
        {
            return pGetCount(year, whereClause);
        }

        public List<vwDashboardInputTargetDetail> GetList(int year, string whereClause = "", string orderBy = "")
        {
            return pGetList( year,  whereClause, orderBy);
        }

        public List<vwDashboardInputTargetDetail> GetPaged(int year, string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(year, whereClause, orderBy, rowSkip, pageSize);
        }

        #region Private

        private int pGetCount(int yaer, string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRADashboardInputTarget";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCountDetail"));
                command.Parameters.Add(command.CreateParameter("@vYear", yaer));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<vwDashboardInputTargetDetail> pGetList(int year, string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRADashboardInputTarget";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListDetail"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vYear", year));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<vwDashboardInputTargetDetail> pGetPaged(int year, string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRADashboardInputTarget";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPagedDetail"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                command.Parameters.Add(command.CreateParameter("@vYear", year));


                return this.ReadTransaction(command).ToList();
            }
        }
        #endregion


        //public vwRABapsSite GetReccuringHistoryInputTargetByPK(long targetID)
        //{
        //    return pGetReccuringHistoryInputTargetByPK(targetID);
        //}
        //private vwRABapsSite pGetReccuringHistoryInputTargetByPK(long targetID)
        //{
        //    using (var command = _context.CreateCommand())
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "dbo.uspvwRABapsSite";

        //        command.Parameters.Add(command.CreateParameter("@vType", "GetRecurringHistoryByTargetID"));
        //        command.Parameters.Add(command.CreateParameter("@targetID", targetID));

        //        return this.ReadTransaction(command).SingleOrDefault();
        //    }
        //}

        //public vwDashboardInputTargetDetail UpdateDashboardInputTargetDetail(vwDashboardInputTargetDetail inputTarget)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new vwDashboardInputTargetDetailRepository(context);
        //    vwDashboardInputTargetDetail result = new vwDashboardInputTargetDetail();
        //    try
        //    {
        //        DateTime dtNow = Helper.GetDateTimeNow();

        //        var target = repo.(inputTarget.TargetID.GetValueOrDefault());
        //        if (target != null)
        //        {
        //            var _target = new MstRATargetRecurring();
        //            _target.Month = inputTarget.TargetMonth;
        //            _target.Year = inputTarget.TargetYear;
        //            _target.ID = inputTarget.TargetID.GetValueOrDefault();
        //            _target.StartInvoiceDate = inputTarget.StartInvoiceDate;
        //            _target.EndInvoiceDate = inputTarget.EndInvoiceDate;
        //            _target.AmountIDR = inputTarget.AmountIDR;
        //            _target.AmountUSD = inputTarget.AmountUSD;

        //            result = _mstRATargetRecurringRepository.Update(_target);
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _context.Dispose();
        //        result = (new MstRATargetRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "UploadTargetNewBaps", "")));
        //        return result;
        //    }

        //}
    }
}
