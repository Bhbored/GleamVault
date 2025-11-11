using GleamVaultApi.DAL.Services;
using GleamVaultApi.DB;
using GleamVaultApi.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Transaction = Shared.Models.Transaction;

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

        [HttpGet("GetTransactionItem")]
        [ApiKeyAuthorize]
        public async Task<ActionResult<IEnumerable<Shared.Models.TransactionItem>>> GetTransactionItems(Guid TransactionID)
        {
            if (TransactionID == Guid.Empty) 
            {
              return BadRequest("transactionID is required");
            }
            var result = await TransactionRepository.GetTransactionItems(TransactionID);
            return Ok(result);
        }

        [HttpPost("SaveTransaction")]
        public async Task<ActionResult<IEnumerable<Transaction>>> SaveTransaction([FromBody] Transaction transaction)
        {
            var user = HttpContext.Items["User"] as User;
            if (user == null)
            {
                return Unauthorized(new { error = "User not found" });
            }


            var userIdentity = new UserIdentity(user);

            var savedTransaction = await TransactionRepository.SaveTransaction(transaction, userIdentity);
            return Ok(savedTransaction);
        }
    }
}
