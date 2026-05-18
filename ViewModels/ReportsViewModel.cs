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

public partial class ReportsViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;
    private readonly ExportService _exportService = new();

    [ObservableProperty]
    private ObservableCollection<ReportOption> reportOptions = new();

    [ObservableProperty]
    private ReportOption? selectedReport;

    [ObservableProperty]
    private ObservableCollection<Flight> previewData = new();

    [ObservableProperty]
    private string? destinationFilter = "";

    [ObservableProperty]
    private string? selectedDestination;

    [ObservableProperty]
    private bool showDestinationInput = false;

    [ObservableProperty]
    private ObservableCollection<string> availableDestinations = new();

    public ReportsViewModel()
    {
        _db.RefreshFlights();
        InitializeReports();
        LoadAvailableDestinations();
    }

    private void LoadAvailableDestinations()
    {
        AvailableDestinations.Clear();
        foreach (var destination in _db.Flights
            .Where(f => f.DestinationAirport != null)
            .Select(f => f.DestinationAirport!.Name)
            .Distinct()
            .OrderBy(d => d))
        {
            AvailableDestinations.Add(destination);
        }
    }

    private void InitializeReports()
    {
        ReportOptions.Clear();
        ReportOptions.Add(new ReportOption { Id = 1, Title = "📅 Monday Schedule", Description = "All flights for Monday" });
        ReportOptions.Add(new ReportOption { Id = 2, Title = "💺 Available Seats", Description = "Seats per flight" });
        ReportOptions.Add(new ReportOption { Id = 3, Title = "⏱️  Longest Flight", Description = "Flight with longest duration" });
        ReportOptions.Add(new ReportOption { Id = 4, Title = "💰 Average Price", Description = "By destination" });
        ReportOptions.Add(new ReportOption { Id = 5, Title = "✈️  Planes at Airport", Description = "Current planes" });
    }

    [RelayCommand]
    private void SelectReport(ReportOption report)
    {
        SelectedReport = report;
        ShowDestinationInput = report.Id == 4;
        RefreshPreview();
    }

    [RelayCommand]
    private void RefreshPreview()
    {
        PreviewData.Clear();

        if (SelectedReport == null)
            return;

        List<Flight> data = SelectedReport.Id switch
        {
            1 => _db.GetFlightsByDay(1).ToList(), // Monday
            2 => _db.Flights.ToList(),
            3 => _db.GetLongestFlight() is Flight longest ? new List<Flight> { longest } : new List<Flight>(),
            4 => string.IsNullOrWhiteSpace(SelectedDestination)
                ? new List<Flight>()
                : _db.GetFlightsByDestination(SelectedDestination).ToList(),
            5 => _db.Flights.Where(f => _db.Planes.Any(p => p.Id == f.PlaneId && p.AtAirport)).ToList(),
            _ => new List<Flight>()
        };

        foreach (var flight in data.OrderBy(f => f.DepartureDate).ThenBy(f => f.DepartureTime))
        {
            PreviewData.Add(flight);
        }
    }

    [RelayCommand]
    private async void ExportToExcel()
    {
        if (SelectedReport == null)
            return;

        var filePath = _exportService.ExportToExcel(PreviewData.ToList(), SelectedReport.Title);
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = filePath, UseShellExecute = true });
    }

    [RelayCommand]
    private void ExportToWord()
    {
        if (SelectedReport == null)
            return;

        var filePath = _exportService.ExportToWord(PreviewData.ToList(), SelectedReport.Title);
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = filePath, UseShellExecute = true });
    }
}

public class ReportOption
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
}
