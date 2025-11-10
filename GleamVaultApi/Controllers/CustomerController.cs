using GleamVaultApi.DAL.Services;
using GleamVaultApi.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace GleamVaultApi.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public CustomerRepository CustomerRepository { get; }
        public CustomerController(CustomerRepository customerRepository)
        {
         CustomerRepository= customerRepository;
        }

        [HttpGet("GetCustomer")]
        [ApiKeyAuthorize]
        public async Task<ActionResult<IEnumerable<CustomerInfo>>> GetCustomer()
        {
            var result = await CustomerRepository.GetAllAsViewModel();
            return Ok(result);
        }
    }
}
