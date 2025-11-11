using GleamVaultApi.DAL.Services;
using GleamVaultApi.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace GleamVaultApi.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public TransactionRepository TransactionRepository { get; }
        public TransactionController(TransactionRepository transactionRepository) 
        {
          TransactionRepository = transactionRepository;
        }

        [HttpGet("GetTransaction")]
        [ApiKeyAuthorize]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransaction()
        {
            var result = await TransactionRepository.GetAllAsViewModel();
            return Ok(result);
        }
    }
}
