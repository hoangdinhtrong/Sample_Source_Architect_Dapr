using Dapr.Client;
using System.Net.Http.Json;

namespace SampeDapr.OutService
{
    public abstract class BaseClient
    {
        private readonly DaprClient _client;
        public BaseClient(DaprClient client)
        {
            _client = client;
        }

        protected virtual async Task<T?> GetAsync<T>(string url, string appId, string param)
        {
            var clientRequest = _client.CreateInvokeMethodRequest(HttpMethod.Get, appId, $"{url}?{param}");
            var data = await _client.InvokeMethodWithResponseAsync(clientRequest);
            if (data.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default(T?);
            }
            else if (data.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Something went wrong calling the API {data.RequestMessage}: {data.ReasonPhrase}");
            }

            data.EnsureSuccessStatusCode();
            return await data.Content.ReadFromJsonAsync<T>();
        }

        protected virtual async Task<T?> GetAsync<T>(string url,string appId)
        {
            var clientRequest = _client.CreateInvokeMethodRequest(HttpMethod.Get, appId, $"{url}");
            var data = await _client.InvokeMethodWithResponseAsync(clientRequest);
            if (data.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default(T?);
            }
            else if (data.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Something went wrong calling the API {data.RequestMessage}: {data.ReasonPhrase}");
            }

            data.EnsureSuccessStatusCode();

            return await data.Content.ReadFromJsonAsync<T>();
        }

        protected virtual async Task<T?> PostAsync<T>(string url, string appId,object dataObj)
        {
            var clientRequest = _client.CreateInvokeMethodRequest(HttpMethod.Post, appId, $"{url}", dataObj);
            var data = await _client.InvokeMethodWithResponseAsync(clientRequest);
            if (data.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return default(T?);
            }
            else if (data.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Something went wrong calling the API {data.RequestMessage}: {data.ReasonPhrase}");
            }

            data.EnsureSuccessStatusCode();

            return await data.Content.ReadFromJsonAsync<T>();
        }
    }
}