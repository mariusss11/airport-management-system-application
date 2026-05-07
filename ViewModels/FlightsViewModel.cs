using AirportFlightManagement.Models;
using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AirportFlightManagement.ViewModels;

public partial class FlightsViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;
    private List<Flight> _filteredFlights = [];
    private int? _editingFlightId;
    private const int PageSize = 8;

    [ObservableProperty]
    private ObservableCollection<Flight> flights;

    [ObservableProperty]
    private ObservableCollection<Flight> pagedFlights = [];

    [ObservableProperty]
    private string searchText = "";

    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int totalPages = 1;

    [ObservableProperty]
    private Flight? selectedFlight;

    [ObservableProperty]
    private bool isPanelOpen = false;

    [ObservableProperty]
    private string panelTitle = "Add New Flight";

    [ObservableProperty]
    private string flightCode = "";

    [ObservableProperty]
    private string destination = "";

    [ObservableProperty]
    private string departureTime = "08:00";

    [ObservableProperty]
    private string arrivalTime = "10:00";

    [ObservableProperty]
    private int planeId = 0;

    [ObservableProperty]
    private int totalSeats = 180;

    [ObservableProperty]
    private int availableSeats = 180;

    [ObservableProperty]
    private string ticketPrice = "100";

    [ObservableProperty]
    private string status = "On Time";

    [ObservableProperty]
    private string departureDate = "";

    [ObservableProperty]
    private int? selectedDepartureAirportId;

    [ObservableProperty]
    private ObservableCollection<Airport> airports;

    [ObservableProperty]
    private string? errorMessage;

    public FlightsViewModel()
    {
        Flights = new ObservableCollection<Flight>(_db.Flights);
        Airports = new ObservableCollection<Airport>(_db.GetAirports());
        DepartureDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        _filteredFlights = _db.Flights
            .Where(f => f.FlightCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       f.Destination.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
            .OrderBy(f => f.DepartureTime)
            .ToList();

        CurrentPage = 1;
        TotalPages = (int)Math.Ceiling(_filteredFlights.Count / (double)PageSize);
        LoadPage();
    }

    private void LoadPage()
    {
        var start = (CurrentPage - 1) * PageSize;
        var pageItems = _filteredFlights.Skip(start).Take(PageSize).ToList();
        PagedFlights = new ObservableCollection<Flight>(pageItems);
    }

    [RelayCommand]
    private void NewFlight()
    {
        _editingFlightId = null;
        FlightCode = "";
        Destination = "";
        DepartureTime = "08:00";
        ArrivalTime = "10:00";
        PlaneId = 0;
        SelectedDepartureAirportId = null;
        TotalSeats = 180;
        AvailableSeats = 180;
        TicketPrice = "100";
        Status = "On Time";
        DepartureDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
        ErrorMessage = null;
        PanelTitle = "Add New Flight";
        IsPanelOpen = true;
    }

    [RelayCommand]
    private void SelectFlight(Flight flight)
    {
        if (flight != null)
        {
            _editingFlightId = flight.Id;
            SelectedFlight = flight;
            FlightCode = flight.FlightCode;
            Destination = flight.Destination;
            DepartureTime = flight.DepartureTime.ToString("HH:mm");
            ArrivalTime = flight.ArrivalTime.ToString("HH:mm");
            PlaneId = flight.PlaneId;
            SelectedDepartureAirportId = flight.DepartureAirportId;
            TotalSeats = flight.TotalSeats;
            AvailableSeats = flight.AvailableSeats;
            TicketPrice = flight.TicketPrice.ToString();
            Status = flight.Status ?? "On Time";
            DepartureDate = flight.DepartureDate.ToString("yyyy-MM-dd");
            ErrorMessage = null;
            PanelTitle = "Edit Flight";
            IsPanelOpen = true;
        }
    }

    [RelayCommand]
    private void SaveFlight()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(FlightCode))
        {
            ErrorMessage = "Flight code is required";
            return;
        }

        try
        {
            if (!int.TryParse(TicketPrice, out var price))
            {
                ErrorMessage = "Invalid ticket price";
                return;
            }

            var flight = new Flight
            {
                Id = _editingFlightId ?? 0,
                FlightCode = FlightCode,
                Destination = Destination,
                DepartureTime = TimeOnly.Parse(DepartureTime),
                ArrivalTime = TimeOnly.Parse(ArrivalTime),
                PlaneId = PlaneId,
                DepartureAirportId = SelectedDepartureAirportId,
                TotalSeats = TotalSeats,
                AvailableSeats = AvailableSeats,
                TicketPrice = price,
                Status = Status,
                DepartureDate = DateOnly.Parse(DepartureDate)
            };

            if (_editingFlightId.HasValue)
            {
                _db.UpdateFlight(flight);
            }
            else
            {
                _db.AddFlight(flight);
            }

            Flights = new ObservableCollection<Flight>(_db.Flights);
            ApplyFilter();
            CancelPanel();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error saving flight: {ex.Message}";
        }
    }

    [RelayCommand]
    private void DeleteFlight()
    {
        if (!_editingFlightId.HasValue)
            return;

        try
        {
            _db.DeleteFlight(_editingFlightId.Value);
            Flights = new ObservableCollection<Flight>(_db.Flights);
            ApplyFilter();
            CancelPanel();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting flight: {ex.Message}";
        }
    }

    [RelayCommand]
    private void CancelPanel()
    {
        IsPanelOpen = false;
        _editingFlightId = null;
        SelectedFlight = null;
        ErrorMessage = null;
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
    private void GoToPage(int page)
    {
        if (page >= 1 && page <= TotalPages)
        {
            CurrentPage = page;
            LoadPage();
        }
    }


    private void ResetForm()
    {
        FlightCode = "";
        Destination = "";
        DepartureDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
        DepartureTime = "08:00";
        ArrivalTime = "10:00";
        PlaneId = 0;
        SelectedDepartureAirportId = null;
        TotalSeats = 180;
        AvailableSeats = 180;
        TicketPrice = "100";
        Status = "On Time";
        ErrorMessage = null;
    }
}
