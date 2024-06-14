﻿
using CVR_API.Data;
using CVR_API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CVR_API.Models.Dtos;

namespace CVR_API.Repository;

public class UserRepository : IRepository
{
    private readonly CVR_APIContext _context;
    private readonly IMapper _mapper;

    public UserRepository(CVR_APIContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
        
    public async Task DeleteUser(Guid id)
    {
        var userToDelete = await _context.User.FindAsync(id);

        if (userToDelete == null)
        {
            throw new Exception($"User with id: {id} does not exist! No user has been deleted!");
        }

        _context.User.Remove(userToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserDTO>> GetUsers()
    {
        var users = await _context.User.ToListAsync();
        return users.Select(user => _mapper.Map<UserDTO>(user));
    }

    public async Task<UserDTO> GetUser(Guid id)
    {
        var user = await _context.User.FindAsync(id);

        if (user == null)
        {
            throw new Exception($"User with id: {id} does not exist!");
        }

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<User> PostUser(User user)
    {
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task PutUser(Guid id, User user)
    {
        if (id != user.Id)
        {
            throw new Exception();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                throw new Exception($"User with id: {id} does not exist!");
            }
            else
            {
                throw;
            }
        }
    }

    public bool UserExists(Guid id)
    {
        return _context.User.Any(x => x.Id == id);
    }
}
