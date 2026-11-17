using System.Threading.Tasks;

namespace Narcisa.Servicios.ServicioHTTP
{
    public interface IHttpService
    {
        Task<HttpResp<T?>> GetAsync<T>(string url);
        Task<HttpResp<TResp?>> PostAsync<T, TResp>(string url, T DTO);
        Task<HttpResp<TResp?>> PutAsync<T, TResp>(string url, T DTO);
        Task<HttpResp<TResp?>> DeleteAsync<TResp>(string url);
    }
}
