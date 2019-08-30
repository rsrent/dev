using System;
using System.Linq;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Uri myUri = new Uri("https://storagesample.blob.core.windows.net/sample-container/" +
    "sampleBlob.txt?sv=2015-07-08&sr=b&sig=39Up9JzHkxhUIhFEjEH9594DJxe7w6cIRCg0V6lCGSo%3D" +
    "&se=2016-10-18T21%3A51%3A37Z&sp=rcw");

            var res = myUri.Query.Split(new [] {'=', '&', '?' }).ToList();
            Console.WriteLine(res[res.IndexOf("se") + 1]);

        }
    }
}
