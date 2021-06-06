using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace CouchTo5kTracker.Classes
{
    static class JSON
    {
        public static T parse<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Concat("Error occured while deserializing JSON: ", e));
                return default;
            }
        }

        public static string load(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            string fileString = File.ReadAllText(path);
            return fileString;
        }

        public static void save(string path, object obj)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static void clear(string path)
        {
            File.Delete(path);
            File.Create(path);
        }

        public static string read(string path)
        {
            string text = File.ReadAllText(path);
            return text;
        }
    }
}
