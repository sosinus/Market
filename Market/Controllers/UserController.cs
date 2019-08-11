using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Market.Models;
using Market.Models.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _marketUoW;

        public UserController(IUnitOfWork marketUoW)
        {
            _marketUoW = marketUoW;
        }

        // GET: /<controller>/
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IActionResult GetUsers()
        {
           var users = _marketUoW.UseUserMngmtRepository().GetAllUsers();
            return Ok(users);
        }

        [HttpPut]
        public IActionResult UpdateUser(AppUser appUser)
        {
          return Ok(_marketUoW.UseUserMngmtRepository().UpdateUser(appUser));
        }
    }
}
