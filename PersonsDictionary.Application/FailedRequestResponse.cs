using System.Text.Json;

namespace Application
{
    public class FailedRequestResponse
    {
        public FailedRequestResponse()
        {
            Details = new Dictionary<string, string>();
        }

        public string Message { get; set; }

        public string ErrorCode { get; set; }

        public IDictionary<string, string> Details { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
