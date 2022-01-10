using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(web_app_asp_net_mvc_grid.Startup))]
namespace web_app_asp_net_mvc_grid
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
