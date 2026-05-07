using Avalonia.Controls;
using Avalonia.Interactivity;
using AirportFlightManagement.ViewModels;
using AirportFlightManagement.Models;
using System;

namespace AirportFlightManagement.Views;

public partial class PlanesView : UserControl
{
    public PlanesView()
    {
        InitializeComponent();
    }

    private void OnEditClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is Button button && DataContext is PlanesViewModel vm && button.DataContext is Plane plane)
            {
                var mainWindow = TopLevel.GetTopLevel(this) as Window;
                if (mainWindow != null)
                {
                    vm.SelectPlane(plane);
                    vm.IsModalOpen = true;
                    var modal = new EditPlaneModal { DataContext = vm };
                    modal.ShowDialog(mainWindow);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OnEditClick error: {ex}");
        }
    }

    private void OnAddPlaneClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is PlanesViewModel vm)
        {
            var mainWindow = TopLevel.GetTopLevel(this) as Window;
            if (mainWindow != null)
            {
                vm.NewPlaneCommand.Execute(null);
                vm.IsModalOpen = true;
                var modal = new EditPlaneModal { DataContext = vm };
                modal.ShowDialog(mainWindow);
            }
        }
    }
}
