
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
    public class mstDashboardTemplateRepository : BaseRepository<mstDashboardTemplate>
    {
        private DbContext _context;
        public mstDashboardTemplateRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public int GetCount(string whereClause = "")
        {
            return pGetCount(whereClause);
        }

        public List<mstDashboardTemplate> GetList(string whereClause = "", string orderBy = "")
        {
            return pGetList(whereClause, orderBy);
        }

        public List<mstDashboardTemplate> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
        }

        public mstDashboardTemplate GetByPK(int iD)
        {
            return pGetByPK(iD);
        }

        public mstDashboardTemplate Create(mstDashboardTemplate data)
        {
            return pCreate(data);
        }

        public List<mstDashboardTemplate> CreateBulky(List<mstDashboardTemplate> data)
        {
            return pCreateBulky(data);
        }

        public mstDashboardTemplate Update(mstDashboardTemplate data)
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
                command.CommandText = "dbo.uspmstDashboardTemplate";

                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.CountTransaction(command);
            }
        }
        private List<mstDashboardTemplate> pGetList(string whereClause, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstDashboardTemplate";

                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
            }
        }
        private List<mstDashboardTemplate> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstDashboardTemplate";

                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

                return this.ReadTransaction(command).ToList();
            }
        }
        private mstDashboardTemplate pGetByPK(int iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstDashboardTemplate";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }
        private mstDashboardTemplate pCreate(mstDashboardTemplate data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstDashboardTemplate>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstDashboardTemplate";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                data.ID = this.WriteTransaction(command);

                return data;
            }
        }
        private List<mstDashboardTemplate> pCreateBulky(List<mstDashboardTemplate> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<mstDashboardTemplate>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstDashboardTemplate";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private mstDashboardTemplate pUpdate(mstDashboardTemplate data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<mstDashboardTemplate>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspmstDashboardTemplate";

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
                command.CommandText = "dbo.uspmstDashboardTemplate";

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
                command.CommandText = "dbo.uspmstDashboardTemplate";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }

        public async Task<string> GetHiddenColumn(string viewName, string column, string dbName)
        {

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"DECLARE  @string NVARCHAR(MAX)='" + column + @"',
                                        @delimiter CHAR(1)=','
                                    BEGIN
	                                    DECLARE @out_put TABLE (
                                        [column_id] INT IDENTITY(1, 1) NOT NULL,
                                        [value] NVARCHAR(MAX)
                                        )

                                        DECLARE @value NVARCHAR(MAX),
                                            @pos INT = 0,
                                            @len INT = 0,
		                                    @result INT = 0

                                        SET @string = CASE 
                                                WHEN RIGHT(@string, 1) != @delimiter
                                                    THEN @string + @delimiter
                                                ELSE @string
                                                END

                                        WHILE CHARINDEX(@delimiter, @string, @pos + 1) > 0
                                        BEGIN
                                            SET @len = CHARINDEX(@delimiter, @string, @pos + 1) - @pos
                                            SET @value = SUBSTRING(@string, @pos, @len)

                                            INSERT INTO @out_put ([value])
                                            SELECT LTRIM(RTRIM(@value)) AS [column]

                                            SET @pos = CHARINDEX(@delimiter, @string, @pos + @len) + 1
                                        END
	
	                                    SELECT COLUMN_NAME as Result
		                                    FROM " + dbName + @".INFORMATION_SCHEMA.COLUMNS 
		                                    WHERE TABLE_NAME='" + viewName + @"' AND COLUMN_NAME NOT IN	(SELECT [value] FROM @out_put)

                                    END
                                    ";

                return command.ExecuteReader().ListToString();
            }

            #endregion
        }
    }
}