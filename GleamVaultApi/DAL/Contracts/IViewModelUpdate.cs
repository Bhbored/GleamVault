namespace GleamVaultApi.DAL.Contracts
{
    public interface IViewModelUpdate<TViewModel, TEntity>
    {
        Task<TEntity> UpdateByViewModel(TViewModel viewmodel);
        TEntity MapViewModelToEntity(TViewModel viewmodel);
    }
}
