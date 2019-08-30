using System;
using System.Collections.Generic;

namespace RentAppProject
{
    public class CustomerDocument
    {
        public int ID { get; set; }
        public string Category { get; set; }
		public string Name { get; set; }
		public string URL { get; set; }

		public static Dictionary<string, List<CustomerDocument>> AsDisctionary(List<CustomerDocument> documents)
		{
			var dic = new Dictionary<string, List<CustomerDocument>>();
			foreach (var document in documents)
			{
				if (!dic.ContainsKey(document.Category))
				{
					dic.Add(document.Category, new List<CustomerDocument>());
				}
				dic[document.Category].Add(document);
			}
			return dic;
		}
	}
}
