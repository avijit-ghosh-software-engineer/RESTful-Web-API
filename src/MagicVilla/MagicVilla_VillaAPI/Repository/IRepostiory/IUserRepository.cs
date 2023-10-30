using MagicVilla_VillaAPI.Models.Dtos;

namespace MagicVilla_VillaAPI.Repository.IRepostiory
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
