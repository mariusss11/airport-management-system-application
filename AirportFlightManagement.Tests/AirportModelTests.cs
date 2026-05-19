using AirportFlightManagement.Models;
using Xunit;

namespace AirportFlightManagement.Tests;

public class AirportModelTests
{
    [Fact]
    public void Airport_DefaultIsActive_IsTrue()
    {
        var airport = new Airport { Name = "Chisinau International", Code = "KIV" };
        Assert.True(airport.IsActive);
    }

    [Fact]
    public void Airport_DefaultCity_IsEmptyString()
    {
        var airport = new Airport { Name = "Test", Code = "TST" };
        Assert.Equal("", airport.City);
    }

    [Fact]
    public void Airport_DefaultCountry_IsEmptyString()
    {
        var airport = new Airport { Name = "Test", Code = "TST" };
        Assert.Equal("", airport.Country);
    }

    [Fact]
    public void Airport_PropertiesRoundTrip()
    {
        var airport = new Airport
        {
            Id = 42,
            Name = "Henri Coandă",
            Code = "OTP",
            City = "Bucharest",
            Country = "Romania",
            IsActive = false
        };

        Assert.Equal(42, airport.Id);
        Assert.Equal("Henri Coandă", airport.Name);
        Assert.Equal("OTP", airport.Code);
        Assert.Equal("Bucharest", airport.City);
        Assert.Equal("Romania", airport.Country);
        Assert.False(airport.IsActive);
    }
}
