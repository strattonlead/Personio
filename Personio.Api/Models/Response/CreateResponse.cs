using Newtonsoft.Json;

namespace Personio.Api.Models.Response
{
    public class CreateResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ErrorObject Error { get; set; }

        public static CreateResponse<TItem> FromData<TItem>(TItem item)
            => new CreateResponse<TItem> { Data = item, Success = true };

        public static CreateResponse<TItem> FromError<TItem>(CreateResponse createResponse)
            => new CreateResponse<TItem>() { Error = createResponse.Error };
    }

    public class CreateResponse<T> : CreateResponse
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }
}
