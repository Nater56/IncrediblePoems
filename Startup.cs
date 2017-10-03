using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IncrediblePoems.Startup))]
namespace IncrediblePoems
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
