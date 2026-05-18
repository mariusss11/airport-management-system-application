using AirportFlightManagement.Models;
using BCrypt.Net;

namespace AirportFlightManagement.Services;

public class AuthService
{
    public User? CurrentUser { get; private set; }
    public string? LastLoginError { get; private set; }

    public bool Login(string username, string password)
    {
        var totalUsers = DatabaseService.Instance.Users.Count;
        var user = DatabaseService.Instance.GetUserByUsername(username);

        if (user == null)
        {
            LastLoginError = $"User '{username}' not found (total users loaded: {totalUsers})";
            return false;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            LastLoginError = $"Password mismatch for user '{username}'";
            return false;
        }

        LastLoginError = null;
        CurrentUser = user;
        DatabaseService.Instance.UpdateLastLogin(user.Id);
        return true;
    }

    public void Logout()
    {
        CurrentUser = null;
    }

    public bool IsLoggedIn => CurrentUser != null;

    public static string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);
}
