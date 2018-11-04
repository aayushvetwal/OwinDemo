using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppFunc = System.Func<
    System.Collections.Generic.IDictionary<string, object>,
    System.Threading.Tasks.Task
>;

namespace OwinDemo.Middleware
{
    public class DebugMiddleware
    {
        //If you inherit from base class called OwinMiddleware, the middleware won't work in any other OWIN 
        //implementation than project Katana. OwinMiddleware is Katana specific. Therefore, we'll skip the inheritance
        //and rather use AppFunc.

        AppFunc _next;

        public DebugMiddleware(AppFunc next)
        {
            _next = next;
        }

        /// <summary>
        /// Takes care of invoking the middleware
        /// </summary>
        /// <param name="environment">environment</param>
        /// <returns>Task</returns>
        public async Task Invoke(IDictionary<string, object> environment)
        {
            var ctx = new OwinContext(environment);
            //or use: var path = (string)environment["owin.RequestPath"];

            Debug.WriteLine("Incoming Request: " + ctx.Request.Path);
            await _next(environment);
            Debug.WriteLine("Outgoing Request: " + ctx.Request.Path);
        }
    }
}