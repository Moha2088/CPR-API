using CVR_API.Models;
using CVR_API.Models.Dtos;

namespace CVR_API.Services;
public interface IUserService
{
    Task DeleteUser(Guid id);
    Task<UserDTO> GetUser(Guid id);
    Task<IEnumerable<UserDTO>> GetUsers();
    Task<User> PostUser(User user);
    Task PutUser(Guid id, User user);
    bool UserExists(Guid id);
}