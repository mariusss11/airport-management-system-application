namespace AirportFlightManagement.Models;

public class Airport
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
    public bool IsActive { get; set; } = true;
}
