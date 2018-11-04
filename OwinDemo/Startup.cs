using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinDemo
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.Use(async (ctx, next) => {
                await ctx.Response.WriteAsync("Hello World");
            });
            //ctx is IOwinContext; it is just a wrapper around "environment" variable
            //next is a delegate
        }
    }
}