using System;
using System.Threading.Tasks;

namespace Market.Models
{
    public interface IUnitOfWork 
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IUMRepository UseUserMngmtRepository();
        IItemRepository UseItemRepository();
        IOrderRepository UseOrderRepository();
        Task<int> CommitAsync();
    }
}


