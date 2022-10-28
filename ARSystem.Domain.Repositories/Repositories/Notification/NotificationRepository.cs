
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
	public class NotificationRepository : BaseRepository<Notification>
	{
		private DbContext _context;
		public NotificationRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<Notification> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<Notification> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public Notification GetByPK(int id)
		{
			return pGetByPK(id);
		}

		public Notification Create(Notification data)
		{
			return pCreate(data);
		}

		public List<Notification> CreateBulky(List<Notification> data)
		{
			return pCreateBulky(data);
		}

		public Notification Update(Notification data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int id)
		{
			pDeleteByPK(id);
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
				command.CommandText = "dbo.uspNotification";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<Notification> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspNotification";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<Notification> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspNotification";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private Notification pGetByPK(int id)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspNotification";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@Id", id));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}

        public List<Notification> pGetByUserID(string UserID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspNotification";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByUserID"));
                command.Parameters.Add(command.CreateParameter("@UserID", UserID));

                return this.ReadTransaction(command).ToList();
            }
        }
        private Notification pCreate(Notification data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<Notification>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspNotification";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.Id = this.WriteTransaction(command);

				return data;
			}
		}
		private List<Notification> pCreateBulky(List<Notification> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<Notification>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspNotification";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private Notification pUpdate(Notification data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<Notification>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspNotification";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@Id", data.Id));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int id)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspNotification";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@Id", id));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspNotification";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}

        public Notification Claim(Notification data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<Notification>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspNotification";

                command.Parameters.Add(command.CreateParameter("@vType", "Claim"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@Id", data.Id));

                command.ExecuteNonQuery();

                return data;
            }
        }
        #endregion

    }
}