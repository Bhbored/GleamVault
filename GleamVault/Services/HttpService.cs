using GleamVault.Services.Interfaces;
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
      

        public HttpService()
        {
            _httpClient = new HttpClient();
           
            //_httpClient.DefaultRequestHeaders.Accept.Add(
            //   new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> Delete(string baseUrl, Guid id)
        {
            try
            {
                //AddAuthHeader();
                var url = $"{baseUrl}/{id}";
                var response = await _httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DELETE Error: {ex.Message}");
                return false;
            }
        }

        public async Task<HttpResult<TResponse>> Delete<TResponse>(string baseUrl, Guid id)
        {
            try
            {
                //AddAuthHeader();
                var url = $"{baseUrl}/{id}";
                var response = await _httpClient.DeleteAsync(url);

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
                Debug.WriteLine($"DELETE Error: {ex.Message}");
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
                //AddAuthHeader();

                var response = await _httpClient.GetAsync(url);



                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Debug.WriteLine($"GET request failed with status: {response.StatusCode} - {response.ReasonPhrase}");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GET Error: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                return default(T);
            }
        }


        public async Task<HttpResult<TResponse>> Post<TRequest, TResponse>(string url, TRequest Data)
        {
            try
            {
                //AddAuthHeader();

                var json = JsonSerializer.Serialize(Data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await _httpClient.PostAsync(url, content);



                var responseJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"   Response JSON: {responseJson}");

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
