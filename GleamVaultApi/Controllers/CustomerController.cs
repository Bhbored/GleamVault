using GleamVaultApi.DAL.Services;
using GleamVaultApi.DB;
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

        [HttpPost("SaveCustomer")]
        [ApiKeyAuthorize]
        public async Task<ActionResult<CustomerInfo>> SaveCustomer([FromBody] Shared.Models.Customer customer)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;
                if (user == null)
                {
                    return Unauthorized(new { error = "User not found" });
                }

                var userIdentity = new UserIdentity(user);
                var result = await CustomerRepository.SaveAsync(customer, userIdentity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while saving customer", details = ex.Message });
            }
        }


        [HttpDelete("DeleteCustomer/{id}")]
        [ApiKeyAuthorize]
        public async Task<ActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;
                if (user == null)
                {
                    return Unauthorized(new { error = "User not found" });
                }

                var result = await CustomerRepository.DeleteAsync(id);

                if (result)
                {
                    return Ok(new { message = "Customer deleted successfully" });
                }
                else
                {
                    return NotFound(new { error = "Customer not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while deleting customer", details = ex.Message });
            }
        }
    }
}
