namespace CVR_API.Models.Dtos;

public class UserDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;
}