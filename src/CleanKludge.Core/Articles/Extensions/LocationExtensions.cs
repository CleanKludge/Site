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
                case Location.Experiments:
                    return Api.Responses.Articles.Location.Experiments;
                case Location.Home:
                    return Api.Responses.Articles.Location.Home;
                default:
                    throw ExceptionBecause.UnknownSection(self);
            }
        }

        public static Location ToCoreType(this Api.Responses.Articles.Location self)
        {
            switch (self)
            {
                case Api.Responses.Articles.Location.About:
                    return Location.About;
                case Api.Responses.Articles.Location.Account:
                    return Location.Account;
                case Api.Responses.Articles.Location.Blog:
                    return Location.Blog;
                case Api.Responses.Articles.Location.Error:
                    return Location.Error;
                case Api.Responses.Articles.Location.Experiments:
                    return Location.Experiments;
                case Api.Responses.Articles.Location.Home:
                    return Location.Home;
                default:
                    throw ExceptionBecause.UnknownSection(self);
            }
        }
    }
}