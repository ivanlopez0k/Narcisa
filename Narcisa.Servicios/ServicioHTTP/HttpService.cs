using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Narcisa.Servicios.ServicioHTTP
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient http;

        public HttpService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<HttpResp<T?>> GetAsync<T>(string url)
        {
            try
            {
                var response = await http.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    return new HttpResp<T?>(default, true, response, string.IsNullOrWhiteSpace(msg) ? null : msg);
                }

                var dato = await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new HttpResp<T?>(dato, false, response, null);
            }
            catch (Exception e)
            {
                return new HttpResp<T?>(default, true, null, e.Message);
            }
        }

        public async Task<HttpResp<TResp?>> PostAsync<T, TResp>(string url, T DTO)
        {
            try
            {
                var json = JsonSerializer.Serialize(DTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    return new HttpResp<TResp?>(default, true, response, string.IsNullOrWhiteSpace(msg) ? null : msg);
                }

                if (typeof(TResp) == typeof(bool) || typeof(TResp) == typeof(int) || typeof(TResp) == typeof(string))
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    var converted = (TResp)Convert.ChangeType(raw, typeof(TResp));
                    return new HttpResp<TResp?>(converted, false, response, null);
                }

                var dato = await response.Content.ReadFromJsonAsync<TResp>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return new HttpResp<TResp?>(dato, false, response, null);
            }
            catch (Exception e)
            {
                return new HttpResp<TResp?>(default, true, null, e.Message);
            }
        }

        public async Task<HttpResp<TResp?>> PutAsync<T, TResp>(string url, T DTO)
        {
            try
            {
                var json = JsonSerializer.Serialize(DTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PutAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    return new HttpResp<TResp?>(default, true, response, string.IsNullOrWhiteSpace(msg) ? null : msg);
                }

                if (typeof(TResp) == typeof(bool))
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    if (bool.TryParse(raw, out var bVal))
                    {
                        return new HttpResp<TResp?>((TResp)(object)bVal, false, response, null);
                    }
                }

                var dato = await response.Content.ReadFromJsonAsync<TResp>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return new HttpResp<TResp?>(dato, false, response, null);
            }
            catch (Exception e)
            {
                return new HttpResp<TResp?>(default, true, null, e.Message);
            }
        }

        public async Task<HttpResp<TResp?>> DeleteAsync<TResp>(string url)
        {
            try
            {
                var response = await http.DeleteAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    return new HttpResp<TResp?>(default, true, response, string.IsNullOrWhiteSpace(msg) ? null : msg);
                }

                if (typeof(TResp) == typeof(bool))
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    if (bool.TryParse(raw, out var bVal))
                    {
                        return new HttpResp<TResp?>((TResp)(object)bVal, false, response, null);
                    }
                }

                var dato = await response.Content.ReadFromJsonAsync<TResp>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return new HttpResp<TResp?>(dato, false, response, null);
            }
            catch (Exception e)
            {
                return new HttpResp<TResp?>(default, true, null, e.Message);
            }
        }
    }
}
