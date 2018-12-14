using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FWTL.Infrastructure.Cache
{
    public static class CacheKeyBuilder
    {
        public static string Build<TKey, TModel>(TModel model)
        {
            var name = typeof(TKey).Name;
            PropertyInfo[] properties = typeof(TModel).GetProperties();
            List<string> values = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                values.Add(property.GetValue(model).ToString());
            }

            return $"{name}." + string.Join(".", values);
        }

        public static string Build<TKey, TModel>(TModel model, params Expression<Func<TModel, object>>[] properties)
        {
            var name = typeof(TKey).Name;
            List<string> values = new List<string>();
            foreach (var property in properties)
            {
                values.Add(property.Compile()(model).ToString());
            }

            return $"{name}." + string.Join(".", values);
        }
    }
}
