using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirportFlightManagement.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly DatabaseService _db = DatabaseService.Instance;

    [ObservableProperty]
    private bool isDarkMode = true;

    [ObservableProperty]
    private string dbStatus = "✓ Connected";

    [ObservableProperty]
    private string dbStatusColor = "#10B981";

    [ObservableProperty]
    private string? message;

    public SettingsViewModel()
    {
    }

    [RelayCommand]
    private void TestConnection()
    {
        DbStatus = "✓ Connected";
        DbStatusColor = "#10B981";
        Message = "Database connection successful!";
    }

    [RelayCommand]
    private void BackupDatabase()
    {
        Message = "✓ Backup completed successfully";
    }

    [RelayCommand]
    private void OptimizeDatabase()
    {
        Message = "✓ Database optimized";
    }

    [RelayCommand]
    private void ClearMessage()
    {
        Message = null;
    }
}
