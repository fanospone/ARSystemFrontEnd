﻿using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Repositories.Repositories.RevenueAssurance
{
    public class trxRANewBapsActivityRepository : BaseRepository<trxRANewBapsActivity>
    {
        private DbContext _context;
        public trxRANewBapsActivityRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<trxRANewBapsActivity> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<trxRANewBapsActivity> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public trxRANewBapsActivity GetByPK(int iD, string soNumber, int SIRO, int BapsType, int PowerType)
        {
            return pGetByPK(iD, soNumber, SIRO, BapsType, PowerType);
        }

        public trxRANewBapsActivity Create(trxRANewBapsActivity data)
        {
            return pCreate(data);
        }

        public List<trxRANewBapsActivity> CreateBulky(List<trxRANewBapsActivity> data)
        {
            return pCreateBulky(data);
        }

        public trxRANewBapsActivity Update(trxRANewBapsActivity data)
        {
            return pUpdate(data);
        }

        public bool DeleteByPK(int iD, string soNumber)
        {
            pDeleteByPK(iD, soNumber);
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
                command.CommandText = "dbo.usptrxRANewBapsActivity";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<trxRANewBapsActivity> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRANewBapsActivity";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<trxRANewBapsActivity> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRANewBapsActivity";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private trxRANewBapsActivity pGetByPK(int iD, string soNumber, int SIRO, int BapsType, int PowerType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRANewBapsActivity";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));
                command.Parameters.Add(command.CreateParameter("@SoNumber", soNumber));
                command.Parameters.Add(command.CreateParameter("@SIRO", SIRO));
                command.Parameters.Add(command.CreateParameter("@BapsType", BapsType));
                command.Parameters.Add(command.CreateParameter("@PowerType", PowerType));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private trxRANewBapsActivity pCreate(trxRANewBapsActivity data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxRANewBapsActivity>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRANewBapsActivity";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                data.ID = this.WriteTransaction(command);

                return data;
            }
        }
        private List<trxRANewBapsActivity> pCreateBulky(List<trxRANewBapsActivity> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxRANewBapsActivity>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRANewBapsActivity";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private trxRANewBapsActivity pUpdate(trxRANewBapsActivity data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxRANewBapsActivity>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRANewBapsActivity";

                command.Parameters.Add(command.CreateParameter("@vType", "Update"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));
                command.Parameters.Add(command.CreateParameter("@SoNumber", data.SoNumber));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private void pDeleteByPK(int iD, string soNumber)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRANewBapsActivity";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));
                command.Parameters.Add(command.CreateParameter("@SoNumber", soNumber));

                command.ExecuteNonQuery();
            }
        }
        private void pDeleteByFilter(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRANewBapsActivity";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }
        #endregion

    }
}
