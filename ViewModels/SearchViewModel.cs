using AirportFlightManagement.Models;
using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace AirportFlightManagement.ViewModels;

public partial class SearchViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;
    private List<Flight> _filteredResults = [];
    private const int PageSize = 8;

    [ObservableProperty]
    private string departureLocation = "";

    [ObservableProperty]
    private string destination = "";

    [ObservableProperty]
    private string departureDate = "";

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
    private ObservableCollection<Flight> pagedResults = [];

    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int totalPages = 1;

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

        if (!string.IsNullOrWhiteSpace(DepartureLocation))
        {
            results = results.Where(f =>
            {
                Debug.Assert(f.DepartureAirport != null, "f.DepartureAirport != null");
                return f.DepartureAirport.Name.Contains(DepartureLocation, StringComparison.OrdinalIgnoreCase);
            });
        }

        if (!string.IsNullOrWhiteSpace(Destination))
        {
            results = results.Where(f => f.Destination.Contains(Destination, StringComparison.OrdinalIgnoreCase));
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

        _filteredResults = results.OrderBy(f => f.DepartureDate).ThenBy(f => f.DepartureTime).ToList();
        SearchResults = new ObservableCollection<Flight>(_filteredResults);

        CurrentPage = 1;
        TotalPages = (int)Math.Ceiling(_filteredResults.Count / (double)PageSize);
        LoadPage();
        HasSearched = true;
    }

    private void LoadPage()
    {
        var start = (CurrentPage - 1) * PageSize;
        var pageItems = _filteredResults.Skip(start).Take(PageSize).ToList();
        PagedResults = new ObservableCollection<Flight>(pageItems);
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

    [RelayCommand]
    private void Reset()
    {
        Destination = "";
        DepartureDate = "";
        FromTime = "00:00";
        ToTime = "23:59";
        MinSeats = 0;
        SelectedStatus = "All";
        SearchResults.Clear();
        PagedResults.Clear();
        CurrentPage = 1;
        TotalPages = 1;
        HasSearched = false;
        ErrorMessage = null;
    }
}
