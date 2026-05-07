using AirportFlightManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AirportFlightManagement.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly AuthService _authService = new();
    private readonly MainWindowViewModel _mainWindow;

    [ObservableProperty]
    private string username = "admin";

    [ObservableProperty]
    private string password = "password";

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isLoading = false;

    public LoginViewModel(MainWindowViewModel mainWindow)
    {
        _mainWindow = mainWindow;
    }

    [RelayCommand]
    private async Task Login()
    {
        ErrorMessage = null;
        IsLoading = true;

        await Task.Delay(500);

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Username and password are required";
            IsLoading = false;
            return;
        }

        if (_authService.Login(Username, Password))
        {
            _mainWindow.SetLoggedIn(true);
            _mainWindow.NavigateTo(new DashboardViewModel());
        }
        else
        {
            ErrorMessage = _authService.LastLoginError ?? DatabaseService.Instance.LastError ?? "Invalid username or password";
            Password = "";
        }

        IsLoading = false;
    }
}
