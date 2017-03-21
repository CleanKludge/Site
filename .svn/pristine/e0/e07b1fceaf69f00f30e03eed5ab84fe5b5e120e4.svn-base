using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanKludge.Server.Authentication.Filters
{
    public class AuthenticationAttribute : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            var user = context.HttpContext.User;
            var controller = context.Controller as Controller;

            if (user == null || controller == null)
                return;

            controller.ViewBag.IsSignedIn = user.Identity.IsAuthenticated;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}