using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace CleanKludge.Server.Filters
{
    public class SiteVersionAttribute : Attribute, IResultFilter
    {
        private readonly IConfigurationRoot _configurationRoot;

        public SiteVersionAttribute(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var controller = context.Controller as Controller;
            if (controller == null)
                return;

            var version = _configurationRoot.GetValue("SiteVersion", "0.0.0");
            controller.ViewData["Version"] = version;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}