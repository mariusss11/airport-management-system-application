using Avalonia.Controls;
using AirportFlightManagement.ViewModels;
using System;
using System.ComponentModel;

namespace AirportFlightManagement.Views;

public partial class EditFlightModal : Window
{
    private FlightsViewModel? _viewModel;

    public EditFlightModal()
    {
        InitializeComponent();
        DataContextChanged += (s, e) =>
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            if (DataContext is FlightsViewModel vm)
            {
                _viewModel = vm;
                vm.PropertyChanged += ViewModel_PropertyChanged;
            }
        };
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(FlightsViewModel.IsModalOpen))
        {
            if (_viewModel != null && !_viewModel.IsModalOpen)
            {
                Close();
            }
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }
        base.OnClosed(e);
    }
}
