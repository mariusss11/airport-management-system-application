namespace AirportFlightManagement.Models;

public class Plane
{
    public int Id { get; set; }
    public required string RegistrationNumber { get; set; }
    public string? Model { get; set; }
    public bool AtAirport { get; set; } = true;
}
