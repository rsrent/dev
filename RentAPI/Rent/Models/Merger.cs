using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace Rent.Models
{
    public class Merger
    {
        public static dynamic Merge(object item1, object item2)
        {
            if (item1 == null || item2 == null)
                return item1 ?? item2 ?? new ExpandoObject();
              
            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;
            
            foreach (var fi in GetDic(item1))
            {
                result[fi.Key] = fi.Value;
            }
            foreach (var fi in GetDic(item2))
            {
                result[fi.Key] = fi.Value;
            }
            
            return result;
        }

        private static IDictionary<string, object> GetDic(object item)
        {
            if (item is ExpandoObject o)
            {
                return Get(o);
            }
            else
            {
                return Get(item);
            }
        }
        
        private static IDictionary<string, object> Get(object item)
        {
            return item.GetType().GetProperties().ToDictionary(fi => fi.Name, fi => fi.GetValue(item, null));
        }
        
        private static IDictionary<string, object> Get(ExpandoObject item)
        {
            var result = item as IDictionary<string, object>;
            return result;
        }
    }
}