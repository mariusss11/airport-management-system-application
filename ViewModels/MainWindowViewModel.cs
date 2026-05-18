using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirportFlightManagement.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly AuthService _authService = new();

    [ObservableProperty]
    private ViewModelBase? currentPage;

    [ObservableProperty]
    private string title = "Flight Management System";

    [ObservableProperty]
    private bool isLoggedIn = false;

    public MainWindowViewModel()
    {
#if DEBUG
        // Auto-login in debug mode for faster development
        if (_authService.Login("admin", "password"))
        {
            IsLoggedIn = true;
            CurrentPage = new DashboardViewModel();
        }
        else
        {
            CurrentPage = new LoginViewModel(this);
        }
#else
        CurrentPage = new LoginViewModel(this);
#endif
    }

    public void NavigateTo(ViewModelBase viewModel)
    {
        CurrentPage = viewModel;
    }

    public void SetLoggedIn(bool loggedIn)
    {
        IsLoggedIn = loggedIn;
    }

    [RelayCommand]
    public void Logout()
    {
        _authService.Logout();
        IsLoggedIn = false;
        CurrentPage = new LoginViewModel(this);
    }
}
