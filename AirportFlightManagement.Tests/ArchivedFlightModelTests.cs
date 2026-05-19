using AirportFlightManagement.Models;
using Xunit;

namespace AirportFlightManagement.Tests;

public class ArchivedFlightModelTests
{
    [Fact]
    public void ArchivedFlight_CanceledAt_DefaultsToUtcNow()
    {
        var before = DateTime.UtcNow;
        var archived = new ArchivedFlight
        {
            FlightCode = "AB123",
            Destination = "Paris"
        };
        var after = DateTime.UtcNow;

        Assert.InRange(archived.CanceledAt, before, after);
    }

    [Fact]
    public void ArchivedFlight_CancellationReason_DefaultsToNull()
    {
        var archived = new ArchivedFlight { FlightCode = "AB123", Destination = "Paris" };
        Assert.Null(archived.CancellationReason);
    }

    [Fact]
    public void ArchivedFlight_PropertiesRoundTrip()
    {
        var archived = new ArchivedFlight
        {
            Id = 10,
            FlightCode = "KL502",
            Destination = "Amsterdam",
            DepartureTime = new TimeOnly(6, 30),
            ArrivalTime = new TimeOnly(9, 0),
            PlaneId = 3,
            TotalSeats = 200,
            AvailableSeats = 50,
            TicketPrice = 299.99m,
            DayOfWeek = 2,
            CancellationReason = "Weather",
            DepartureAirportId = 1,
            DestinationAirportId = 4
        };

        Assert.Equal("KL502", archived.FlightCode);
        Assert.Equal("Amsterdam", archived.Destination);
        Assert.Equal(new TimeOnly(6, 30), archived.DepartureTime);
        Assert.Equal(299.99m, archived.TicketPrice);
        Assert.Equal("Weather", archived.CancellationReason);
    }
}
