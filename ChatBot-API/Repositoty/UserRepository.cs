using ChatBot_API.Models.DTO;
using ChatBot_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace ChatBot_API.Repositoty
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _dbData;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string SecretKey;

        public UserRepository(ApplicationDbContext dbData, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbData = dbData;
            SecretKey = configuration.GetValue<string>("ApiSettings:Secrets");
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public bool IsUniqueEmail(string email)
        {
            var User = _dbData.ApplicationUsers.FirstOrDefault(u => u.Email == email);

            if (User == null)
            {
                return true;
            }

            return false;
        }


        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO)
        {
            var user = await _dbData.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);


            if (user == null || isValid == false)
            {
                return new LoginResponseDto()
                {
                    Token = "",

                };

            }


            var roles = await _userManager.GetRolesAsync(user);

            var TokenHandler = new JwtSecurityTokenHandler();

            var Key = Encoding.ASCII.GetBytes(SecretKey);

            var TokenDescriptor = new SecurityTokenDescriptor

            {
                Subject = new ClaimsIdentity(new Claim[]
                {

                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),

                Expires = DateTime.UtcNow.AddDays(7),

                SigningCredentials = new(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)

            };


            var Token = TokenHandler.CreateToken(TokenDescriptor);

            LoginResponseDto loginResponseDTO = new LoginResponseDto()

            {
                Token = TokenHandler.WriteToken(Token),
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email

            };

            return loginResponseDTO;
        }

        public async Task<RegistrationResponseDto> Registration(RegistrationRequestDto registrationRequestDTO)
        {
            var newUser = new ApplicationUser
            {
                Name = registrationRequestDTO.Name,
                UserName = registrationRequestDTO.Email,
                NormalizedUserName = registrationRequestDTO.Email.ToUpper(),
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper()
            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, registrationRequestDTO.Password);

                if (result.Succeeded)
                {

                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                    }
                    if (!_roleManager.RoleExistsAsync("customer").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }


                    string roleToAssign = string.IsNullOrWhiteSpace(registrationRequestDTO.Role) ? "customer" : registrationRequestDTO.Role.ToLower();

                    if (roleToAssign != "admin" && roleToAssign != "customer")
                    {
                        roleToAssign = "customer";
                    }

                    await _userManager.AddToRoleAsync(newUser, roleToAssign);

                    var UserToReturn = await _dbData.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == registrationRequestDTO.Email);

                    return new RegistrationResponseDto()
                    {
                        IsSuccess = true,
                        Message = "User registration successful"

                    };

                };

            }
            catch (Exception ex)
            {
                throw new Exception("Error during registration: " + ex.Message);
            }

            return null;
        }
    }
}



