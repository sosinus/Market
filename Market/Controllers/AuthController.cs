using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Registration;
using Models.RepositoryResults;
using Models.Tables;
using System.Linq;
using UnitsOfWork;

namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _marketUoW;
        public AuthController(IUnitOfWork marketUoW)
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
            LoadPageResult result = new LoadPageResult();
            if (_marketUoW.UseUserMngmtRepository().GetAllUsers().Length == 0)
                result.HasDefaultUser = false;
            else
                result.HasDefaultUser = true;
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateUser")]
        public IActionResult Register(LoginRegisterModel user)
        {
            CreateUserResult result = _marketUoW.UseUserMngmtRepository().CreateUser(user, "User");
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateManager")]
        public IActionResult RegisterManager(LoginRegisterModel user)
        {
            CreateUserResult result = _marketUoW.UseUserMngmtRepository().CreateUser(user, "Manager");
            return Ok(result);
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
            Customer result = _marketUoW.UseUserMngmtRepository().GetCustomer(userId);
            if (result != null)
            {
                return Ok(result);
            }
            else return Ok();
        }
    }
}