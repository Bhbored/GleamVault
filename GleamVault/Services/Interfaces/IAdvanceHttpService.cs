using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Services.Interfaces
{
    public interface IAdvanceHttpService
    {
        Task<T> Get<T>(string Url);
        Task<HttpResult<TResponse>> Post<TRequest, TResponse>(string url, TRequest Data);

        Task<bool> Delete(string baseUrl, Guid id);
        Task<HttpResult<TResponse>> Delete<TResponse>(string baseUrl, Guid id);
    }
}
