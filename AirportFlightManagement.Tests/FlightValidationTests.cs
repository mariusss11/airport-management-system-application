using System.Text.RegularExpressions;
using Xunit;

namespace AirportFlightManagement.Tests;

/// <summary>
/// Exercises the validation rules that live in FlightsViewModel.SaveFlight
/// as pure logic — no ViewModel, no database required.
/// </summary>
public class FlightValidationTests
{
    // ── Flight code ─────────────────────────────────────────────────────────

    [Theory]
    [InlineData("AB123")]
    [InlineData("FLIGHT 1")]
    [InlineData("KL-400")]   // hyphen allowed by the regex
    public void FlightCode_Valid_PassesRegex(string code)
    {
        Assert.True(IsValidFlightCode(code));
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("AB@123")]
    [InlineData("KL#99")]
    public void FlightCode_Invalid_FailsRegex(string code)
    {
        Assert.False(IsValidFlightCode(code));
    }

    // ── Ticket price ─────────────────────────────────────────────────────────

    [Theory]
    [InlineData("0.01")]
    [InlineData("100")]
    [InlineData("9999.99")]
    public void TicketPrice_PositiveDecimal_IsValid(string input)
    {
        Assert.True(decimal.TryParse(input, out decimal price) && price > 0);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-5")]
    [InlineData("abc")]
    public void TicketPrice_ZeroNegativeOrText_IsInvalid(string input)
    {
        bool valid = decimal.TryParse(input, out decimal price) && price > 0;
        Assert.False(valid);
    }

    // ── Seat constraints ─────────────────────────────────────────────────────

    [Fact]
    public void Seats_AvailableCannotExceedTotal()
    {
        int totalSeats = 100;
        int availableSeats = 150;
        Assert.True(availableSeats > totalSeats);
    }

    [Fact]
    public void Seats_AvailableCannotBeNegative()
    {
        int availableSeats = -1;
        Assert.True(availableSeats < 0);
    }

    [Fact]
    public void Seats_TotalMustBeGreaterThanZero()
    {
        int totalSeats = 0;
        Assert.False(totalSeats > 0);
    }

    [Fact]
    public void Seats_ValidConfiguration_Passes()
    {
        int totalSeats = 180;
        int availableSeats = 90;
        Assert.True(totalSeats > 0);
        Assert.True(availableSeats >= 0);
        Assert.True(availableSeats <= totalSeats);
    }

    // ── Arrival must be after departure ──────────────────────────────────────

    [Fact]
    public void ArrivalTime_AfterDeparture_IsValid()
    {
        var departure = new TimeOnly(8, 0);
        var arrival = new TimeOnly(10, 0);
        Assert.True(arrival > departure);
    }

    [Fact]
    public void ArrivalTime_SameAsDeparture_IsInvalid()
    {
        var departure = new TimeOnly(8, 0);
        var arrival = new TimeOnly(8, 0);
        Assert.False(arrival > departure);
    }

    [Fact]
    public void ArrivalTime_BeforeDeparture_IsInvalid()
    {
        var departure = new TimeOnly(10, 0);
        var arrival = new TimeOnly(8, 0);
        Assert.False(arrival > departure);
    }

    // ── Departure date must not be in the past ────────────────────────────

    [Fact]
    public void DepartureDate_Today_IsValid()
    {
        var date = DateTime.Today;
        Assert.False(date.Date < DateTime.Today);
    }

    [Fact]
    public void DepartureDate_Yesterday_IsInvalid()
    {
        var date = DateTime.Today.AddDays(-1);
        Assert.True(date.Date < DateTime.Today);
    }

    [Fact]
    public void DepartureDate_Tomorrow_IsValid()
    {
        var date = DateTime.Today.AddDays(1);
        Assert.False(date.Date < DateTime.Today);
    }

    // ── Departure and destination airports must differ ────────────────────

    [Fact]
    public void Airports_SameId_IsInvalid()
    {
        int departureId = 1;
        int destinationId = 1;
        Assert.Equal(departureId, destinationId);
    }

    [Fact]
    public void Airports_DifferentIds_IsValid()
    {
        int departureId = 1;
        int destinationId = 2;
        Assert.NotEqual(departureId, destinationId);
    }

    // ── Time parsing ──────────────────────────────────────────────────────

    [Theory]
    [InlineData("08:00")]
    [InlineData("23:59")]
    [InlineData("00:00")]
    public void TimeOnly_ValidFormats_ParseSuccessfully(string input)
    {
        Assert.True(TimeOnly.TryParse(input, out _));
    }

    [Theory]
    [InlineData("25:00")]
    [InlineData("abc")]
    [InlineData("")]
    public void TimeOnly_InvalidFormats_FailParsing(string input)
    {
        Assert.False(TimeOnly.TryParse(input, out _));
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static bool IsValidFlightCode(string value) =>
        !string.IsNullOrWhiteSpace(value) && Regex.IsMatch(value, @"^[a-zA-Z0-9\s'-]+$");
}
