using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(conestogacinema_project.Startup))]
namespace conestogacinema_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
