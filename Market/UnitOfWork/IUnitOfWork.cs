using System;
using System.Threading.Tasks;

namespace Market.Models
{
    public interface IUnitOfWork
    {
        IUMRepository UseUserMngmtRepository();
        IItemRepository UseItemRepository();
        IOrderRepository UseOrderRepository();
        Task<int> CommitAsync();
    }
}


