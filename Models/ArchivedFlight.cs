using System;

namespace AirportFlightManagement.Models;

public class ArchivedFlight
{
    public int Id { get; set; }
    public required string FlightCode { get; set; }
    public required string Destination { get; set; }
    public TimeOnly DepartureTime { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public int PlaneId { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal TicketPrice { get; set; }
    public int DayOfWeek { get; set; }
    public DateTime CanceledAt { get; set; } = DateTime.UtcNow;
    public string? CancellationReason { get; set; }
    public int? DepartureAirportId { get; set; }
    public int? DestinationAirportId { get; set; }
}
