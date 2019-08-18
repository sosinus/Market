using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.RepositoryResults;
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
        public IActionResult Get()
        {
            Item[] items = _marketUoW.UseItemRepository().GetAllItems();
            foreach (Item item in items)
            {
                string folderName = Path.Combine("Resources", "Images", item.Id.ToString());
                if (Directory.Exists(folderName))
                    item.Image = Directory.GetFiles(folderName)[0];
            }
            return Ok(items);
        }



        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult NewItem(Item item)
        {
            ItemResult result = _marketUoW.UseItemRepository().AddNewItem(item).Result;
            moveImages(item.Id.ToString());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteItem(int id)
        {
            ItemResult result = _marketUoW.UseItemRepository().DeleteItem(id);
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", id.ToString());
            if (Directory.Exists(imgPath))
                Directory.Delete(imgPath, true);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public IActionResult UpdateItem(Item item)
        {
            ItemResult result = _marketUoW.UseItemRepository().UpdateItem(item);
            moveImages(item.Id.ToString());
            return Ok(result);

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
