using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace FaceAggregator
{
    public partial class Startup
    {
        private static readonly string GoogleClientId = WebConfigurationManager.AppSettings["GoogleClientId"];
        private static readonly string GoogleClientSecret = WebConfigurationManager.AppSettings["GoogleClientSecret"];
    }
}