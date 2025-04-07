using api_bcra.Request.interfaces;

namespace api_bcra.Request
{
    public class BCRAClient : IBCRAClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public BCRAClient(HttpClient _client, IConfiguration configuration)
        {
            _httpClient = _client;
            _configuration = configuration;
        }

        public async Task<string> GetFullHistory(string cuit)
        {
            var bcra_cfg = _configuration.GetSection("API_Config");
            var endpoint = bcra_cfg.GetValue<string>("History_endpoint");

            var response = await _httpClient.GetAsync($"{endpoint}{cuit}");
            //response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetSimpleData(string cuit)
        {
            var bcra_cfg = _configuration.GetSection("BCRA_Config");
            var endpoint = bcra_cfg.GetValue<string>("Main_endpoint");

            var response = await _httpClient.GetAsync($"{endpoint}{cuit}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
