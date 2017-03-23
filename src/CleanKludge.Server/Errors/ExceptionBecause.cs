using System;
using CleanKludge.Api.Responses.Articles;

namespace CleanKludge.Server.Errors
{
    public static class ExceptionBecause
    {
        public static Exception UnknownLocation(Location location)
        {
            return new ArgumentException($"Unknown location value '{location}'");
        }
    }
}