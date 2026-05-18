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
    private string searchText = string.Empty;

    [ObservableProperty]
    private DateTime? fromDate = DateTime.Now.AddMonths(-6);

    [ObservableProperty]
    private DateTime? toDate = DateTime.Now;

    public ArchiveViewModel()
    {
        _db.RefreshArchivedFlights();
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var query = _db.ArchivedFlights.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var term = SearchText.Trim().ToLowerInvariant();
            query = query.Where(f =>
                f.FlightCode.ToLowerInvariant().Contains(term) ||
                f.Destination.ToLowerInvariant().Contains(term) ||
                (f.CancellationReason?.ToLowerInvariant().Contains(term) ?? false));
        }

        if (FromDate.HasValue)
            query = query.Where(f => f.CanceledAt.Date >= FromDate.Value.Date);

        if (ToDate.HasValue)
            query = query.Where(f => f.CanceledAt.Date <= ToDate.Value.Date);

        ArchivedFlights = new ObservableCollection<ArchivedFlight>(query);
    }

    [RelayCommand]
    private void Filter() => ApplyFilter();

    [RelayCommand]
    private void ResetFilter()
    {
        SearchText = string.Empty;
        FromDate = DateTime.Now.AddMonths(-6);
        ToDate = DateTime.Now;
        ApplyFilter();
    }
}
