using System;
using CleanKludge.Api.Responses.Articles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanKludge.Server.Filters
{
    public class DynamicLocationAttribute : Attribute, IResultFilter
    {
        private readonly Location _location;

        public DynamicLocationAttribute(Location location)
        {
            _location = location;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var controller = context.Controller as Controller;

            if (controller == null)
                return;

            if(context.RouteData.Values.ContainsKey(nameof(Location).ToLower()) && Enum.TryParse(context.RouteData.Values[nameof(Location).ToLower()].ToString(), true, out Location location))
                controller.ViewData[nameof(Location)] = location;
            else if(string.IsNullOrWhiteSpace(controller.ViewBag.Section))
                controller.ViewData[nameof(Location)] = _location;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}