using CleanKludge.Api.Responses;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Server.Errors;

namespace CleanKludge.Server.Extensions
{
    public static class LocationExtensions
    {
        public static Core.Articles.Location ToCoreType(this Location self)
        {
            switch(self)
            {
                case Location.About:
                    return Core.Articles.Location.About;
                case Location.Account:
                    return Core.Articles.Location.Account;
                case Location.Blog:
                    return Core.Articles.Location.Blog;
                case Location.Error:
                    return Core.Articles.Location.Error;
                case Location.Code:
                    return Core.Articles.Location.Code;
                case Location.Home:
                    return Core.Articles.Location.Home;
                default:
                    throw ExceptionBecause.UnknownLocation(self);
            }
        }
    }
}