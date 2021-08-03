using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NetZoneApplication.BaseEntity
{
    public static class Configuration
    {
        const string settingJson = "appsettings.json";

        public static AppSettings AppSettings()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory()) //Nuget Microsoft.Extensions.Configuration.FileExtensions to use SetBasePath
                 .AddJsonFile(settingJson);// Nuget Microsoft.Extensions.Configuration.Json to use AddJsonFile

            var config = builder.Build();
            var appConfig = new AppSettings();
            config.GetSection("AppSettings").Bind(appConfig);
            return appConfig;
        }

        public static string ConnectionString()
        {
            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory()) //Nuget Microsoft.Extensions.Configuration.FileExtensions to use SetBasePath
                  .AddJsonFile(settingJson);// Nuget Microsoft.Extensions.Configuration.Json to use AddJsonFile
            var config = builder.Build();

            var appConfig = new ConnectionStrings();
            config.GetSection("ConnectionStrings").Bind(appConfig);
            return appConfig.Connection;
        }
    }
}
