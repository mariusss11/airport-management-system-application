using AirportFlightManagement.Models;
using Xunit;

namespace AirportFlightManagement.Tests;

public class FlightModelTests
{
    // ── Duration ────────────────────────────────────────────────────────────

    [Fact]
    public void Duration_SameDay_ReturnsCorrectMinutes()
    {
        var flight = MakeFlight(new TimeOnly(8, 0), new TimeOnly(10, 30));
        Assert.Equal(150, flight.Duration);
    }

    [Fact]
    public void Duration_ExactlyOneHour_Returns60()
    {
        var flight = MakeFlight(new TimeOnly(14, 0), new TimeOnly(15, 0));
        Assert.Equal(60, flight.Duration);
    }

    [Fact]
    public void Duration_OvernightFlight_WrapsCorrectly()
    {
        // 23:00 → 01:00 = 120 minutes spanning midnight
        var flight = MakeFlight(new TimeOnly(23, 0), new TimeOnly(1, 0));
        Assert.Equal(120, flight.Duration);
    }

    [Fact]
    public void Duration_ZeroMinutes_ReturnsZero()
    {
        var flight = MakeFlight(new TimeOnly(9, 0), new TimeOnly(9, 0));
        Assert.Equal(0, flight.Duration);
    }

    // ── DurationFormatted ───────────────────────────────────────────────────

    [Fact]
    public void DurationFormatted_UnderOneHour_ShowsOnlyMinutes()
    {
        var flight = MakeFlight(new TimeOnly(9, 0), new TimeOnly(9, 45));
        Assert.Equal("45 min", flight.DurationFormatted);
    }

    [Fact]
    public void DurationFormatted_ExactHours_OmitsMinutes()
    {
        var flight = MakeFlight(new TimeOnly(8, 0), new TimeOnly(10, 0));
        Assert.Equal("2 hrs", flight.DurationFormatted);
    }

    [Fact]
    public void DurationFormatted_OneHourExact_UsesSingular()
    {
        var flight = MakeFlight(new TimeOnly(8, 0), new TimeOnly(9, 0));
        Assert.Equal("1 hr", flight.DurationFormatted);
    }

    [Fact]
    public void DurationFormatted_HoursAndMinutes_ShowsBoth()
    {
        var flight = MakeFlight(new TimeOnly(8, 0), new TimeOnly(10, 35));
        Assert.Equal("2 hrs 35 min", flight.DurationFormatted);
    }

    [Fact]
    public void DurationFormatted_OneHourAndMinutes_UsesSingularHr()
    {
        var flight = MakeFlight(new TimeOnly(8, 0), new TimeOnly(9, 20));
        Assert.Equal("1 hr 20 min", flight.DurationFormatted);
    }

    // ── DayName ─────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(2024, 1, 1, "Monday")]
    [InlineData(2024, 1, 6, "Saturday")]
    [InlineData(2024, 1, 7, "Sunday")]
    public void DayName_ReturnsCorrectWeekdayName(int year, int month, int day, string expected)
    {
        var flight = new Flight
        {
            FlightCode = "TEST",
            DepartureTime = new TimeOnly(8, 0),
            ArrivalTime = new TimeOnly(10, 0),
            DepartureDate = new DateOnly(year, month, day)
        };
        Assert.Equal(expected, flight.DayName);
    }

    // ── Destination derived property ─────────────────────────────────────

    [Fact]
    public void Destination_WhenAirportSet_ReturnsAirportName()
    {
        var flight = new Flight
        {
            FlightCode = "AB123",
            DepartureTime = new TimeOnly(8, 0),
            ArrivalTime = new TimeOnly(9, 0),
            DepartureDate = DateOnly.FromDateTime(DateTime.Today),
            DestinationAirport = new Airport { Id = 1, Name = "Charles de Gaulle", Code = "CDG" }
        };
        Assert.Equal("Charles de Gaulle", flight.Destination);
    }

    [Fact]
    public void Destination_WhenAirportNull_ReturnsUnknown()
    {
        var flight = new Flight
        {
            FlightCode = "AB123",
            DepartureTime = new TimeOnly(8, 0),
            ArrivalTime = new TimeOnly(9, 0),
            DepartureDate = DateOnly.FromDateTime(DateTime.Today),
            DestinationAirport = null
        };
        Assert.Equal("Unknown", flight.Destination);
    }

    // ── Status default ───────────────────────────────────────────────────

    [Fact]
    public void Status_Default_IsOnTime()
    {
        var flight = new Flight { FlightCode = "X1", DepartureTime = default, ArrivalTime = default };
        Assert.Equal("On Time", flight.Status);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static Flight MakeFlight(TimeOnly departure, TimeOnly arrival) =>
        new()
        {
            FlightCode = "TEST",
            DepartureTime = departure,
            ArrivalTime = arrival,
            DepartureDate = DateOnly.FromDateTime(DateTime.Today)
        };
}
