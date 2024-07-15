using CVR_API.Data;
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
public class UserControllerTests
{
    private string baseAddress = "https://localhost:7152/api/v1/Users";
    private readonly HttpClient _client;


    public UserControllerTests()
    {
        var appFactory = new WebApplicationFactory<Program>();
        _client = appFactory.CreateClient();
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
        baseAddress += "/ac40ab48-c21e-4201-8e12-ba577d72241f";
        var response = await _client.GetAsync(baseAddress);
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUserResponseBody_ShouldBeOfTypeUser()
    {
        baseAddress += "/ac40ab48-c21e-4201-8e12-ba577d72241f";
        var response = await _client.GetAsync(baseAddress);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<User>(body);

        Assert.NotNull(obj);
        Assert.IsType<User>(obj);
    }

    //[Fact]
    //public async Task GetUser_Should_Return404_WithInvalidID()
    //{
    //    baseAddress += "/ac40ab48-c21e-4201-8e12-ba577d72242f";
    //    var response = await _client.GetAsync(baseAddress);
    //    response.EnsureSuccessStatusCode();
    //    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    //}
}
