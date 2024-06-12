namespace CVR_API.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public string CVR { get; set; } = null!;
}