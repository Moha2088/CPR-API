using CVR_API.Models;
using CVR_API.Models.Dtos;
using CVR_API.Repository;

namespace CVR_API.Services;

public class UserService : IUserService
{
    private readonly IRepository _repository;

    public UserService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task DeleteUser(Guid id)
    {
        await _repository.DeleteUser(id);
    }

    public async Task<UserDTO> GetUser(Guid id)
    {
        return await _repository.GetUser(id);
    }

    public async Task<IEnumerable<UserDTO>> GetUsers()
    {
        return await _repository.GetUsers();
    }

    public async Task<User> PostUser(User user)
    {
        return await _repository.PostUser(user);
    }

    public async Task PutUser(Guid id, User user)
    {
        await _repository.PutUser(id, user);
    }

    public bool UserExists(Guid id)
    {
        return _repository.UserExists(id);
    }
}
