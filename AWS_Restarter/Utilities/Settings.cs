using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace AWS_Restarter.Utilities
{
    public static class Settings
    {
        public static string Get(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key] ?? null;
        }
    }
}
