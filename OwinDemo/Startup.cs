using Owin;
using OwinDemo.Middleware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Nancy.Owin;
using Nancy;
using System.Web.Http;

namespace OwinDemo
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.UseDebugMiddleware(new DebugMiddlewareOptions
            {
                OnIncomingRequest = (ctx) => {
                    var watch = new Stopwatch();
                    watch.Start();
                    ctx.Environment["DebugStopwatch"] = watch;
                },

                OnOutgoingRequest = (ctx) => {
                    var watch = (Stopwatch)ctx.Environment["DebugStopwatch"];
                    watch.Stop();
                    Debug.WriteLine("Request took: " + watch.ElapsedMilliseconds + " ms");
                }

            });

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions {
                AuthenticationType = "ApplicationCookie", //you can call it whatever you want
                LoginPath = new Microsoft.Owin.PathString("/Auth/Login")
            });

            app.UseFacebookAuthentication(new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions{
                AppId = "1918997718410185",
                AppSecret = "1f2f69e55a5bf6fe55d1fea907fcbb46",
                SignInAsAuthenticationType = "ApplicationCookie"
            });

            app.Use(async (ctx, next) => {
                if(ctx.Authentication.User.Identity.IsAuthenticated)
                    Debug.WriteLine("User: " + ctx.Authentication.User.Identity.Name);
                else
                    Debug.WriteLine("User Not Authenticated");
                await next();
            });

            var config = new HttpConfiguration();   //config object contains all configuration for web api to run
            config.MapHttpAttributeRoutes(); //maps all the attributed routes setup in controller
            app.UseWebApi(config);

            app.Map("/nancy", mappedApp => { mappedApp.UseNancy(); }); //this works only with localhost:xxxx/nancy/nancy...because Nancy does its routing based on RequestPath only ignoring RequestPathBase
            //app.UseNancy(); //this gives 404 error for root i.e. localhost:xxxx/ but works for localhost:xxxx/nancy

            //app.UseNancy(conf => {
            //    //pass through nancy to next middleware if status code is "not found" (404)
            //    conf.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound);
            //});

            //The reason the hello world middleware is commented below is because asp.net mvc middleware only
            //works if there are no other middlewares to send response. If the middleware below continues to 
            //send response, mvc doesn't work.
            //app.Use(async (ctx, next) => {
            //    await ctx.Response.WriteAsync("<html><head></head><body>Hello World</body></html>");
            //});
        }
    }
}