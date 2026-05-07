using System;

namespace AirportFlightManagement.Models;

public class Flight
{
    public int Id { get; set; }
    public required string FlightCode { get; set; }
    public required string Destination { get; set; }
    public TimeOnly DepartureTime { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public int PlaneId { get; set; }
    public int? DepartureAirportId { get; set; }
    public Airport? DepartureAirport { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal TicketPrice { get; set; }
    public DateOnly DepartureDate { get; set; }
    public string Status { get; set; } = "On Time";

    public int Duration => (int)(ArrivalTime.ToTimeSpan() - DepartureTime.ToTimeSpan()).TotalMinutes;

    public string DayName => DepartureDate.DayOfWeek.ToString();
}
