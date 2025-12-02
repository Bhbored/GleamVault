using GleamVault.Services.Interfaces;
using GleamVaultApp.Converters;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GleamVault.Services
{
    public class HttpService: IAdvanceHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ISessionService _sessionService;


        public HttpService(ISessionService sessionService)
        {
            //_httpClient = new HttpClient();
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30) 
            };
            _sessionService = sessionService;
           
            //_httpClient.DefaultRequestHeaders.Accept.Add(
            //   new MediaTypeWithQualityHeaderValue("application/json"));
        }


        private HttpRequestMessage CreateRequestWithApiKey(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, url);

            try
            {
              
                var key = _sessionService.GetApiKey();
                

                if (!string.IsNullOrWhiteSpace(key))
                {
                  
                    request.Headers.Add("X-API-Key", key);
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HTTP] ERROR in CreateRequestWithApiKey: {ex.GetType().Name}");
                Debug.WriteLine($"[HTTP] ERROR Message: {ex.Message}");
                Debug.WriteLine($"[HTTP] ERROR Stack: {ex.StackTrace}");
            }

            return request;
        }
        public async Task<bool> Delete(string baseUrl, Guid id)
        {
            try
            {
                var url = $"{baseUrl}/{id}";
                var request = CreateRequestWithApiKey(HttpMethod.Delete, url);
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }

        public async Task<HttpResult<TResponse>> Delete<TResponse>(string baseUrl, Guid id)
        {
            try
            {
                var url = $"{baseUrl}/{id}";
                var request = CreateRequestWithApiKey(HttpMethod.Delete, url);
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.Headers.ContentLength > 0)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<TResponse>(responseJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return new HttpResult<TResponse>
                        {
                            Result = result,
                            IsSuccess = true
                        };
                    }
                    else
                    {
                        return new HttpResult<TResponse>
                        {
                            Result = default(TResponse),
                            IsSuccess = true
                        };
                    }
                }

                return new HttpResult<TResponse>
                {
                    Result = default(TResponse),
                    IsSuccess = false,
                    ErrorMessage = $"HTTP {response.StatusCode}: {response.ReasonPhrase}"
                };
            }
            catch (Exception ex)
            {
                
                return new HttpResult<TResponse>
                {
                    Result = default(TResponse),
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }



        public async Task<T> Get<T>(string url)
        {
           

            try
            {
              
                var request = CreateRequestWithApiKey(HttpMethod.Get, url);

                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

                

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                  
                    return default;
                }

                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters =
            {
                new SafeEnumConverter() 
            }
                };

              
                var result = JsonSerializer.Deserialize<T>(jsonString, options);

                return result;
            }
            catch (JsonException ex)
            {
             
                return default;
            }
            catch (TaskCanceledException ex)
            {
               
                return default;
            }
            catch (Exception ex)
            {
               
                return default;
            }
        }

        public async Task<HttpResult<TResponse>> Post<TRequest, TResponse>(string url, TRequest Data)
        {
            try
            {
                var json = JsonSerializer.Serialize(Data);
                var request = CreateRequestWithApiKey(HttpMethod.Post, url);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);

                var responseJson = await response.Content.ReadAsStringAsync();
              

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<TResponse>(responseJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new HttpResult<TResponse>
                    {
                        Result = result,
                        IsSuccess = true
                    };
                }

                return new HttpResult<TResponse>
                {
                    Result = default(TResponse),
                    IsSuccess = false
                };
            }
            catch (Exception ex)
            {
                return new HttpResult<TResponse>
                {
                    Result = default(TResponse),
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
        //private void AddAuthHeader()
        //{

        //    var token = AuthenticationHelper.GetAccessToken(_appName);

        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        _httpClient.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", token);
        //    }
        //}






    }
}
