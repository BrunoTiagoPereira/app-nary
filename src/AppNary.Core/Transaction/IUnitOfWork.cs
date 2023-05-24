namespace AppNary.Core.Transaction
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}