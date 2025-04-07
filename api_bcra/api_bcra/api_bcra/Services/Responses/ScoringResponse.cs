using api_bcra.Models;

namespace api_bcra.Services.Responses
{
    public class ScoringResponse
    {
        public string? Scoring { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
