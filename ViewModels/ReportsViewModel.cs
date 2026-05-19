using AirportFlightManagement.Models;
using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Viewer.Avalonia.Viewer;

namespace AirportFlightManagement.ViewModels;

public partial class ReportsViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;
    private readonly ExportService _exportService = new();

    [ObservableProperty] private ObservableCollection<ReportOption> reportOptions = new();

    [ObservableProperty] private ReportOption? selectedReport;

    [ObservableProperty] private ObservableCollection<Flight> previewData = new();

    [ObservableProperty] private string? destinationFilter = "";

    [ObservableProperty] private string? selectedDestination;

    [ObservableProperty] private bool showDestinationInput = false;

    [ObservableProperty] private ObservableCollection<string> availableDestinations = new();

    [ObservableProperty] private string destinationError = "";

    public bool CanExport => SelectedReport != null && (!ShowDestinationInput || !string.IsNullOrWhiteSpace(SelectedDestination));

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
        ReportOptions.Add(new ReportOption { Id = 1, Title = "All Flights", Description = "By destination" });
    }

    partial void OnSelectedReportChanged(ReportOption? value)
    {
        ShowDestinationInput = value?.Id == 1;
        SelectedDestination = null;
        DestinationError = "";
        OnPropertyChanged(nameof(CanExport));
        RefreshPreview();
    }

    partial void OnSelectedDestinationChanged(string? value)
    {
        DestinationError = "";
        OnPropertyChanged(nameof(CanExport));
        RefreshPreview();
    }

    [RelayCommand]
    private void RefreshPreview()
    {
        PreviewData.Clear();

        if (SelectedReport == null)
            return;

        if (ShowDestinationInput && string.IsNullOrWhiteSpace(SelectedDestination))
            return;

        List<Flight> data = SelectedReport.Id switch
        {
            1 => _db.GetFlightsByDestination(SelectedDestination!).ToList(),
            _ => new List<Flight>()
        };

        foreach (var item in data)
        {
            PreviewData.Add(item);
        }
    }

    [RelayCommand]
    private async void ExportToExcel()
    {
        if (SelectedReport == null)
            return;

        if (ShowDestinationInput && string.IsNullOrWhiteSpace(SelectedDestination))
        {
            DestinationError = "Please select a destination before exporting.";
            return;
        }

        RefreshPreview();
        
        var filePath = _exportService.ExportToExcel(PreviewData.ToList(), SelectedReport.Title);
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            { FileName = filePath, UseShellExecute = true });
    }

    [RelayCommand]
    private void ExportToWord()
    {
        if (SelectedReport == null)
            return;

        if (ShowDestinationInput && string.IsNullOrWhiteSpace(SelectedDestination))
        {
            DestinationError = "Please select a destination before exporting.";
            return;
        }

        var filePath = _exportService.ExportToWord(PreviewData.ToList(), SelectedReport.Title);
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            { FileName = filePath, UseShellExecute = true });
    }
    
    public class ReportOption
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
