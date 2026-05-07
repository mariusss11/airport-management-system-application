using Avalonia.Controls;
using Avalonia.Interactivity;
using AirportFlightManagement.ViewModels;
using AirportFlightManagement.Models;
using System;

namespace AirportFlightManagement.Views;

public partial class AirportsView : UserControl
{
    public AirportsView()
    {
        InitializeComponent();
    }

    private void OnEditClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is Button button && DataContext is AirportsViewModel vm && button.DataContext is Airport airport)
            {
                var mainWindow = TopLevel.GetTopLevel(this) as Window;
                if (mainWindow != null)
                {
                    vm.SelectAirport(airport);
                    vm.IsModalOpen = true;
                    var modal = new EditAirportModal { DataContext = vm };
                    modal.ShowDialog(mainWindow);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OnEditClick error: {ex}");
        }
    }

    private void OnAddAirportClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is AirportsViewModel vm)
        {
            var mainWindow = TopLevel.GetTopLevel(this) as Window;
            if (mainWindow != null)
            {
                vm.NewAirportCommand.Execute(null);
                vm.IsModalOpen = true;
                var modal = new EditAirportModal { DataContext = vm };
                modal.ShowDialog(mainWindow);
            }
        }
    }
}
