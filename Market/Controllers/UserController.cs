using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Registration;
using UnitsOfWork;


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
