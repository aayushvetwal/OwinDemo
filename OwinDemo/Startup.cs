using Owin;
using OwinDemo.Middleware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Nancy.Owin;
using Nancy;

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

            //app.Map("/nancy", mappedApp => { mappedApp.UseNancy(); }); //this works only with localhost:xxxx/nancy/nancy...because Nancy does its routing based on RequestPath only ignoring RequestPathBase
            //app.UseNancy(); //this gives 404 error for root i.e. localhost:xxxx/ but works for localhost:xxxx/nancy

            app.UseNancy(config => {
                //pass through nancy to next middleware if status code is "not found" (404)
                config.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound);
            });

            app.Use(async (ctx, next) => {
                await ctx.Response.WriteAsync("<html><head></head><body>Hello World</body></html>");
            });
        }
    }
}