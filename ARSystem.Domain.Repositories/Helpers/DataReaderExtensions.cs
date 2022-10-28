using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ARSystem.Domain.Repositories
{
    public static class DataReaderExtensions
    {
        public static List<dynamic> MapToList(this IDataReader dr)
        {
            DataTable dt = new DataTable();
            var dynamicDt = new List<dynamic>();

            try
            {
                dt.Load(dr);
                foreach (DataRow row in dt.Rows)
                {
                    dynamic dyn = new ExpandoObject();
                    dynamicDt.Add(dyn);
                    foreach (DataColumn column in dt.Columns)
                    {
                        var dic = (IDictionary<string, object>)dyn;
                        dic[column.ColumnName] = row[column];
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dynamicDt;
        }

        public static List<Dictionary<string, string>> DictionaryList(this IDataReader dr)
        {
            DataTable dt = new DataTable();
            var dynamicDt = new List<Dictionary<string, string>>();

            try
            {
                dt.Load(dr);
                foreach (DataRow row in dt.Rows)
                {
                    var data = new Dictionary<string, string>();
                    foreach (DataColumn column in dt.Columns)
                    {

                        data.Add(CamelToRegular(column.ColumnName.TrimStart().TrimEnd()), row[column.ColumnName].ToString());
                    }
                    dynamicDt.Add(data);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dynamicDt;
        }

        public static List<Dictionary<string, string>> DictionaryList2(this IDataReader dr)
        {
            DataTable dt = new DataTable();
            var dynamicDt = new List<Dictionary<string, string>>();

            try
            {
                dt.Load(dr);
                foreach (DataRow row in dt.Rows)
                {
                    var data = new Dictionary<string, string>();
                    foreach (DataColumn column in dt.Columns)
                    {

                        data.Add(column.ColumnName, row[column.ColumnName].ToString());
                    }
                    dynamicDt.Add(data);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dynamicDt;
        }

        public static string DataToString(this IDataReader dr)
        {
            DataTable dt = new DataTable();
            string result = "";

            try
            {
                dt.Load(dr);
                result = dt.Rows[0]["Result"].ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static string ListToString(this IDataReader dr)
        {
            DataTable dt = new DataTable();
            string result = "";

            try
            {
                dt.Load(dr);
                List<string> list = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                { 
                    list.Add(dt.Rows[i]["result"].ToString());
                }
                result = string.Join(",", list);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }

        public static List<T> DataReaderMapToList<T>(this IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public static Dictionary<string, string> ToDictionary(this IDataReader dr)
        {
            DataTable dt = new DataTable();
            var data = new Dictionary<string, string>();
            try
            {
                dt.Load(dr);
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {

                        data.Add(column.ColumnName, row[column.ColumnName].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return data;

        }

        public static string CamelToRegular(string text)
        {
            string[] split = text.Split(',');
            //string[] result = new string[] { };
            int i = 0;
            List<string> result = new List<string>(); ;
            foreach (var item in split)
            {
                string[] splitText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(item, "(\\B[A-Z])", " $1")).Split(' ');
                string value = "";
                foreach (var a in splitText)
                {
                    if (a.Length == 1)
                        value += a;
                    else
                        value += " " + a + " ";
                }
                result.Add(value);
                i++;
            }
            string aaa = string.Join(",", result);

            return aaa;
        }
       
        public static List<Dictionary<string, object>> ToDictionaryDynamicList(DataTable dt)
        {
            
            var dynamicDt = new List<Dictionary<string, object>>();

            try
            {                
                foreach (DataRow row in dt.Rows)
                {
                    var data = new Dictionary<string, object>();
                    foreach (DataColumn column in dt.Columns)
                    {

                        data.Add(column.ColumnName, row[column.ColumnName].ToString());
                    }
                    dynamicDt.Add(data);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dynamicDt;
        }
        public static DataTable ToDictionaryDataTable(this IDataReader dr)
        {
            DataTable dt = new DataTable();
            var dynamicDt = new List<Dictionary<string, object>>();

            try
            {
                dt.Load(dr);
                
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

    }
}
