using Models;
using Models.Tables;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IItemRepository
    {
        Task<int> AddNewItem(Item item);
        Item[] GetAllItems();
        int DeleteItem(int id);
        int UpdateItem(Item item);
    }
    public class ItemRepository : IItemRepository
    {
        private readonly MarketDBContext _context;
        public ItemRepository(MarketDBContext context)
        {
            _context = context;
        }

        public async Task<int> AddNewItem(Item item)
        {
            await _context.Items.AddAsync(item);
            int res = await _context.SaveChangesAsync();
            return res;

        }

        public Item[] GetAllItems()
        {
            return _context.Items.ToArray();
        }
        public int DeleteItem(int id)
        {
            Item item = _context.Items.SingleOrDefault(i => i.Id == id);
            _context.Items.Remove(item);
            int result = _context.SaveChanges();
            return result;
        }
        public int UpdateItem(Item item)
        {
            _context.Items.Update(item);
            return _context.SaveChanges();
        }
    }
}
