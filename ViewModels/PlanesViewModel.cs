using System;
using System.Collections.Generic;
using AirportFlightManagement.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AirportFlightManagement.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;

namespace AirportFlightManagement.ViewModels;

public partial class PlanesViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;
    private List<Plane> _filteredPlanes = [];
    private int? _editingId;
    private const int PageSize = 8;

    [ObservableProperty]
    private ObservableCollection<Plane> pagedPlanes = [];

    [ObservableProperty]
    private string searchText = "";

    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int totalPages = 1;

    [ObservableProperty]
    private int totalPlanes;

    [ObservableProperty]
    private int planesAtAirport;

    [ObservableProperty]
    private int planesAway;

    [ObservableProperty]
    private bool isPanelOpen;

    [ObservableProperty]
    private string panelTitle = "";

    [ObservableProperty]
    private Plane? selectedPlane;

    [ObservableProperty]
    private string editRegistrationNumber = "";

    [ObservableProperty]
    private string editModel = "";

    [ObservableProperty]
    private bool editAtAirport = true;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isModalOpen = false;

    public PlanesViewModel()
    {
        _db.RefreshPlanes();
        RefreshStats();
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        _filteredPlanes = _db.Planes
            .Where(p => p.RegistrationNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       (p.Model?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false))
            .OrderBy(p => p.RegistrationNumber)
            .ToList();

        CurrentPage = 1;
        TotalPages = (int)Math.Ceiling(_filteredPlanes.Count / (double)PageSize);
        LoadPage();
    }

    private void LoadPage()
    {
        var start = (CurrentPage - 1) * PageSize;
        var pageItems = _filteredPlanes.Skip(start).Take(PageSize).ToList();
        PagedPlanes = new ObservableCollection<Plane>(pageItems);
    }

    private void RefreshStats()
    {
        TotalPlanes = _db.Planes.Count;
        PlanesAtAirport = _db.Planes.Count(p => p.AtAirport);
        PlanesAway = _db.Planes.Count(p => !p.AtAirport);
    }

    private int GetFlightCount(int planeId) =>
        _db.Flights.Count(f => f.PlaneId == planeId);

    [RelayCommand]
    private void NewPlane()
    {
        _editingId = null;
        SelectedPlane = null;
        EditRegistrationNumber = "";
        EditModel = "";
        EditAtAirport = true;
        ErrorMessage = null;
        PanelTitle = "Add New Plane";
        IsPanelOpen = true;
    }

    [RelayCommand]
    private void SavePlane()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(EditRegistrationNumber))
        {
            ErrorMessage = "Registration number is required";
            return;
        }

        if (!ContainsValidInput(EditRegistrationNumber))
        {
            ErrorMessage = "Please enter a valid registration number";
            return;
        }

        if (string.IsNullOrWhiteSpace(EditModel))
        {
            ErrorMessage = "Model is required";
            return;
        }
        
        if (!ContainsValidInput(EditModel))
        {
            ErrorMessage = "Please enter a valid plane model";
        }

        try
        {
            var plane = new Plane
            {
                Id = _editingId ?? 0,
                RegistrationNumber = EditRegistrationNumber,
                Model = string.IsNullOrWhiteSpace(EditModel) ? null : EditModel,
                AtAirport = EditAtAirport
            };

            if (_editingId.HasValue)
            {
                _db.UpdatePlane(plane);
            }
            else
            {
                _db.AddPlane(plane);
            }

            RefreshStats();
            ApplyFilter();
            CancelPanel();
            IsModalOpen = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error saving plane: {ex.Message}";
        }
    }

    [RelayCommand]
    private void DeletePlane()
    {
        if (!_editingId.HasValue)
            return;

        try
        {
            _db.DeletePlane(_editingId.Value);
            RefreshStats();
            ApplyFilter();
            CancelPanel();
            IsModalOpen = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting plane: {ex.Message}";
        }
    }

    [RelayCommand]
    private void CancelPanel()
    {
        IsPanelOpen = false;
        _editingId = null;
        SelectedPlane = null;
        EditRegistrationNumber = "";
        EditModel = "";
        EditAtAirport = true;
        ErrorMessage = null;
    }

    [RelayCommand]
    private void CancelModal()
    {
        _editingId = null;
        SelectedPlane = null;
        EditRegistrationNumber = "";
        EditModel = "";
        EditAtAirport = true;
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

    public void SelectPlane(Plane plane)
    {
        _editingId = plane.Id;
        SelectedPlane = plane;
        EditRegistrationNumber = plane.RegistrationNumber;
        EditModel = plane.Model ?? "";
        EditAtAirport = plane.AtAirport;
        ErrorMessage = null;
        PanelTitle = "Edit Plane";
        IsPanelOpen = true;
    }
    
    private bool ContainsValidInput(string value)
    {
        return Regex.IsMatch(value, @"^[a-zA-Z0-9\s'-]+$");
    }
}
