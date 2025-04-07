using api_bcra.Models;

namespace api_bcra.Services.Responses
{
    public class UserActionResponse
    {
        public User? _User { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public ICollection<User>? _Users { get; set; }
    }
}
