using Microsoft.AspNetCore.Mvc;
using Models.Tables;
using System;
using System.IO;
using System.Linq;
using UnitsOfWork;

namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IUnitOfWork _marketUoW;
        public ItemsController(IUnitOfWork marketUoF)
        {
            _marketUoW = marketUoF;
        }

        [HttpGet]
        // [Authorize(Roles = "Manager, User")]
        public Object Get()
        {
            Item[] items = _marketUoW.UseItemRepository().GetAllItems();
            foreach (Item item in items)
            {
                string folderName = Path.Combine("Resources", "Images", item.Id.ToString());
                if (Directory.Exists(folderName))
                    item.Image = Directory.GetFiles(folderName)[0];
            }
            return items;
        }



        [HttpPost]
        public int NewItem(Item item)
        {
            _marketUoW.UseItemRepository().AddNewItem(item);            
            moveImages(item.Id.ToString());
            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            int result = _marketUoW.UseItemRepository().DeleteItem(id);
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", id.ToString());
            if (Directory.Exists(imgPath))
                Directory.Delete(imgPath, true);
            if (result == 1)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPut]
        public IActionResult UpdateItem(Item item)
        {
            if (_marketUoW.UseItemRepository().UpdateItem(item) == 1)
            {
                moveImages(item.Id.ToString());
                return Ok();
            }
            else
                return BadRequest();

        }

        public void moveImages(string id)
        {
            string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            string srcDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", "Temp", userId);
            string dstDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", id);
            if (Directory.Exists(dstDir))
            {
                foreach (var file in Directory.GetFiles(dstDir))
                {
                    System.IO.File.Delete(file);
                }
            }

            else
                Directory.CreateDirectory(dstDir);
            string[] files = Directory.GetFiles(srcDir);
            foreach (string file in files)
            {
                var fileName = Path.GetFileName(file);
                string dstFile = Path.Combine(dstDir, fileName);
                if (!System.IO.File.Exists(dstFile))
                    System.IO.File.Move(file, dstFile);
            }
            Directory.Delete(srcDir, true);
        }
    }
}
