using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Rent.Models
{
    public class Filter
    {
        public static dynamic FilterUnpermissioned(dynamic o, string thisKey, Dictionary<string, List<string>> unallowed)
        {
            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;

            if (o == null)
                return null;
            
            if (o is IEnumerable<object>)
            {
                var oss = o as IEnumerable<object>;
                return oss.Select(os => FilterUnpermissioned(os, thisKey, unallowed));
            }
            else
            {
                foreach (var fi in GetDic(o))
                {
                    if (unallowed.ContainsKey(thisKey) && unallowed[thisKey].Any(a => a.Equals(fi.Key)))
                    {
                        // not allowed
                    } 
                    else if (unallowed.ContainsKey(fi.Key))
                    {
                        result[fi.Key] = FilterUnpermissioned(fi.Value, fi.Key, unallowed);
                    }
                    else
                    {
                        result[fi.Key] = fi.Value;
                    }
                
                }
                return result;
            }
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