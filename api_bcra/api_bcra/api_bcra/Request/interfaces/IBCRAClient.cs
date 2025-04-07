namespace api_bcra.Request.interfaces
{
    public interface IBCRAClient
    {
        Task<string> GetFullHistory(string cuit);
        Task<string> GetSimpleData(string cuit);
    }
}
