using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace FaceAggregator.Utils
{
    public class WebConfiguration : IConfiguration
    {
        public string GetValue(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}