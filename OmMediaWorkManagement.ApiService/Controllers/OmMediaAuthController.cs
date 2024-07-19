using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OmMediaWorkManagement.ApiService.DataContext;
using OmMediaWorkManagement.ApiService.Models;
using OmMediaWorkManagement.ApiService.Models.AuthModels;
using OmMediaWorkManagement.ApiService.ViewModels;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OmMediaWorkManagement.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OmMediaAuthController : ControllerBase
    {
        private readonly OmContext _context;
        private readonly UserManager<UserRegistration> _userManager;
        private readonly ILogger<OmMediaAuthController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;


        public OmMediaAuthController(OmContext context, UserManager<UserRegistration> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<OmMediaAuthController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        #region Register
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> RegisterAsync(UserRegistrationViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest($"User creation failed: {string.Join(", ", errors)}");
            }
            // Check if email or username already exists
            var existingUsername = await _userManager.FindByNameAsync(registerViewModel.UserName);


            if (existingUsername != null)
            {
                return BadRequest("UserName address already exists.");
            }
            // Check if email or username already exists
            var existingUser = await _userManager.FindByNameAsync(registerViewModel.EmailAddress);


            if (existingUser != null)
            {
                return BadRequest("Email address already exists.");
            }
            if (registerViewModel.RoleId.IsNullOrEmpty())
            {
                var role = _roleManager.Roles.ToList();
              var UserRoleId = role.FirstOrDefault(d => d.Name == "User")?.Id;
                if (UserRoleId != null)
                {
                    registerViewModel.RoleId = UserRoleId;
                }
            }

            // Hash the password
            //	var passwordHash = _passwordHasher.HashPassword(null, registerViewModel.Password);

            // Create UserRegistration object
            var newUser = new UserRegistration
            {
                FirstName = registerViewModel.FirstName,
                UserName = registerViewModel.UserName,
                Email = registerViewModel.EmailAddress,
                PhoneNumber = registerViewModel.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.UtcNow
            };

            // Attempt to create the user
            var createUserResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                return BadRequest($"User creation failed: {errors}");
            }

            // Assign role if RoleId is provided
            if (!string.IsNullOrEmpty(registerViewModel.RoleId))
            {
                var roleExists = await _roleManager.FindByIdAsync(registerViewModel.RoleId);
                if (roleExists == null)
                {
                    return BadRequest("Role does not exist.");
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, roleExists.Name);
                if (!addToRoleResult.Succeeded)
                {
                    return BadRequest("Failed to assign role to user.");
                }
            }

            return Ok("User created successfully!");
        }
        #endregion

        #region Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserId", user.Id),
                    new Claim(ClaimTypes.Role,userRoles.FirstOrDefault()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        #endregion

        #region AddDyanmicRole && Get Role
        [HttpPost]
        [Route("AddDyanmicRole")]
        public async Task<IActionResult> AddDyanmicRole([FromBody] RolesViewModel rolesViewModel)
        {
            var existingRole = await _roleManager.FindByNameAsync(rolesViewModel.Role);
            if (existingRole != null)
            {
                return BadRequest("Role already exists!");

            }

            // Create a new role
            var newRole = new IdentityRole(rolesViewModel.Role);

            // Add the role to the database
            var result = await _roleManager.CreateAsync(newRole);

            if (!result.Succeeded)
            {
                return BadRequest("Role creation failed! Please check role details and try again.");
            }
            else
            {
                return Ok("Role created successfully!");


            }
        }
        [HttpGet]
        [Route("GetRoles")]

        public async Task<IActionResult> GetRoles()
        {
            var getRoles = await _roleManager.Roles.ToListAsync();
            _logger.LogInformation("Roles");
            return Ok(getRoles);
        }
        #endregion

        #region ValidateMobile
        [HttpPost]
        [Route("ValidateMobile")]
        public async Task<IActionResult> ValidateMobile(ValidateMobileViewModel validateMobileViewModel)
        {
            var employee = await _context.OmEmployee
                .Where(d => d.PhoneNumber == validateMobileViewModel.MobileNumber)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return BadRequest("User Not Exist Kindly Contact Admin");
            }

            if (string.IsNullOrEmpty(validateMobileViewModel.Otp) || validateMobileViewModel.Otp.Length < 5)
            {
                var otp = GenerateOTP();
                employee.OTPGeneratedTime = DateTime.UtcNow;
                employee.OTPExpireTime = DateTime.UtcNow.AddSeconds(30);
                employee.OTP = otp;
                employee.OTPAttempts += 1;

                _context.OmEmployee.Update(employee);
                _context.SaveChanges();
                return Ok($"OTP Sent Successfully on your Number {otp}");
            }
            else
            {
                if (employee.OTP == validateMobileViewModel.Otp && DateTime.UtcNow <= employee.OTPExpireTime)
                {
                    var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.Name),
                new Claim(ClaimTypes.MobilePhone, employee.PhoneNumber),
                new Claim(ClaimTypes.Role, "Mobile"),
                new Claim("UserId", employee.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                    var token = GetToken(authClaims);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                else
                {
                    return BadRequest("Invalid OTP or OTP has expired");
                }
            }
        }

        #endregion

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public static string GenerateOTP(int length = 6)
        {
            const string chars = "123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
