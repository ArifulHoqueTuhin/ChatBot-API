using ChatBot_API.Models.DTO;

namespace ChatBot_API.Repositoty
{
    public interface IUserRepository
    {
        bool IsUniqueEmail(string email);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO);

        Task<RegistrationResponseDto> Registration(RegistrationRequestDto registrationRequestDTO);
    }
}
