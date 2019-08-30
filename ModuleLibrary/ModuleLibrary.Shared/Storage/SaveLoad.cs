using System;
using System.IO;
using Newtonsoft.Json;

namespace ModuleLibrary.Shared.Storage
{
    public class SaveLoad
    {
        public SaveLoad()
        {

        }

        public static void SaveText(string filename, object text)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllText(filePath, JsonConvert.SerializeObject(text));
        }
        public static bool LoadText<Value>(string filename, out Value result)
        {
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);
                var idAsString = File.ReadAllText(filePath);
                result = JsonConvert.DeserializeObject<Value>(idAsString);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = default(Value);
                // This allows application to redirect to "Sign-In" when there is no value stored for the Token
                return false;
            }
        }
    }
}
