using Getnet.IService;
using Getnet.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Getnet.Service
{
    public class AppSettingsService : IAppSettingsService
    {
        public AppSettings GetAppSettings()
        {
            const string path = "appsettings.json";

            if (!File.Exists(path))
            {
                throw new Exception("The file appSettings does not exists!");
            }


            string fileReader = File.ReadAllText(path);

            AppSettings appSettings = JsonConvert.DeserializeObject<AppSettings>(fileReader);

            return appSettings;
        }
    }
}
