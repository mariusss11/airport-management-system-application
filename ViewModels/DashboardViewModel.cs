using AirportFlightManagement.Models;
using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;

namespace AirportFlightManagement.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;
    private const int PageSize = 5;

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
    
    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int totalPages = 1;
    
    [ObservableProperty]
    private ObservableCollection<Flight> pagedFlights = [];
    
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

        var today = DateTime.Today;
        TodayFlights = new ObservableCollection<Flight>(
            _db.Flights.Where(f => f.DepartureDate.DayOfYear == today.DayOfYear && f.DepartureDate.Year == today.Year));
        
        TotalPages = (int)Math.Ceiling((double)TodayFlights.Count / PageSize);
        
        CurrentPage = 1;
        
        LoadPage();
    }
    
    private void LoadPage()
    {
        var start = (CurrentPage - 1) * PageSize;
        var pageItems = TodayFlights.Skip(start).Take(PageSize).ToList();
        PagedFlights = new ObservableCollection<Flight>(pageItems);
    }
    
    [RelayCommand]
    private void NextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            LoadPage();
        }
    }

    [RelayCommand]
    private void PreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            LoadPage();
        }
    }
}
