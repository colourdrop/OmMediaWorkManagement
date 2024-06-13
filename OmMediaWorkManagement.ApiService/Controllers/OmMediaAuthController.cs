using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OmMediaWorkManagement.ApiService.DataContext;
using OmMediaWorkManagement.ApiService.Models;
using OmMediaWorkManagement.ApiService.ViewModels;

namespace OmMediaWorkManagement.ApiService.Controllers
{
    public class OmMediaAuthController : ControllerBase
    {
        private readonly OmContext _context;
        private readonly UserManager<UserRegistration> _userManager;

        public OmMediaAuthController(OmContext context, UserManager<UserRegistration> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
            
        #region Register
        [HttpPost]
        [Route("registeration")]
        public async Task<IActionResult> RegisterAsync(UserRegistrationViewModel registerViewModel)
        {
            var userRegistration = new UserRegistration
            {
                UserName = registerViewModel.UserName,
                Password = registerViewModel.Password,
                PhoneNumber = registerViewModel.PhoneNumber,
                CreatedOn = DateTime.UtcNow
            };
            var userExists = await _userManager.FindByNameAsync(userRegistration.UserName);
            if (userExists != null)
            {
                return BadRequest("User Already Exist");
            }
            else
            {

                var createUserResult = await _userManager.CreateAsync(userRegistration, userRegistration.PasswordHash);
                if (!createUserResult.Succeeded)
                {
                    return BadRequest("User creation failed! Please check user details and try again");
               }
                else
                {
                    return Ok("User Created Successfully");
                }
            }
            throw new NotImplementedException();

        }
        #endregion
        
       

    }
}
