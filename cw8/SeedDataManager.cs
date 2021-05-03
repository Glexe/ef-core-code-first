using cw8.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace cw8
{
    public static class SeedDataManager
    {
        private const string DATA_FILE_LOCAL_PATH = @"Data\SeedData.json";

        public static SeedDataDTO GetData()
        {
            SeedDataDTO result = null;
            var project_root_path = Environment.CurrentDirectory.Replace(@"bin\Debug", "");
            try
            {
                var json = File.ReadAllText(Path.Combine(project_root_path, DATA_FILE_LOCAL_PATH));
                result = JsonConvert.DeserializeObject<SeedDataDTO>(json);
            }
            catch { }

            return result;
        }
    }
}
