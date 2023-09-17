using System.Net;

namespace Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public HttpStatusCode Code { get; set; }

        public bool ShowMessage { get; set; }

        public BadRequestException(string message, HttpStatusCode code) : base(message)
        {
            Code = code;
        }
    }
}
