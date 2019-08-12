using Market.Models.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Market.Models
{
    public interface IUMRepository
    {
        int CreateUser(LoginRegisterModel user, string role);
        Array GetAllUsers();
        Task<Object> GetJwtToken(LoginRegisterModel loginModel);
        Task<IdentityResult> AssignCustomer(FrontCustomer frontCustomer, string userId);
        Object GetCustomer(string id);
        object UpdateUser(AppUser appUser);
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

        public int CreateUser(LoginRegisterModel user, string role)
        {
            AppUser appuser = new AppUser() { UserName = user.UserName, Email = user.Email };
            try
            {
                if (_userManager.Users.SingleOrDefault(u => u.UserName == user.UserName) != null)
                {
                    return 2;
                }
                IdentityResult userCreation = _userManager.CreateAsync(appuser, user.Password).Result;
                IdentityResult inRoleAdding = _userManager.AddToRoleAsync(appuser, role).Result;
                if (userCreation.Succeeded)
                {
                    if (inRoleAdding.Succeeded)
                        return 1;
                    AppUser appUser = _userManager.Users.SingleOrDefault(u => u.UserName == user.UserName);
                    _userManager.DeleteAsync(appUser);
                    return 3;
                }
                return 3;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 3;
            }
        }

        public Array GetAllUsers()
        {
            return _userManager.Users.Include(u => u.Customer).ToArray();
        }

        public async Task<Object> GetJwtToken(LoginRegisterModel loginModel)
        {
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
                return new { token };

            }
            else
                return new { message = "Username or password is incorrect." };
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

        public Object GetCustomer(string id)
        {

            var user = _userManager.Users.Include(u => u.Customer).SingleOrDefault(u => u.Id == id);
            if (user.Customer != null)
            {
                return new
                {
                    name = user.Customer.Name,
                    address = user.Customer.Address,
                    discount = user.Customer.Discount
                };
            }
            else
                return null;

        }

        public object UpdateUser(AppUser appUser)
        {
            // var user = _userManager.Users.SingleOrDefault(u=>u.Id == appUser.Id);
           
            _context.Customers.Update(appUser.Customer);
            _context.SaveChanges();
            appUser.Customer = null;
            try
            {

                var sus = _userManager.UpdateAsync(appUser).Result;
                return sus;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
