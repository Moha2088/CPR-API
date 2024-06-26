﻿using System.Text.Json.Serialization;

namespace CVR_API.Models;

public class User
{
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string CPR { get; set; } = null!;
}