using System.IO;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "");

        Task<Stream> GetStreamAsync(string uri, string token = "");

        Task<Stream> PostGetStreamAsync<TResult>(string uri, TResult data, string token = "", string header = "");

        Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "");

        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", string header = "");

        Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret);

        Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "");

        Task<TResult> GetHandleOutputAsync<TResult>(string uri, string token = "");

        Task DeleteAsync(string uri, string token = "");

        Task<T> UploadImageAsync<T>(string uri, Stream image, string fileName, string token = "", string header = "");

        Task<TResult> PostStreamAsync<TResult>(string uri, Stream data, string token = "", string header = "");
    }
}