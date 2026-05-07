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

    private void OnEditClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is Button button && DataContext is PlanesViewModel vm)
            {
                if (button.DataContext is Plane plane)
                    vm.SelectPlane(plane);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OnEditClick error: {ex}");
        }
    }
}
