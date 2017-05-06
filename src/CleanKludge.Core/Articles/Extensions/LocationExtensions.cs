using CleanKludge.Core.Errors;

namespace CleanKludge.Core.Articles.Extensions
{
    public static class LocationExtensions
    {
        public static Api.Responses.Articles.Location ToApiType(this Location self)
        {
            switch (self)
            {
                case Location.About:
                    return Api.Responses.Articles.Location.About;
                case Location.Account:
                    return Api.Responses.Articles.Location.Account;
                case Location.Blog:
                    return Api.Responses.Articles.Location.Blog;
                case Location.Error:
                    return Api.Responses.Articles.Location.Error;
                case Location.Code:
                    return Api.Responses.Articles.Location.Code;
                case Location.Home:
                    return Api.Responses.Articles.Location.Home;
                case Location.Webhook:
                    return Api.Responses.Articles.Location.Webhook;
                default:
                    throw ExceptionBecause.UnknownSection(self);
            }
        }
    }
}