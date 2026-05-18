using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using AirportFlightManagement.Models;
using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirportFlightManagement.ViewModels;

public partial class AirportsViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;
    private List<Airport> _filteredAirports = [];
    private int? _editingId;
    private const int PageSize = 8;

    [ObservableProperty]
    private ObservableCollection<Airport> pagedAirports = [];

    [ObservableProperty]
    private string searchText = "";

    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int totalPages = 1;

    [ObservableProperty]
    private int totalAirports;

    [ObservableProperty]
    private int activeAirports;

    [ObservableProperty]
    private bool isPanelOpen;

    [ObservableProperty]
    private string panelTitle = "";

    [ObservableProperty]
    private Airport? selectedAirport;

    [ObservableProperty]
    private string editName = "";

    [ObservableProperty]
    private string editCode = "";

    [ObservableProperty]
    private string editCity = "";

    [ObservableProperty]
    private string editCountry = "";

    [ObservableProperty]
    private bool editIsActive = true;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isModalOpen = false;

    public AirportsViewModel()
    {
        _db.RefreshAirports();
        RefreshStats();
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        _filteredAirports = _db.Airports
            .Where(a =>
                a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.Code.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.City.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.Country.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
            .OrderBy(a => a.Name)
            .ToList();

        CurrentPage = 1;
        TotalPages = Math.Max(1, (int)Math.Ceiling(_filteredAirports.Count / (double)PageSize));
        LoadPage();
    }

    private void LoadPage()
    {
        var start = (CurrentPage - 1) * PageSize;
        PagedAirports = new ObservableCollection<Airport>(_filteredAirports.Skip(start).Take(PageSize));
    }

    private void RefreshStats()
    {
        TotalAirports = _db.Airports.Count;
        ActiveAirports = _db.Airports.Count(a => a.IsActive);
    }

    [RelayCommand]
    private void NewAirport()
    {
        _editingId = null;
        SelectedAirport = null;
        EditName = "";
        EditCode = "";
        EditCity = "";
        EditCountry = "";
        EditIsActive = true;
        ErrorMessage = null;
        PanelTitle = "Add New Airport";
        IsPanelOpen = true;
    }

    [RelayCommand]
    private void SaveAirport()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(EditName))
        {
            ErrorMessage = "Airport name is required";
            return;
        }

        if (!ContainsOnlyLetters(EditName))
        {
            ErrorMessage = "Please enter a valid airport name";
            return;
        }


        if (string.IsNullOrWhiteSpace(EditCode))
        {
            ErrorMessage = "IATA code is required";
            return;
        }
        
        if (!ContainsOnlyLetters(EditCode))
        {
            ErrorMessage = "Please enter a valid code for the airport";
            return;
        }

        if (!Regex.IsMatch(EditCode.Trim(), @"^[A-Z]{3}$"))
        {
            ErrorMessage = "IATA code must be exactly 3 uppercase letters (e.g. OTP)";
            return;
        }

        if (string.IsNullOrWhiteSpace(EditCity))
        {
            ErrorMessage = "City is required";
            return;
        }
        
        if (!ContainsValidCityName(EditName))
        {
            ErrorMessage = "Please enter a valid airport name";
            return;
        }

        if (string.IsNullOrWhiteSpace(EditCountry))
        {
            ErrorMessage = "Country is required";
            return;
        }
        
        if (!ContainsOnlyLetters(EditCountry))
        {
            ErrorMessage = "Please enter a valid country";
            return;
        }

        try
        {
            var airport = new Airport
            {
                Id = _editingId ?? 0,
                Name = EditName.Trim(),
                Code = EditCode.Trim().ToUpperInvariant(),
                City = EditCity.Trim(),
                Country = EditCountry.Trim(),
                IsActive = EditIsActive
            };

            if (_editingId.HasValue)
                _db.UpdateAirport(airport);
            else
                _db.AddAirport(airport);

            RefreshStats();
            ApplyFilter();
            CancelModal();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error saving airport: {ex.Message}";
        }
    }

    [RelayCommand]
    private void DeleteAirport()
    {
        if (!_editingId.HasValue)
            return;

        try
        {
            _db.DeleteAirport(_editingId.Value);
            RefreshStats();
            ApplyFilter();
            CancelModal();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting airport: {ex.Message}";
        }
    }

    [RelayCommand]
    private void CancelModal()
    {
        _editingId = null;
        SelectedAirport = null;
        EditName = "";
        EditCode = "";
        EditCity = "";
        EditCountry = "";
        EditIsActive = true;
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

    public void SelectAirport(Airport airport)
    {
        _editingId = airport.Id;
        SelectedAirport = airport;
        EditName = airport.Name;
        EditCode = airport.Code;
        EditCity = airport.City;
        EditCountry = airport.Country;
        EditIsActive = airport.IsActive;
        ErrorMessage = null;
        PanelTitle = "Edit Airport";
        IsPanelOpen = true;
    }
    
    private bool ContainsOnlyLetters(string value)
    {
        return Regex.IsMatch(value, @"^[a-zA-Z\s]+$");
    }
    
    private bool ContainsValidCityName(string value)
    {
        return Regex.IsMatch(value, @"^[a-zA-Z\s'-]+$");
    }
}
