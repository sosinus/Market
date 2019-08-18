using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Registration;
using Models.RepositoryResults;
using Models.Tables;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUMRepository
    {
        CreateUserResult CreateUser(LoginRegisterModel user, string role);
        Array GetAllUsers();
        Task<GetJwtResult> GetJwtToken(LoginRegisterModel loginModel);
        Task<IdentityResult> AssignCustomer(FrontCustomer frontCustomer, string userId);
        Customer GetCustomer(string id);
        Task UpdateUser(AppUser appUser);
    }

    public class UMRepository : IUMRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationSettings _appSettings;
        private readonly MarketDBContext _context;
        public UMRepository(UserManager<AppUser> userManager, ApplicationSettings appSettings, MarketDBContext context)
        {
            _userManager = userManager;
            _appSettings = appSettings;
            _context = context;
        }

        public CreateUserResult CreateUser(LoginRegisterModel user, string role)
        {
            CreateUserResult result = new CreateUserResult();
            AppUser appuser = new AppUser() { UserName = user.UserName, Email = user.Email };
            try
            {
                if (_userManager.Users.SingleOrDefault(u => u.UserName == user.UserName) != null)
                {
                    result.Message = "Пользователь с таким логином уже существует";
                    result.Success = false;
                    result.IsAlreadyExist = true;
                }
                else
                {
                    IdentityResult userCreation = _userManager.CreateAsync(appuser, user.Password).Result;
                    IdentityResult inRoleAdding = _userManager.AddToRoleAsync(appuser, role).Result;
                    if (userCreation.Succeeded)
                    {
                        if (inRoleAdding.Succeeded)
                            result.IsAlreadyExist = false;
                            result.Success = true;
                            result.Message = "Пользователь успешно создан";
                        AppUser appUser = _userManager.Users.SingleOrDefault(u => u.UserName == user.UserName);
                        _userManager.DeleteAsync(appUser);
                        result.IsAlreadyExist = false;
                        result.Other = true;
                        result.Message = "Не удалось создать пользователя";
                    }                    
                }
            }
            catch (Exception ex)
            {
                result.Message = "Не удалось создать пользователя " + ex;
            }
            return result;
        }

        public Array GetAllUsers()
        {
            return _userManager.Users.Include(u => u.Customer).ToArray();
        }

        public async Task<GetJwtResult> GetJwtToken(LoginRegisterModel loginModel)
        {
            GetJwtResult resutl = new GetJwtResult();
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            IdentityOptions _options = new IdentityOptions();
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]{
                        new Claim("UserID", user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType, role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWTSecret)),
                    SecurityAlgorithms.HmacSha256)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                resutl.Token = token;
                resutl.Success = true;
                return resutl;

            }
            else
            {
                resutl.Success = false;
                return resutl;
            }
        }

        public async Task<IdentityResult> AssignCustomer(FrontCustomer frontCustomer, string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null && user.Customer_Id == null)
            {
                Customer customer = new Customer()
                {
                    Address = frontCustomer.Address,
                    Name = frontCustomer.Name,
                    Code = "0"
                };
                if (customer.Address != null && customer.Name != null)
                {
                    await _context.Customers.AddAsync(customer);
                    await _context.SaveChangesAsync();
                    customer = _context.Customers.Last();
                    customer.Code = string.Format("{0:0000}", customer.Id) + "-" + DateTime.Now.Year.ToString();
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                    user.Customer_Id = customer.Id;
                    return await _userManager.UpdateAsync(user);
                }

            }
            return new IdentityResult();
        }

        public Customer GetCustomer(string id)
        {

            var user = _userManager.Users.Include(u => u.Customer).SingleOrDefault(u => u.Id == id);
            if (user.Customer != null)
            {
                return user.Customer;
            }
            else
                return null;

        }

        public async Task UpdateUser(AppUser appUser)
        {
            // var user = _userManager.Users.SingleOrDefault(u=>u.Id == appUser.Id);

            _context.Customers.Update(appUser.Customer);
            await _context.SaveChangesAsync();
            appUser.Customer = null;
            try
            {

                var sus = _userManager.UpdateAsync(appUser).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}


