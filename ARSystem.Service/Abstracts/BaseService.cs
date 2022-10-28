using System.Collections.Generic;

using ARSystem.Domain.DAL;

namespace ARSystem.Service
{
    public abstract class BaseService
    {
        List<DbContext> _contexts;

        public BaseService()
        {
            _contexts = new List<DbContext>();
        }

        public DbContext SetContext(string connection = "ARSystem")
        {
            DbContext context = new DbContext(Helper.GetConnection(connection));
            _contexts.Add(context);
            return context;
        }

        public void Dispose()
        {
            foreach (var context in _contexts)
            {
                context.Dispose();
            }
        }
    }
}
