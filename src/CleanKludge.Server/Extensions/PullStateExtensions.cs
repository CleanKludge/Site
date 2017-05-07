using System.Net;
using CleanKludge.Data.Git.Articles;

namespace CleanKludge.Server.Extensions
{
    public static class PullStateExtensions
    {
        public static HttpStatusCode ToStatusCode(this PullState self)
        {
            switch(self)
            {
                case PullState.Failed:
                    return HttpStatusCode.InternalServerError;
                case PullState.Successful:
                    return HttpStatusCode.OK;
                case PullState.Unauthorized:
                    return HttpStatusCode.Unauthorized;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}