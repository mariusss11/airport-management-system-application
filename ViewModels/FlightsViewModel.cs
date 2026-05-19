using AirportFlightManagement.Models;
using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace AirportFlightManagement.ViewModels;

public partial class FlightsViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;
    private List<Flight> _filteredFlights = [];
    private int? _editingFlightId;
    private const int PageSize = 5;

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
    private int? selectedDestinationAirportId;

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
    private string duration = "60";

    [ObservableProperty]
    private string status = "On Time";

    [ObservableProperty]
    private string departureDate = "";

    [ObservableProperty]
    private int? selectedDepartureAirportId;

    [ObservableProperty]
    private ObservableCollection<Airport> airports;

    [ObservableProperty]
    private ObservableCollection<Plane> planes;

    [ObservableProperty]
    private Plane? selectedPlane;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isModalOpen = false;

    public FlightsViewModel()
    {
        _db.RefreshFlights();
        _db.RefreshPlanes();
        _db.RefreshAirports();
        Flights = new ObservableCollection<Flight>(_db.Flights);
        Airports = new ObservableCollection<Airport>(_db.GetAirports());
        Planes = new ObservableCollection<Plane>(_db.Planes);
        DepartureDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        _filteredFlights = _db.Flights
            .Where(f => f.FlightCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        f.Destination.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        (f.DepartureAirport != null && f.DepartureAirport.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)))
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
        SelectedDestinationAirportId = null;
        DepartureTime = "08:00";
        ArrivalTime = "10:00";
        PlaneId = 0;
        SelectedPlane = null;
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
            SelectedDestinationAirportId = flight.DestinationAirportId;
            DepartureTime = flight.DepartureTime.ToString("HH:mm");
            ArrivalTime = flight.ArrivalTime.ToString("HH:mm");
            PlaneId = flight.PlaneId;
            SelectedPlane = Planes.FirstOrDefault(p => p.Id == flight.PlaneId);
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

        // Flight code
        if (string.IsNullOrWhiteSpace(FlightCode))
        {
            ErrorMessage = "Flight code is required";
            return;
        }
        
        if (!ContainsValidFlightCode(FlightCode))
        {
            ErrorMessage = "Flight code can only contain letters and numbers";
            return;
        }

        // Destination airport
        if (SelectedDestinationAirportId == null)
        {
            ErrorMessage = "Please select a destination airport";
            return;
        }

        // Plane
        if (SelectedPlane == null)
        {
            ErrorMessage = "Please select a plane";
            return;
        }

        // Airport
        if (SelectedDepartureAirportId == null)
        {
            ErrorMessage = "Please select a departure airport";
            return;
        }

        if (SelectedDepartureAirportId == SelectedDestinationAirportId)
        {
            ErrorMessage = "Departure and destination airports must be different";
            return;
        }

        // Departure date
        if (!DateTime.TryParse(DepartureDate, out DateTime departureDate))
        {
            ErrorMessage = "Invalid departure date";
            return;
        }

        if (departureDate.Date < DateTime.Today)
        {
            ErrorMessage = "Departure date cannot be in the past";
            return;
        }

        // Departure time
        if (!TimeOnly.TryParse(DepartureTime, out TimeOnly departureTime))
        {
            ErrorMessage = "Invalid departure time";
            return;
        }

        // Arrival time
        if (!TimeOnly.TryParse(ArrivalTime, out TimeOnly arrivalTime))
        {
            ErrorMessage = "Invalid arrival time";
            return;
        }

        // Arrival must be after departure
        if (arrivalTime <= departureTime)
        {
            ErrorMessage = "Arrival time must be after departure time";
            return;
        }

        // Ticket price
        if (!decimal.TryParse(TicketPrice, out decimal price))
        {
            ErrorMessage = "Invalid ticket price";
            return;
        }

        if (price <= 0)
        {
            ErrorMessage = "Ticket price must be greater than 0";
            return;
        }

        // Seats
        if (TotalSeats <= 0)
        {
            ErrorMessage = "Total seats must be greater than 0";
            return;
        }

        if (AvailableSeats < 0)
        {
            ErrorMessage = "Available seats cannot be negative";
            return;
        }

        if (AvailableSeats > TotalSeats)
        {
            ErrorMessage = "Available seats cannot exceed total seats";
            return;
        }

        try
        {
            var flight = new Flight
            {
                Id = _editingFlightId ?? 0,
                FlightCode = FlightCode.Trim(),
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime,
                PlaneId = SelectedPlane.Id,
                DepartureAirportId = SelectedDepartureAirportId,
                DepartureAirport = Airports.FirstOrDefault(a => a.Id == SelectedDepartureAirportId),
                DestinationAirportId = SelectedDestinationAirportId,
                DestinationAirport = Airports.FirstOrDefault(a => a.Id == SelectedDestinationAirportId),
                TotalSeats = TotalSeats,
                AvailableSeats = AvailableSeats,
                TicketPrice = price,
                Status = Status,
                DepartureDate = DateOnly.FromDateTime(departureDate)
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
            IsModalOpen = false;
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
            IsModalOpen = false;
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
    private void CancelModal()
    {
        _editingFlightId = null;
        SelectedFlight = null;
        ErrorMessage = null;
        IsModalOpen = false;
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
        SelectedDestinationAirportId = null;
        DepartureDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
        DepartureTime = "08:00";
        ArrivalTime = "10:00";
        PlaneId = 0;
        SelectedPlane = null;
        SelectedDepartureAirportId = null;
        TotalSeats = 180;
        AvailableSeats = 180;
        TicketPrice = "100";
        Status = "On Time";
        ErrorMessage = null;
    }
    
    private bool ContainsValidFlightCode(string value)
    {
        return Regex.IsMatch(value, @"^[a-zA-Z0-9\s'-]+$");
    }
}