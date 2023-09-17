using System.Net;

namespace Domain.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode Code { get; set; }

        public HttpException(string message, HttpStatusCode code) : base(message)
        {
            Code = code;
        }
    }
}
