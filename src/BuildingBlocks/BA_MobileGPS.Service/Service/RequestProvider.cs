using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        private HttpClient CreateHttpClient(string token = "")
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                token = StaticSettings.Token;
            }
            Debug.WriteLine("HTTP-TOKENT: " + token);
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ServerConfig.ApiEndpoint)
            };
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache"); // <-- doesn't seem to have any effect
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return httpClient;
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            try
            {
                using (var httpClient = CreateHttpClient(token))
                {
                    Debug.WriteLine("HTTP-GET: " + httpClient.BaseAddress + uri);

                    using (var response = await httpClient.GetAsync(uri))
                    {
                        await HandleResponse(response);

                        string serialized = await response.Content.ReadAsStringAsync();

                        return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }

            return default;
        }

        public async Task<Stream> GetStreamAsync(string uri, string token = "")
        {
            try
            {
                using (var httpClient = CreateHttpClient(token))
                {
                    Debug.WriteLine("HTTP-GET-STREAM: " + httpClient.BaseAddress + uri);

                    using (var response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStreamAsync();
                        }
                        else
                        {
                            var content = await response.Content.ReadAsStringAsync();

                            Debug.WriteLine("HTTP-ERROR: " + response.RequestMessage.RequestUri + " :" + content);
                            Logger.WriteError($"status :{response.StatusCode} content:{content}");

                            if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                            {
                            }

                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }

            return default;
        }

        //public async Task<Stream> PostGetStreamAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        //{
        //    try
        //    {
        //        using (var httpClient = CreateHttpClient(token))
        //        {
        //            if (!string.IsNullOrEmpty(header))
        //            {
        //                AddHeaderParameter(httpClient, header);
        //            }

        //            var content = JsonConvert.SerializeObject(data);

        //            var stringContent = new StringContent(content, Encoding.UTF8);
        //            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //            Debug.WriteLine("HTTP-POST: " + httpClient.BaseAddress + uri);
        //            Debug.WriteLine("HTTP-POST-CONTENT: " + content);

        //            using (var response = await httpClient.PostAsync(uri, stringContent))
        //            {
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    return await response.Content.ReadAsStreamAsync();
        //                }
        //                else
        //                {
        //                    var contentResponse = await response.Content.ReadAsStringAsync();

        //                    Debug.WriteLine("HTTP-ERROR: " + response.RequestMessage.RequestUri + " :" + contentResponse);
        //                    Logger.WriteError($"status :{response.StatusCode} content:{contentResponse}");

        //                    if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
        //                    {
        //                    }

        //                    return null;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteError($"HandleResponse-error:{ex.Message}");
        //    }

        //    return default;
        //}

        public async Task<Stream> PostGetStreamAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            httpClient.BaseAddress = new Uri(ServerConfig.ApiEndpoint);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = JsonConvert.SerializeObject(data);

            var stringContent = new StringContent(content, Encoding.UTF8);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Debug.WriteLine("HTTP-POST: " + httpClient.BaseAddress + uri);
            Debug.WriteLine("HTTP-POST-CONTENT: " + content);
            HttpResponseMessage response = await httpClient.PostAsync(uri, stringContent);

            if (!response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();

                Debug.WriteLine("HTTP-ERROR: " + response.RequestMessage.RequestUri + " :" + contentResponse);

                Logger.WriteError($"status :{response.StatusCode} content:{contentResponse}");

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                }

                return null;
            }
            else
            {
                return await response.Content.ReadAsStreamAsync();
            }
        }

        public async Task<TResult> GetHandleOutputAsync<TResult>(string uri, string token = "")
        {
            try
            {
                using (var httpClient = CreateHttpClient(token))
                {
                    Debug.WriteLine("HTTP-GET: " + httpClient.BaseAddress + uri);

                    using (var response = await httpClient.GetAsync(uri))
                    {
                        return await HandleResponseWithResult<TResult>(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }

            return default;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            try
            {
                using (var httpClient = CreateHttpClient(token))
                {
                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    var content = JsonConvert.SerializeObject(data);

                    var stringContent = new StringContent(content, Encoding.UTF8);
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    Debug.WriteLine("HTTP-POST: " + httpClient.BaseAddress + uri);
                    Debug.WriteLine("HTTP-POST-CONTENT: " + content);

                    using (var response = await httpClient.PostAsync(uri, stringContent))
                    {
                        await HandleResponse(response);

                        string serialized = await response.Content.ReadAsStringAsync();

                        return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }

            return default;
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", string header = "")
        {
            try
            {
                using (var httpClient = CreateHttpClient(token))
                {
                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    var content = JsonConvert.SerializeObject(data);

                    var stringContent = new StringContent(content, Encoding.UTF8);
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    Debug.WriteLine("HTTP-POST: " + httpClient.BaseAddress + uri);
                    Debug.WriteLine("HTTP-POST-CONTENT: " + content);

                    using (var response = await httpClient.PostAsync(uri, stringContent))
                    {
                        return await HandleResponseWithResult<TResult>(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }

            return default;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret)
        {
            try
            {
                using (var httpClient = CreateHttpClient())
                {
                    if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
                    {
                        AddBasicAuthenticationHeader(httpClient, clientId, clientSecret);
                    }

                    var stringContent = new StringContent(data);
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    Debug.WriteLine("HTTP-POST: " + httpClient.BaseAddress + uri);
                    Debug.WriteLine("HTTP-POST-CONTENT: " + data);

                    using (var response = await httpClient.PostAsync(uri, stringContent))
                    {
                        await HandleResponse(response);

                        string serialized = await response.Content.ReadAsStringAsync();

                        return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }

            return default;
        }

        public async Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            try
            {
                using (var httpClient = CreateHttpClient(token))
                {
                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    var content = new StringContent(JsonConvert.SerializeObject(data));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    Debug.WriteLine("HTTP-PUT: " + httpClient.BaseAddress + uri);

                    using (var response = await httpClient.PutAsync(uri, content))
                    {
                        await HandleResponse(response);

                        string serialized = await response.Content.ReadAsStringAsync();

                        return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }

            return default;
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            try
            {
                using (var httpClient = CreateHttpClient(token))
                {
                    Debug.WriteLine("HTTP-DELETE: " + httpClient.BaseAddress + uri);

                    using (await httpClient.DeleteAsync(uri))
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }
        }

        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private void AddBasicAuthenticationHeader(HttpClient httpClient, string clientId, string clientSecret)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                return;

            httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }

        private async Task<TResult> HandleResponseWithResult<TResult>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            Debug.WriteLine("HTTP-RESPONSE: " + response.RequestMessage.RequestUri + ": " + content);

            TResult result = default;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Logger.WriteError($"status :{response.StatusCode} content:{content}");
                }
                var message = await Task.Run(() => JsonConvert.DeserializeObject<ResponeMessage>(content, _serializerSettings));
            }
            else
            {
                result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(content, _serializerSettings));
            }

            return result;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();

                Debug.WriteLine("HTTP-RESPONSE: " + response.RequestMessage.RequestUri + ": " + content);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Logger.WriteError($"status :{response.StatusCode} content:{content}");
                    }
                    Logger.WriteError($"status :{response.StatusCode} content:{content}");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }
        }

        public async Task<T> UploadImageAsync<T>(string uri, Stream image, string fileName, string token = "", string header = "")
        {
            try
            {
                using (var httpClient = CreateHttpClient(token))
                {
                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    HttpContent fileStreamContent = new StreamContent(image);
                    fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(fileStreamContent);
                        Debug.WriteLine("HTTP-POST: " + httpClient.BaseAddress + uri);

                        using (var response = await httpClient.PostAsync(uri, formData))
                        {
                            return await HandleResponseWithResult<T>(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"HandleResponse-error:{ex.Message}");
            }

            return default;
        }

        public async Task<TResult> PostStreamAsync<TResult>(string uri, Stream data, string token = "", string header = "")
        {
            try
            {
                using (var httpClient = CreateHttpClient(token))
                {
                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    StreamContent content = new StreamContent(data);
                    //content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                    Debug.WriteLine("HTTP-POST: " + httpClient.BaseAddress + uri);

                    using (var response = await httpClient.PostAsync(uri, content))
                    {
                        await HandleResponse(response);

                        string serialized = await response.Content.ReadAsStringAsync();

                        return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    internal class ResponeMessage
    {
        public string Message { get; set; }
    }

    internal enum TypeMessage
    {
        Image = 1,
        File = 2,
        Http = 3
    }
}