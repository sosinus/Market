using Market.Models.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Models
{
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


        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TEntity>(_marketDBContext, _userManager);
            return (IRepository<TEntity>)_repositories[type];
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
