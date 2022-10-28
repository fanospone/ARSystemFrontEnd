using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ARSystemFrontEnd.Helper
{
    public class MappingHelper<T> where T: new()
    {
        public static T MappingModel(NameValueCollection nvc)
        {
            var model = Activator.CreateInstance<T>();
            foreach (string kvp in nvc.AllKeys)
            {
                PropertyInfo pi = model.GetType().GetProperty(kvp, BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    pi.SetValue(model, nvc[kvp], null);
                }
            }
            return model;
        }
    }
}