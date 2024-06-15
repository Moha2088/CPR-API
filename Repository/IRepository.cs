using CVR_API.Models;
using CVR_API.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CVR_API.Repository;

public interface IRepository
{
    Task DeleteUser(Guid id);
    Task<IEnumerable<UserDTO>> GetUsers();
    Task<UserDTO> GetUser(Guid id);
    Task<UserDTO> PostUser(User user);
    Task PutUser(Guid id, User user);
    bool UserExists(Guid id);
}