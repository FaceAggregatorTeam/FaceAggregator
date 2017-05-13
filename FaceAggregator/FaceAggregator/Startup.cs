using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FaceAggregator.Startup))]
namespace FaceAggregator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
