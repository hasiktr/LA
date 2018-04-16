using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Case.Study.Api.Startup))]
namespace Case.Study.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}