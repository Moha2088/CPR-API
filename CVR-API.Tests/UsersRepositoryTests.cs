using AutoMapper;
using CVR_API.Controllers.v1;
using CVR_API.Data;
using CVR_API.Models;
using CVR_API.Models.Dtos;
using CVR_API.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NuGet.ContentModel;
using System.Runtime.ConstrainedExecution;

namespace CVR_API.Tests;

public class UsersRepositoryTests
{

    public CVR_APIContext GetContext()
    {
        var options = new DbContextOptionsBuilder<CVR_APIContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new CVR_APIContext(options);
    }



    [Fact]
    public async Task DeleteUser_ShouldReturnNull_WhenUserDoesnotExist()
    {
        var context = GetContext();
        var mapper = new Mock<IMapper>().Object;
        var logger = new Mock<ILogger<UserRepository>>().Object;
        var repository = new Mock<UserRepository>(context, mapper, logger);

        var result = repository.Object.DeleteUser(Guid.NewGuid());

        Assert.IsNotType<UserDTO>(result);
        await Assert.ThrowsAsync<Exception>(() => repository.Object.DeleteUser(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetUsers_ShouldBeOfType_IEnumerableDTO()
    {
        var context = GetContext();
        var mapper = new Mock<IMapper>().Object;
        var logger = new Mock<ILogger<UserRepository>>().Object;
        var repository = new Mock<UserRepository>(context, mapper, logger);

        var result = repository.Object.GetUsers();

        Assert.NotNull(result);
        await Assert.IsType<Task<IEnumerable<UserDTO>?>>(result);
    }
}