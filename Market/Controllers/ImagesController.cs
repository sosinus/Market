using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult GetImages(string id)
        {
            if (id != null)
            {
                string folderName = Path.Combine("Resources", "Images", id);
                if (Directory.Exists(folderName))
                {
                    string[] images = Directory.GetFiles(folderName);
                    return Ok(images);
                }
                else return Ok(null);

            }
            return BadRequest();
        }

        [HttpGet]
        [Route("ForEdit/{id}")]
        public IActionResult GetImagesForEdit(string id)
        {
            if (id != null)
            {
                string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
                string folderName = Path.Combine("Resources", "Images", "Temp", userId);
                copyImages(id, userId);
                if (Directory.Exists(folderName))
                {
                    string[] images = Directory.GetFiles(folderName);
                    return Ok(images);
                }
                else
                    return Ok(null);

            }
            return BadRequest();
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        public IActionResult Upload()
        {
            try
            {
                List<string> imgPathes = new List<string>();
                string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
                var files = Request.Form.Files;
                if (userId != null && files.Count > 0)
                {
                    string folderName = Path.Combine("Resources", "Images", "Temp", userId);
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                    string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    foreach (var file in files)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var imgPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        imgPathes.Add(imgPath);
                    }

                    return Ok(new { imgPathes });


                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error" + ex);
            }
        }
        [HttpDelete]
        [Route("deleteAll")]
        public IActionResult DeleteAll()
        {

            var userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            if (userId != null)
            {
                var folderName = Path.Combine("Resources", "Images", "Temp", userId);
                var pathWhereSaved = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (Directory.Exists(pathWhereSaved))
                {
                    Directory.Delete(pathWhereSaved, true);
                }
                if (!Directory.Exists(pathWhereSaved))
                    return Ok();
                else
                    return StatusCode(500);
            }
            else
                return StatusCode(500);
        }

        [HttpDelete]
        [Route("deleteOne/{imageName}")]
        public IActionResult DeleteOne(string imageName)
        {

            var userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            if (userId != null)
            {
                var folderName = Path.Combine("Resources", "Images", "Temp", userId);
                if (Directory.Exists(folderName))
                {
                    var pathWhereSaved = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    string file = Path.Combine(pathWhereSaved, imageName);

                    System.IO.File.Delete(file);
                    string[] imgPathes = Directory.GetFiles(folderName);
                    return Ok(new { imgPathes });
                }
            }
            return BadRequest();
        }

        public void copyImages(string id, string userId)
        {
            string dstDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", "Temp", userId);
            if (Directory.Exists(dstDir))
            {
                foreach (var file in Directory.GetFiles(dstDir))
                {
                    System.IO.File.Delete(file);
                }
            }
            else
                Directory.CreateDirectory(dstDir);


            string srcDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", id);
            
            string[] files = Directory.GetFiles(srcDir);
            foreach (string file in files)
            {
                var fileName = Path.GetFileName(file);
                string dstFile = Path.Combine(dstDir, fileName);
                System.IO.File.Copy(file, dstFile);
            }

        }

    }
}