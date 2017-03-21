using System;
using CleanKludge.Api.Responses.Articles;

namespace CleanKludge.Server.Errors
{
    public static class ExceptionBecause
    {
        public static Exception InvalidUserOperation(string operation)
        {
            return new InvalidOperationException($"Cannot perform user operation '{operation}'.");
        }

        public static Exception InvalidRoleOperation(string operation)
        {
            return new InvalidOperationException($"Cannot perform role operation '{operation}'.");
        }

        public static Exception UnknownLocation(Location location)
        {
            return new ArgumentException($"Unknown location value '{location}'");
        }
    }
}