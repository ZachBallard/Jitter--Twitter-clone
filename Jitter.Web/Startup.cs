using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jitter.Web.Startup))]
namespace Jitter.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
