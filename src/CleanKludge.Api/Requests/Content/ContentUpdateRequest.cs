using Newtonsoft.Json;

namespace CleanKludge.Api.Requests.Content
{
    public class ContentUpdateRequest
    {
        [JsonProperty("pusher")]
        public PusherRequest PusherRequest { get; set; }
    }

    public class PusherRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}