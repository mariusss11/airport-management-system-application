using System;

namespace AirportFlightManagement.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public string Role { get; set; } = "Operator";
    public DateTime? LastLogin { get; set; }
}
