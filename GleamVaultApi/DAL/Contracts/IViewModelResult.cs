namespace GleamVaultApi.DAL.Contracts
{
    public interface IViewModelResult<T,TResult>
    {
        Task<TResult> GetAsViewModel(Guid id);
        Task<List<TResult>> GetAllAsViewModel();
        TResult MapViewModel(T entity);
    }
}
