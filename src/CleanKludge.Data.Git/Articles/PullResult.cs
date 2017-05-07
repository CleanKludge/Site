using CleanKludge.Api.Responses.Content;

namespace CleanKludge.Data.Git.Articles
{
    public class PullResult
    {
        public PullState State { get; set; }
        public string Message { get; set; }

        public static PullResult Failed(string message)
        {
            return new PullResult { State = PullState.Failed, Message = message };
        }

        public static PullResult Unauthorized(string message)
        {
            return new PullResult { State = PullState.Unauthorized, Message = message };
        }

        public static PullResult Success(string message)
        {
            return new PullResult { State = PullState.Successful, Message = message };
        }

        public ContentUpdateResponse ToResponse()
        {
            return new ContentUpdateResponse
            {
                Message = Message,
                Successful = State == PullState.Successful
            };
        }
    }
}