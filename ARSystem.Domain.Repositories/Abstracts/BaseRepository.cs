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
    public abstract class BaseRepository<T> where T : new()
    {
        DbContext _context;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="context">DBContext</param>
        public BaseRepository(DbContext context)
        {
            _context = context;
        }

        protected DbContext Context
        {
            get
            {
                return this._context;
            }
        }

        /// <summary>
        /// Generic Object To List
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns></returns>
        protected int CountTransaction(IDbCommand command)
        {
            using (var record = command.ExecuteReader())
            {
                int result = 0;
                while (record.Read())
                {
                    result = (int)record[0];
                }
                return result;
            }
        }

        /// <summary>
        /// Generic Object To List
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns></returns>
        protected IList<T> ReadTransaction(IDbCommand command)
        {
            using (var record = command.ExecuteReader())
            {
                List<T> items = new List<T>();
                while (record.Read())
                {
                    items.Add(Map<T>(record));
                }
                return items;
            }
        }

        /// <summary>
        /// Mapping Generic Object
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="record">Data Record</param>
        /// <returns></returns>
        protected T Map<T>(IDataRecord record)
        {
            var objT = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                    property.SetValue(objT, record[property.Name]);
            }
            return objT;

        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected int WriteTransaction(IDbCommand command)
        {
            int returnVal = 0;

            using (var record = command.ExecuteReader())
            {
                while (record.Read())
                {
                    returnVal = Convert.ToInt32(record[0]);
                }
            }

            return returnVal;
        }

        /// <summary>
        /// Generic Object to new object view list.
        /// </summary>
        /// <typeparam name="T">object view object as expected</typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        protected IList<T> ReadTransaction<T>(IDbCommand command)
        {
            using (var record = command.ExecuteReader())
            {
                List<T> items = new List<T>();
                while (record.Read())
                {
                    items.Add(Map<T>(record));
                }
                return items;
            }
        }

        protected T ReadSingleRow<T>(IDbCommand command)
        {
            using (var record = command.ExecuteReader())
            {
                T item = default(T);
                if (record.Read())
                {
                    item = Map<T>(record);
                }
                return item;
            }
        }
    }
}
