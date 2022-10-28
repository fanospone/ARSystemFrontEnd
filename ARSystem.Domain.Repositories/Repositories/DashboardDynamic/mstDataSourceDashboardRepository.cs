
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
	public class mstDataSourceDashboardRepository : BaseRepository<mstDataSourceDashboard>
	{
		private DbContext _context;
		public mstDataSourceDashboardRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<mstDataSourceDashboard> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<mstDataSourceDashboard> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public mstDataSourceDashboard GetByPK(int iD)
		{
			return pGetByPK(iD);
		}

		public mstDataSourceDashboard Create(mstDataSourceDashboard data)
		{
			return pCreate(data);
		}

		public List<mstDataSourceDashboard> CreateBulky(List<mstDataSourceDashboard> data)
		{
			return pCreateBulky(data);
		}

		public mstDataSourceDashboard Update(mstDataSourceDashboard data)
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
				command.CommandText = "dbo.uspmstDataSourceDashboard";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<mstDataSourceDashboard> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstDataSourceDashboard";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<mstDataSourceDashboard> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstDataSourceDashboard";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private mstDataSourceDashboard pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstDataSourceDashboard";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private mstDataSourceDashboard pCreate(mstDataSourceDashboard data)
		{
			using (var command = _context.CreateCommand())
			{
                data.ParamFilter = GetParam(data.ViewName, data.DBContext, data.ParamFilter);
                string xml = Helper.XmlSerializer<mstDataSourceDashboard>(data);

                command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstDataSourceDashboard";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<mstDataSourceDashboard> pCreateBulky(List<mstDataSourceDashboard> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<mstDataSourceDashboard>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstDataSourceDashboard";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private mstDataSourceDashboard pUpdate(mstDataSourceDashboard data)
		{
			using (var command = _context.CreateCommand())
			{
                data.ParamFilter = GetParam(data.ViewName,data.DBContext, data.ParamFilter);

                string xml = Helper.XmlSerializer<mstDataSourceDashboard>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstDataSourceDashboard";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ID", data.ID));

				command.ExecuteNonQuery();

				return data;
			}
		}

        public string GetParam(string viewName, string dbName, string paramfilter)
        {

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"DECLARE @value NVARCHAR(MAX),
                                        @pos INT = 0,
                                        @len INT = 0,
		                                @result INT = 0,
                                        @ParamFilter varchar(max)

		                                DECLARE @string VARCHAR(100), @delimiter CHAR(1)
		                                SET @string = '" + paramfilter + @"'
		                                SET @delimiter=','
		                                DECLARE @out_put TABLE (
			                                [column_id] INT IDENTITY(1, 1) NOT NULL,
			                                [value] NVARCHAR(MAX)
			                                )
		
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

		                                DECLARE @table AS TABLE(ID INT IDENTITY, col VARCHAR(50),tipe VARCHAR(20))
		                                DECLARE  @countData INT, @row int
		
		
		                                INSERT INTO @table
		                                SELECT COLUMN_NAME, DATA_TYPE
		                                FROM " + dbName + @".INFORMATION_SCHEMA.COLUMNS a
		                                INNER JOIN @out_put b ON a.COLUMN_NAME = b.[value]
		                                WHERE  a.TABLE_NAME = '" + viewName + @"'
		
		                                SELECT @countData= COUNT(1) FROM @table
		                                SET @row =1
		                                SET @ParamFilter ='{'
                                        DECLARE @colmun varchar(50), @datatipe varchar(30)
		                                WHILE @row <= @countData
		                                BEGIN
                                            select @colmun = col , @datatipe = tipe from @table WHERE ID = @row
			                                IF @row = @countData
				                                SET @ParamFilter = @ParamFilter +'\""'+ @colmun+'\"":\""'+@datatipe+'\""'
                                            ELSE
                                                SET @ParamFilter = @ParamFilter +'\""'+ @colmun + '\"":\""' + @datatipe + '\"",'

                                            SELECT @row = @row + 1
                                        END;
                                        SET @ParamFilter = @ParamFilter + '}'
                                        select @ParamFilter as Result
                                        ";
                command.CommandText = command.CommandText.Replace(@"\","");
                return command.ExecuteReader().DataToString();
            }

        }
        private void pDeleteByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstDataSourceDashboard";

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
				command.CommandText = "dbo.uspmstDataSourceDashboard";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}