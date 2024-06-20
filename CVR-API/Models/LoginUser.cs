namespace CVR_API.Models;

public class LoginUser
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public LoginUser(string username, string password)
    {
        UserName = username;
        Password = password;
    }
}
