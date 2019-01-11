using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

[assembly: OwinStartup(typeof(WinFormServer.Startup))]

namespace WinFormServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(new Microsoft.AspNet.SignalR.HubConfiguration() { EnableJavaScriptProxies = false });
        }
    }
}
