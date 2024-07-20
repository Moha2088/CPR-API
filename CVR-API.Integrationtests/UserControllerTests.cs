using CVR_API.Data;
using CVR_API.Integrationtests.Fixtures;
using CVR_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CVR_API.Integrationtests;
public class UserControllerTests : IClassFixture<UserControllerTestsFixture>
{
    private string baseAddress = "https://localhost:7152/api/v1/Users";
    private readonly HttpClient _client;


    public UserControllerTests(UserControllerTestsFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task GetUsers_Should_Return200OK()
    {
        var response = await _client.GetAsync(baseAddress);
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUser_Should_Return200OK()
    {
        baseAddress += "/991f1701-5538-4528-b75d-417357ad2098";
        var response = await _client.GetAsync(baseAddress);
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUserResponseBody_ShouldBeOfTypeUser()
    {
        baseAddress += "/991f1701-5538-4528-b75d-417357ad2098";
        var response = await _client.GetAsync(baseAddress);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<User>(body);

        Assert.NotNull(obj);
        Assert.IsType<User>(obj);
    }

    [Fact]
    public async Task GetUser_Should_Return404_WithInvalidID()
    {
        baseAddress += "/991f1701-5538-4528-b75d-417357ad2099";
        var response = await _client.GetAsync(baseAddress);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("991f1701-5538-4528-b75d-417357ad2098")]
    [InlineData("86807d09-acb8-49d3-a21f-047134277bb8")]
    [InlineData("dcd31ec0-7ebc-4952-90d4-c929f8e96246")]
    public async Task ExistingUserIDs_Should_ReturnOK(string userId)
    {
        baseAddress += $"/{userId}";
        var response = await _client.GetAsync(baseAddress);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
}
