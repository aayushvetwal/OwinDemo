using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace OwinDemo
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            //keep middlewares in desired order
            //multiple middlewares constitute pipeline
            //"next" is a reference to the next middleware in pipeline. so by executing "next"
            //we are executing rest of the pipeline

            app.Use(async (ctx, next) => {
                Debug.WriteLine("Incoming Request: " + ctx.Request.Path);
                await next();
                Debug.WriteLine("Outgoing Request: " + ctx.Request.Path);
            });
            app.Use(async (ctx, next) => {
                await ctx.Response.WriteAsync("<html><head></head><body>Hello World</body></html>");
            });
            //ctx is IOwinContext; it is just a wrapper around "environment" variable
            //next is a delegate
        }
    }
}