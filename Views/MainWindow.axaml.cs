using Avalonia.Controls;
using Avalonia.Interactivity;
using AirportFlightManagement.ViewModels;
using System;

namespace AirportFlightManagement.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel? _viewModel;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        _viewModel = DataContext as MainWindowViewModel;
    }

    public void OnNavClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        if (button.Tag is not string tag) return;

        ViewModelBase? viewModel = tag switch
        {
            "Dashboard" => new DashboardViewModel(),
            "Flights" => new FlightsViewModel(),
            "Search" => new SearchViewModel(),
            "Reports" => new ReportsViewModel(),
            "Archive" => new ArchiveViewModel(),
            "Planes" => new PlanesViewModel(),
            "Airports" => new AirportsViewModel(),
            "Settings" => new SettingsViewModel(),
            _ => null
        };

        if (viewModel != null && _viewModel != null)
        {
            _viewModel.NavigateTo(viewModel);
        }
    }
}