using Models;
using Models.RepositoryResults;
using Models.Tables;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IItemRepository
    {
        Task<ItemResult> AddNewItem(Item item);
        Item[] GetAllItems();
        ItemResult DeleteItem(int id);
        ItemResult UpdateItem(Item item);
    }
    public class ItemRepository : IItemRepository
    {
        private readonly MarketDBContext _context;
        public ItemRepository(MarketDBContext context)
        {
            _context = context;
        }

        public async Task<ItemResult> AddNewItem(Item item)
        {
            ItemResult result = new ItemResult();
            await _context.Items.AddAsync(item);
            int changedItemsCount = await _context.SaveChangesAsync();
            if (changedItemsCount > 0)
                result.Success = true;
            return result;

        }

        public Item[] GetAllItems()
        {
            return _context.Items.ToArray();
        }
        public ItemResult DeleteItem(int id)
        {
            ItemResult result = new ItemResult();
            Item item = _context.Items.SingleOrDefault(i => i.Id == id);
            _context.Items.Remove(item);
            int SaveChangesResult = _context.SaveChanges();
            if (SaveChangesResult > 0)
                result.Success = true;
            return result;
        }


        public ItemResult UpdateItem(Item item)
        {
            ItemResult result = new ItemResult();
            _context.Items.Update(item);
            int SaveChangesResult = _context.SaveChanges();
            if(SaveChangesResult > 0)
            {
                result.Success = true;
            }
            return result;
        }
    }
}
