using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace ARSystem.Domain.Repositories
{
    public class DynamicDashboardRepository : BaseRepository<DashboardData>
    {
        private DbContext _context;
        public DynamicDashboardRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        /// <summary>
        /// untuk Update Data BAPS di Server TBGSys
        /// </summary>
        /// <param name="RejectBaps"></param>
        /// <returns></returns>
        public async Task<DashboardData> GetDashboardData(string viewName, string dbName, string view, int start, int length, string whereClause, string selectCol)
        {
            DashboardData dataObj = new DashboardData();
            using (var command = _context.CreateCommand())
            {
                selectCol = selectCol == "" ? "*" : selectCol;
                string[] arrayCols = selectCol.Split(',');
                int counCols = arrayCols.Length-1;
                selectCol = "";
                for (int i = 0; i < arrayCols.Length; i++)
                {
                    if (i != counCols)
                        selectCol += "LTRIM(RTRIM(" + arrayCols[i] + ")) "+ arrayCols[i]+", ";
                    else
                        selectCol += "LTRIM(RTRIM(" + arrayCols[i] + ")) " + arrayCols[i];
                }

                string colName = await GetColumnName(view, dbName);
                colName = colName == "" ? "*" : colName;
                command.CommandType = CommandType.Text;
                command.CommandText = "select " + selectCol + " from ( select ROW_NUMBER() OVER(ORDER BY " + colName + ") as RowIndex, * from " + viewName + ")" +
                                        "a where RowIndex > " + start + " and RowIndex <=" + length + " " + whereClause;
                dataObj.dataList = command.ExecuteReader().DictionaryList();
            }
            return dataObj;
        }

        public async Task<int> GetDashboardCountData(string viewName, string whereClause)
        {
            DashboardData dataObj = new DashboardData();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "select count(1) from " + viewName + whereClause;
                return this.CountTransaction(command);
            }

        }


        public async Task<string> GetScriptView(string viewName, string dbName)
        {

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @" USE " + dbName + " \r\n  SELECT OBJECT_DEFINITION(object_id('" + viewName + "')) as result";
                return command.ExecuteReader().DataToString();
            }

        }
        public async Task<string> GetColumnName(string viewName, string dbName)
        {

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @" SELECT TOP 1  COLUMN_NAME as Result FROM " + dbName + ".INFORMATION_SCHEMA.COLUMNS " +
                                         " WHERE TABLE_NAME = N'" + viewName + "'";
                return command.ExecuteReader().DataToString();
            }

        }

        public async Task<string> GetColumnNameListByView(string viewName, string dbName)
        {

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @" SELECT ShowColumn AS result FROM [TBGARSystem].[dbo].[mstDataSourceDashboard] WHERE ViewName='" + viewName + "' AND DBContext='" + dbName + "'";

                return command.ExecuteReader().ListToString();
            }


        }

        public async Task<List<Dictionary<string, string>>> GetColumnNameList(string viewName, string dbName)
        {

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @" SELECT COLUMN_NAME as text , DATA_TYPE as value FROM " + dbName + ".INFORMATION_SCHEMA.COLUMNS " +
                                         " WHERE TABLE_NAME = N'" + viewName + "'";

                return command.ExecuteReader().DictionaryList2();
            }


        }

        public async Task<List<Dictionary<string, string>>> GetSchemaDBList()
        {

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"select s.name as value, 
                                        u.name as text
                                         from sys.schemas s
                                        inner join sys.sysusers u
                                            on u.uid = s.principal_id
		                                    WHERE u.name ='dbo'
                                    order by s.name";

                return command.ExecuteReader().DictionaryList2();
            }
        }

    }
}
