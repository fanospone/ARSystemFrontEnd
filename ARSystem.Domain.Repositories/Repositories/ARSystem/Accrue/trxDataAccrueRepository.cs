
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
	public class trxDataAccrueRepository : BaseRepository<trxDataAccrue>
	{
		private DbContext _context;
		public trxDataAccrueRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxDataAccrue> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}
        public List<trxDataAccrue> CheckGetExistIsReConfirm(int WeekParam, int MonthParam, int YearParam)
        {
            return pCheckGetExistIsReConfirm(WeekParam, MonthParam,YearParam);
        }

        public List<trxDataAccrue> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxDataAccrue GetByPK(int iD)
		{
			return pGetByPK(iD);
		}

		public trxDataAccrue Create(trxDataAccrue data)
		{
			return pCreate(data);
		}

		public List<trxDataAccrue> CreateBulky(List<trxDataAccrue> data)
		{
			return pCreateBulky(data);
		}
        public List<trxDataAccrue> ConfirmFinanceBulky(List<trxDataAccrue> data)
        {
            return pConfirmFinanceBulky(data);
        }
        public List<trxDataAccrue> MoveFinanceBulky(List<trxDataAccrue> data)
        {
            return pMoveFinanceBulky(data);
        }

        public List<trxDataAccrue> ConfirmUserBulky(List<trxDataAccrue> data)
        {
            return pConfirmUserBulky(data);
        }
        public List<trxDataAccrue> MoveUserBulky(List<trxDataAccrue> data)
        {
            return pMoveUserBulky(data);
        }
        public trxDataAccrue FinalConfirmBulky(int? IsReConfirmParam, int AccrueStatusIDParam, string UserID, int WeekParam, int MonthParam, int YearParam)
        {
            return pFinalConfirmBulky(IsReConfirmParam, AccrueStatusIDParam, UserID, WeekParam, MonthParam, YearParam);
        }
        public trxDataAccrue Update(trxDataAccrue data)
		{
			return pUpdate(data);
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

		#region Private

		private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxDataAccrue> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
        private List<trxDataAccrue> pCheckGetExistIsReConfirm(int WeekParam, int MonthParam, int YearParam)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxDataAccrue";

                command.Parameters.Add(command.CreateParameter("@vType", "CheckIsReConfirmExist"));
                command.Parameters.Add(command.CreateParameter("@WeekParam", WeekParam));
                command.Parameters.Add(command.CreateParameter("@MonthParam", MonthParam));
                command.Parameters.Add(command.CreateParameter("@YearParam", YearParam));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<trxDataAccrue> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxDataAccrue pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxDataAccrue pCreate(trxDataAccrue data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxDataAccrue>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxDataAccrue> pCreateBulky(List<trxDataAccrue> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxDataAccrue>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
        private List<trxDataAccrue> pConfirmFinanceBulky(List<trxDataAccrue> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxDataAccrue>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxDataAccrue";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateStatusBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private List<trxDataAccrue> pMoveFinanceBulky(List<trxDataAccrue> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxDataAccrue>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxDataAccrue";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateDeptMoveBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private List<trxDataAccrue> pConfirmUserBulky(List<trxDataAccrue> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxDataAccrue>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxDataAccrue";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateConfirmUserBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private List<trxDataAccrue> pMoveUserBulky(List<trxDataAccrue> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxDataAccrue>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxDataAccrue";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateMoveUserBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private trxDataAccrue pFinalConfirmBulky(int? IsReConfirmParam, int AccrueStatusIDParam, string UserID, int WeekParam, int MonthParam, int YearParam)
        {
            using (var command = _context.CreateCommand())
            {
                trxDataAccrue data = new trxDataAccrue();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxDataAccrue";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateFinalConfirm"));
                command.Parameters.Add(command.CreateParameter("@IsReConfirmParam", IsReConfirmParam));
                command.Parameters.Add(command.CreateParameter("@AccrueStatusIDParam", AccrueStatusIDParam));
                command.Parameters.Add(command.CreateParameter("@UserID", UserID));
                command.Parameters.Add(command.CreateParameter("@WeekParam", WeekParam));
                command.Parameters.Add(command.CreateParameter("@MonthParam", MonthParam));
                command.Parameters.Add(command.CreateParameter("@YearParam", YearParam));
                command.ExecuteNonQuery();

                return data;
            }
        }
        private trxDataAccrue pUpdate(trxDataAccrue data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxDataAccrue>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
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
				command.CommandText = "dbo.usptrxDataAccrue";

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
				command.CommandText = "dbo.usptrxDataAccrue";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}