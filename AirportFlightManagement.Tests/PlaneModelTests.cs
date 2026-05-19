using AirportFlightManagement.Models;
using Xunit;

namespace AirportFlightManagement.Tests;

public class PlaneModelTests
{
    [Fact]
    public void Plane_DefaultAtAirport_IsTrue()
    {
        var plane = new Plane { RegistrationNumber = "ER-AXB" };
        Assert.True(plane.AtAirport);
    }

    [Fact]
    public void Plane_DefaultModel_IsNull()
    {
        var plane = new Plane { RegistrationNumber = "ER-AXB" };
        Assert.Null(plane.Model);
    }

    [Fact]
    public void Plane_PropertiesRoundTrip()
    {
        var plane = new Plane
        {
            Id = 7,
            RegistrationNumber = "ER-AXB",
            Model = "Boeing 737",
            AtAirport = false
        };

        Assert.Equal(7, plane.Id);
        Assert.Equal("ER-AXB", plane.RegistrationNumber);
        Assert.Equal("Boeing 737", plane.Model);
        Assert.False(plane.AtAirport);
    }
}
