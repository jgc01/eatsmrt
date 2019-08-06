using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EatIn.Startup))]
namespace EatIn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
