using System.Net;

namespace QuokkaLabsApi_By_HumiVikash.Models.DTOs.Response
{
    public class Response
    {
        public HttpStatusCode Status { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
