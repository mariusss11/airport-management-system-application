using AirportFlightManagement.Models;
using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AirportFlightManagement.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;

    [ObservableProperty]
    private int totalFlights;

    [ObservableProperty]
    private int availableSeats;

    [ObservableProperty]
    private int planesAtAirport;

    [ObservableProperty]
    private int canceledFlights;

    [ObservableProperty]
    private Flight? longestFlight;

    [ObservableProperty]
    private ObservableCollection<Flight> todayFlights = new();

    [ObservableProperty]
    private decimal averagePrice;

    public DashboardViewModel()
    {
        _db.RefreshAll();
        LoadData();
    }

    private void LoadData()
    {
        TotalFlights = _db.GetTotalFlights();
        AvailableSeats = _db.GetTotalAvailableSeats();
        PlanesAtAirport = _db.GetPlanesAtAirportCount();
        CanceledFlights = _db.GetCanceledFlightsCount();
        LongestFlight = _db.GetLongestFlight();
        AveragePrice = _db.Flights.Count > 0 ? (decimal)_db.Flights.Average(f => f.TicketPrice) : 0;

        TodayFlights = new ObservableCollection<Flight>(_db.Flights.Take(10));
    }
}
