using Market.Models.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public class UnitOfWork : IUnitOfWork
    {
        private readonly MarketDBContext _marketDBContext;
        private readonly UserManager<AppUser> _userManager;
        private Dictionary<Type, object> _repositories;
        private readonly ApplicationSettings _appSettings;
        public UnitOfWork(MarketDBContext marketDBContext, UserManager<AppUser> userManager, IOptions<ApplicationSettings> appSettings)
        {
            _marketDBContext = marketDBContext;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<int> CommitAsync()
        {
            return await _marketDBContext.SaveChangesAsync();
        }

        public IUMRepository UseUserMngmtRepository()
        {
            return new UMRepository(_userManager, _appSettings, _marketDBContext);
        }

        public IItemRepository UseItemRepository()
        {
            return new ItemRepository(_marketDBContext);
        }
        public IOrderRepository UseOrderRepository()
        {
            return new OrderRepository(_marketDBContext, _userManager);
        }

    }

}
