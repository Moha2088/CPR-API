using AutoMapper;
using CVR_API.Models.Dtos;

namespace CVR_API.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
    }
}