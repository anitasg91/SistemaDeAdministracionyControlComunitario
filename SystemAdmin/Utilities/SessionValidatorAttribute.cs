using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SAyCC.Bussiness.Common;
using System;
using System.Threading.Tasks;

namespace SAyCC.SystemAdmin.Utilities
{
    public class SessionValidatorAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var session = context.HttpContext.Session;
            if (session == null || session.GetString(Sessions.IdUser) == null)
            {
                var response = context.HttpContext.Response;
                response.Redirect(DBSet.urlRedirect);
                return;
            }
            await next();
        }
    }
}