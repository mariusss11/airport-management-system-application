using System;
using System.IO;
using Avalonia;
using Avalonia.Styling;

namespace AirportFlightManagement.Services;

public class ThemeService
{
    private static readonly Lazy<ThemeService> _instance = new(() => new ThemeService());
    public static ThemeService Instance => _instance.Value;

    private static readonly string ConfigPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AirportFlightManagement",
        "theme.cfg");

    public bool IsDarkMode { get; private set; } = true;

    private ThemeService() { }

    public void Load()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                var value = File.ReadAllText(ConfigPath).Trim();
                IsDarkMode = !string.Equals(value, "Light", StringComparison.OrdinalIgnoreCase);
            }
        }
        catch
        {
            IsDarkMode = true;
        }

        Apply();
    }

    public void SetDarkMode(bool dark)
    {
        IsDarkMode = dark;
        Apply();
        Save();
    }

    private void Apply()
    {
        if (Application.Current is null) return;
        Application.Current.RequestedThemeVariant = IsDarkMode ? ThemeVariant.Dark : ThemeVariant.Light;
    }

    private void Save()
    {
        try
        {
            var dir = Path.GetDirectoryName(ConfigPath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            File.WriteAllText(ConfigPath, IsDarkMode ? "Dark" : "Light");
        }
        catch
        {
            // best-effort persistence
        }
    }
}
