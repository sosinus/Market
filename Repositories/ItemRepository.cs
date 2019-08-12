using Market.Models;
using Market.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

    public interface IItemRepository
    {
        Task AddNewItem(Item item);
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

        public async Task AddNewItem(Item item)
        {
            await _context.Items.AddAsync(item);

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
