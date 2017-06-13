using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace FaceAggregator.Utils
{
    /// <summary>
    /// Manages app configuration
    /// </summary>
    /// <seealso cref="FaceAggregator.Utils.IConfiguration" />
    public class WebConfiguration : IConfiguration
    {
        /// <summary>
        /// Gets the config value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}