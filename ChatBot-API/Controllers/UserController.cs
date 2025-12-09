using ChatBot_API.Models.DTO;
using ChatBot_API.Repositoty;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository dbData;
        public AuthController(IUserRepository DbData)
        {
            dbData = DbData;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await dbData.Login(model);


            if (string.IsNullOrEmpty(loginResponse.Token))
            {
                return NotFound("User not found");
            }

            return Ok(new
            {


                token = loginResponse.Token,
                userId = loginResponse.UserId,
                email = loginResponse.Email,
                name = loginResponse.Name


            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest("Invalid registration data");
            }

            if (!dbData.IsUniqueEmail(model.Email))
            {
                return BadRequest("Email already exists");
            }

            var user = await dbData.Registration(model);
            if (user == null)
            {
                return BadRequest("User registration failed");
            }

            //return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
            return Ok(user);
        }

    }
}





