using ORM_MiniProject.DTOs.UserDTOs;
using ORM_MiniProject.Models;

namespace ORM_MiniProject.Services.Interfaces
{
    public interface IUsersService
    {
        Task<List<UserGetDto>> GetAllUsersAsync();
        Task<UserGetDto> GetUserAsync(int id);
        Task UpdateUserAsync(UserPutDto user);
        Task CreateUserAsync(UserPostDto user);
        Task DeleteUserAsync(int id);
        Task<UserGetDto> LoginAsync(UserLoginDto login);
        Task<List<Orders>>? GetUserOrdersAsync(int id );
        Task<byte[]> ExportUserOrdersToExcelAsync(int userId);
    }
}
