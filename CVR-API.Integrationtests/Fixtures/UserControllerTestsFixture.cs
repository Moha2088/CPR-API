using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVR_API.Integrationtests.Fixtures;
public class UserControllerTestsFixture : IDisposable
{
    public HttpClient Client { get; private set; }

    public UserControllerTestsFixture()
    {
        var appFactory = new WebApplicationFactory<Program>();
        Client = appFactory.CreateClient();
    }


    public void Dispose() => Client.Dispose();
}
