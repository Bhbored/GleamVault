using GleamVaultApi.DAL.Contracts;
using GleamVaultApi.DB;
using Shared.Models;
using System.Security.Principal;
using Category = GleamVaultApi.DB.Category;

namespace GleamVaultApi.DAL.Services
{
    public class CategoryRepository : BaseServiceClass<Category>, IViewModelResult<Category, CategoryInfo>
    {
        public async Task<List<CategoryInfo>> GetAllAsViewModel()
        {
            var result = await GetAll();
            return result.Select(MapViewModel).ToList();
        }

        public Task<CategoryInfo> GetAsViewModel(Guid id)
        {
            throw new NotImplementedException();
        }
        protected override Category Map(Category original, Category sourceEntity)
        {
            if (original == null || sourceEntity == null)
                return original;

            original.Name = sourceEntity.Name;
            original.Description = sourceEntity.Description;
            original.Icon = sourceEntity.Icon;
         

            return original;
        }
        public CategoryInfo MapViewModel(Category entity)
        {
            if (entity == null) return null;

            return new CategoryInfo()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public async Task<CategoryInfo> SaveAsync(Shared.Models.Category categoryInfo, IIdentity user) 
        {
            if (categoryInfo == null)
                throw new ArgumentNullException(nameof(categoryInfo), "Category data is required");

            var entity = new DB.Category() 
            {
                Id = categoryInfo.Id,
                Name = categoryInfo.Name,
                Icon = categoryInfo.Icon,
                Description = categoryInfo.Description
            };

            var result = await Update(entity, user);
            return MapViewModel(result);
        }
    }
}
