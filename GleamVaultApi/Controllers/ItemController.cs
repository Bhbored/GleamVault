using GleamVaultApi.DAL.Services;
using GleamVaultApi.DB;
using GleamVaultApi.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace GleamVaultApi.Controllers
{
    [Route("api/item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        public CategoryRepository CategoryRepository { get;}
        public ItemRepository ItemRepository { get;}

        public ItemController(CategoryRepository categoryRepository,ItemRepository itemRepository)
        {
            CategoryRepository = categoryRepository;
            ItemRepository = itemRepository;
        }

        [HttpGet("GetCategories")]
        [ApiKeyAuthorize]
        public async Task<ActionResult<IEnumerable<CategoryInfo>>> GetCategories()
        {
            //var user = HttpContext.Items["User"] as User;           
            try
            {
                var category = await CategoryRepository.GetAllAsViewModel();

                if (category == null)
                {
                    return NotFound(new { error = "Category not found" });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", details = ex.Message });
            }
        }

        [HttpGet("GetItems")]
        [ApiKeyAuthorize]
        public async Task<ActionResult<IEnumerable<ItemInfo>>>GetItems(Guid CategoryID)
        {
                     
            var result= await ItemRepository.GetByCategoryAsViewModel(CategoryID);
            return Ok(result);

        }
    }
}
