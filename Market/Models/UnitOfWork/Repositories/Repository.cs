using Market.Models.Registration;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Models
{
    public interface IRepository<T>  where T : class
    {
        Task<IdentityResult> AddNewUser();
        Array GetAllUsers();
        Array GetAll();
        void Add(T entity);
        void AddRange(params T[] entities);
    }

    class Repository<T> : IRepository<T> where T : class
    {
        private readonly MarketDBContext _context;
        public UserManager<AppUser> _userManager;
        private MarketDBContext marketDBContext;

        public Repository(MarketDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public Array GetAllUsers()
        {
            return _userManager.Users.ToArray();
        }

        public async Task<IdentityResult> AddNewUser()
        {
            IdentityResult result = await _userManager.CreateAsync(new AppUser { UserName = "Dimma", Customer_Id = null });
            return result;
        }
        public Array GetAll()
        {
            var itemArray = _context.Set<T>().ToArray();
            return itemArray;
        }

        public void Add(T entity)
        {
            if (entity != null)
                _context.Set<T>().Add(entity);
        }

        public void AddRange(params T[] entities)
        {
            if (entities != null)
                _context.Set<T>().AddRange(entities);
        }




    }
}
