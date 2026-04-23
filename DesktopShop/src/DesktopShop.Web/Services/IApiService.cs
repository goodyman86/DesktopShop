using System.Net.Http;
using System.Threading.Tasks;

namespace DesktopShop.Web.Services;

public interface IApiService
{
    Task<T?> GetAsync<T>(string endpoint);
    Task<T?> PostAsync<T>(string endpoint, object data);
    Task<T?> PutAsync<T>(string endpoint, object data);
    Task<bool> DeleteAsync(string endpoint);
    Task<bool> PatchAsync(string endpoint, object data);
    Task<T?> PostFormAsync<T>(string endpoint, MultipartFormDataContent content);
}
