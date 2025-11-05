namespace GleamVaultApi.DAL.Contracts
{
    public interface IAudit
    {
        Guid Id { get; set; }
        System.DateTime CreatedDate { get; set; }
        string? CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        string? ModifiedBy { get; set; }
       
    }
}
