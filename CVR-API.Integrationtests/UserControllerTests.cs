using CVR_API.Data;
using CVR_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CVR_API.Integrationtests;
public class UserControllerTests
{
    private const string baseAddress = "https://localhost:7152/api/v1/Users";
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
}
