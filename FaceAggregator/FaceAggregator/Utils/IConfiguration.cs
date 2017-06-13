using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceAggregator.Utils
{
    /// <summary>
    /// Interface controlling app configuration
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets the config value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetValue(string key);
    }
}
