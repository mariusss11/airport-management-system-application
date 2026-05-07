using Avalonia.Controls;
using AirportFlightManagement.ViewModels;
using System;
using System.ComponentModel;

namespace AirportFlightManagement.Views;

public partial class EditAirportModal : Window
{
    private AirportsViewModel? _viewModel;

    public EditAirportModal()
    {
        InitializeComponent();
        DataContextChanged += (s, e) =>
        {
            if (_viewModel != null)
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;

            if (DataContext is AirportsViewModel vm)
            {
                _viewModel = vm;
                vm.PropertyChanged += ViewModel_PropertyChanged;
            }
        };
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AirportsViewModel.IsModalOpen))
        {
            if (_viewModel != null && !_viewModel.IsModalOpen)
                Close();
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        if (_viewModel != null)
            _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        base.OnClosed(e);
    }
}
