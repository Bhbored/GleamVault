using System.Security.Principal;

namespace GleamVaultApi.DAL.Contracts
{
    public interface IRepository<T> where T : class, IAudit
    {
        Task<T> Get(Guid id);
        Task<List<T>> GetAll();
        Task<T> Delete(Guid id);
        Task<T> Update(T entity, IIdentity user);

        string GetEntitySetName();
    }
}
