using AirportFlightManagement.Models;
using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AirportFlightManagement.ViewModels;

public partial class SearchViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;

    [ObservableProperty]
    private string destination = "";

    [ObservableProperty]
    private int selectedDay = 0; // 0 = all

    [ObservableProperty]
    private string fromTime = "00:00";

    [ObservableProperty]
    private string toTime = "23:59";

    [ObservableProperty]
    private int minSeats = 0;

    [ObservableProperty]
    private string selectedStatus = "All";

    [ObservableProperty]
    private ObservableCollection<Flight> searchResults = new();

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool hasSearched = false;

    public SearchViewModel()
    {
    }

    [RelayCommand]
    private void Search()
    {
        ErrorMessage = null;

        if (!TimeOnly.TryParse(FromTime, out var fromT) || !TimeOnly.TryParse(ToTime, out var toT))
        {
            ErrorMessage = "Invalid time format";
            return;
        }

        IEnumerable<Flight> results = _db.Flights;

        if (!string.IsNullOrWhiteSpace(Destination))
        {
            results = results.Where(f => f.Destination.Contains(Destination, StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedDay > 0)
        {
            results = results.Where(f => (int)f.DepartureDate.DayOfWeek == SelectedDay);
        }

        results = results.Where(f => f.DepartureTime >= fromT && f.DepartureTime <= toT);

        if (MinSeats > 0)
        {
            results = results.Where(f => f.AvailableSeats >= MinSeats);
        }

        if (SelectedStatus != "All")
        {
            results = results.Where(f => f.Status == SelectedStatus);
        }

        SearchResults = new ObservableCollection<Flight>(results.OrderBy(f => f.DepartureDate).ThenBy(f => f.DepartureTime));
        HasSearched = true;
    }

    [RelayCommand]
    private void Reset()
    {
        Destination = "";
        SelectedDay = 0;
        FromTime = "00:00";
        ToTime = "23:59";
        MinSeats = 0;
        SelectedStatus = "All";
        SearchResults.Clear();
        HasSearched = false;
        ErrorMessage = null;
    }
}
