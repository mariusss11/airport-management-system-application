using Avalonia.Controls;
using AirportFlightManagement.ViewModels;
using System;
using System.ComponentModel;

namespace AirportFlightManagement.Views;

public partial class EditPlaneModal : Window
{
    private PlanesViewModel? _viewModel;

    public EditPlaneModal()
    {
        InitializeComponent();
        DataContextChanged += (s, e) =>
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            if (DataContext is PlanesViewModel vm)
            {
                _viewModel = vm;
                vm.PropertyChanged += ViewModel_PropertyChanged;
            }
        };
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PlanesViewModel.IsModalOpen))
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
