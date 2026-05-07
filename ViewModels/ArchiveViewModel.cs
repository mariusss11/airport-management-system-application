using AirportFlightManagement.Models;
using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AirportFlightManagement.ViewModels;

public partial class ArchiveViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;

    [ObservableProperty]
    private ObservableCollection<ArchivedFlight> archivedFlights = new();

    [ObservableProperty]
    private ArchivedFlight? selectedFlight;

    [ObservableProperty]
    private string fromDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");

    [ObservableProperty]
    private string toDate = DateTime.Now.ToString("yyyy-MM-dd");

    public ArchiveViewModel()
    {
        LoadArchivedFlights();
    }

    private void LoadArchivedFlights()
    {
        ArchivedFlights = new ObservableCollection<ArchivedFlight>(_db.ArchivedFlights);
    }

    [RelayCommand]
    private void FilterByDate()
    {
        if (DateTime.TryParse(FromDate, out var from) && DateTime.TryParse(ToDate, out var to))
        {
            var filtered = _db.ArchivedFlights
                .Where(f => f.CanceledAt >= from && f.CanceledAt <= to)
                .ToList();

            ArchivedFlights = new ObservableCollection<ArchivedFlight>(filtered);
        }
    }

    [RelayCommand]
    private void DeletePermanently(ArchivedFlight flight)
    {
        _db.ArchivedFlights.Remove(flight);
        ArchivedFlights = new ObservableCollection<ArchivedFlight>(_db.ArchivedFlights);
    }

    [RelayCommand]
    private void ResetFilter()
    {
        FromDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        ToDate = DateTime.Now.ToString("yyyy-MM-dd");
        LoadArchivedFlights();
    }
}
