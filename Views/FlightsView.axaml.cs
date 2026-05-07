using Avalonia.Controls;
using Avalonia.Interactivity;
using AirportFlightManagement.Models;
using AirportFlightManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportFlightManagement.Views;

public partial class FlightsView : UserControl
{
    public FlightsView()
    {
        InitializeComponent();
    }

    private void EditButton_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is FlightsViewModel viewModel && sender is Button button && button.DataContext is Flight flight)
        {
            var mainWindow = TopLevel.GetTopLevel(this) as Window;
            if (mainWindow != null)
            {
                viewModel.SelectFlightCommand.Execute(flight);
                viewModel.IsModalOpen = true;
                var modal = new EditFlightModal { DataContext = viewModel };
                modal.ShowDialog(mainWindow);
            }
        }
    }

    private void NewFlightButton_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is FlightsViewModel viewModel)
        {
            var mainWindow = TopLevel.GetTopLevel(this) as Window;
            if (mainWindow != null)
            {
                viewModel.IsModalOpen = true;
                var modal = new EditFlightModal { DataContext = viewModel };
                modal.ShowDialog(mainWindow);
            }
        }
    }
}
