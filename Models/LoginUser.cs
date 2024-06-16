

using System.Text.Json.Serialization;

namespace CVR_API.Models;

public class LoginUser
{
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    [JsonIgnore]
    public User User { get; set; } = null!;
}