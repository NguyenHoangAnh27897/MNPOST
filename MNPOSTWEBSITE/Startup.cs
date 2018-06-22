using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MNPOSTWEBSITE.Startup))]
namespace MNPOSTWEBSITE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
