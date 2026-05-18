using System;

namespace AirportFlightManagement.Models;

public class Flight
{
    public int Id { get; set; }
    public required string FlightCode { get; set; }
    public TimeOnly DepartureTime { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public int PlaneId { get; set; }
    public int? DepartureAirportId { get; set; }
    public Airport? DepartureAirport { get; set; }
    public int? DestinationAirportId { get; set; }
    public Airport? DestinationAirport { get; set; }
    public string Destination => DestinationAirport?.Name ?? "Unknown";
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal TicketPrice { get; set; }
    public DateOnly DepartureDate { get; set; }
    public string Status { get; set; } = "On Time";

    public int Duration
    {
        get
        {
            var minutes = (int)(ArrivalTime.ToTimeSpan() - DepartureTime.ToTimeSpan()).TotalMinutes;
            return minutes >= 0 ? minutes : minutes + 1440;
        }
    }

    public string DurationFormatted
    {
        get
        {
            var hours = Duration / 60;
            var mins = Duration % 60;
            var hrLabel = hours == 1 ? "hr" : "hrs";
            if (hours == 0) return $"{mins} min";
            return mins != 0 ? $"{hours} {hrLabel} {mins} min" : $"{hours} {hrLabel}";
        }
    }

    public string DayName => DepartureDate.DayOfWeek.ToString();
}
