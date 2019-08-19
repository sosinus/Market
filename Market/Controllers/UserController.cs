using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Registration;
using Models.RepositoryResults;
using System.Threading.Tasks;
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
            var result = _marketUoW.UseUserMngmtRepository().UpdateUser(appUser).Result;           
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateUser(FrontUser frontUser)
        {
            CreateUserResult result = new CreateUserResult();
            LoginRegisterModel loginRegisterModel = new LoginRegisterModel() { UserName = frontUser.UserName, Password = frontUser.Password };
            FrontCustomer frontCustomer = new FrontCustomer() { Address = frontUser.Address, Name = frontUser.Name };
            var userCreationResult = _marketUoW.UseUserMngmtRepository().CreateUser(loginRegisterModel, "User");
            if (userCreationResult.Success)
            {
              var assignResult =  _marketUoW.UseUserMngmtRepository().AssignCustomer(frontCustomer, userCreationResult.UserId).Result;
               if(assignResult.Succeeded) {
                    result.Success = true;
                };
            }
            return Ok(result);
        }
    }
}
