using Market.Models;
using Market.Models.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _marketUoW;
        public AuthController(IUnitOfWork marketUoW, UserManager<AppUser> userManager)
        {
            _marketUoW = marketUoW;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginRegisterModel model)
        {

            var result = _marketUoW.UseUserMngmtRepository().GetJwtToken(model).Result;
            return Ok(result);

        }

        [HttpGet]
        public IActionResult LoadPage()
        {
            if (_marketUoW.UseUserMngmtRepository().GetAllUsers().Length == 0)
                return Ok(new { hasDefaultUser = false });
            else
                return Ok(new { hasDefaultUser = true });
        }

        [HttpPost]
        [Route("CreateUser")]
        public IActionResult Register(LoginRegisterModel user)
        {
            int result = _marketUoW.UseUserMngmtRepository().CreateUser(user, "User");
            //string message = "Пользователь успешно создан";
            if (result == 1)
                return Ok(new { message = "Пользователь успешно создан", status = 200 });
            if (result == 2)
                return Ok(new { message = "Пользователь с таким логином уже существует", status = 400 });
            return StatusCode(500);
        }

        [HttpPost]
        [Route("CreateManager")]
        public IActionResult RegisterManager(LoginRegisterModel user)
        {
            int result = _marketUoW.UseUserMngmtRepository().CreateUser(user, "Manager");
            //string message = "Пользователь успешно создан";
            if (result == 1)
                return Ok(new { message = "Пользователь успешно создан", status = 200 });
            if (result == 2)
                return Ok(new { message = "Пользователь с таким логином уже существует", status = 400 });
            return StatusCode(500);
        }

        [HttpPost]
        [Route("Customer")]
        public IActionResult AddCustomer(FrontCustomer frontCustomer)
        {
            string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            var result = _marketUoW.UseUserMngmtRepository().AssignCustomer(frontCustomer, userId).Result;
            if (result.Succeeded)
                return Ok();
            else return BadRequest();
        }

        [HttpGet]
        [Route("Customer")]
        public IActionResult GetCustomer()
        {
            string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            var result = _marketUoW.UseUserMngmtRepository().GetCustomer(userId);
            if (result != null)
            {
                return Ok(result);
            }
            else return Ok(null);

        }
    }
}