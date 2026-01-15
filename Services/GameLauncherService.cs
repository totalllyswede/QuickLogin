using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace QuickLogin.Services
{
    public static class GameLauncherService
    {
        private static readonly string _settingsFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "QuickLogin",
            "settings.json"
        );

        /// <summary>
        /// Load the saved WoW executable path
        /// </summary>
        public static string? LoadWowExePath()
        {
            try
            {
                if (!File.Exists(_settingsFile))
                    return null;

                string json = File.ReadAllText(_settingsFile);
                var settings = JsonSerializer.Deserialize<LauncherSettings>(json);
                return settings?.WowExePath;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Load whether auto-launch is enabled
        /// </summary>
        public static bool LoadLaunchEnabled()
        {
            try
            {
                if (!File.Exists(_settingsFile))
                    return false;

                string json = File.ReadAllText(_settingsFile);
                var settings = JsonSerializer.Deserialize<LauncherSettings>(json);
                return settings?.LaunchEnabled ?? false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Save the WoW executable path
        /// </summary>
        public static void SaveWowExePath(string path)
        {
            try
            {
                var settings = LoadSettings();
                settings.WowExePath = path;
                SaveSettings(settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Save whether auto-launch is enabled
        /// </summary>
        public static void SaveLaunchEnabled(bool enabled)
        {
            try
            {
                var settings = LoadSettings();
                settings.LaunchEnabled = enabled;
                SaveSettings(settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Launch World of Warcraft using the saved path
        /// </summary>
        public static bool LaunchWow(string? exePath = null)
        {
            try
            {
                string? path = exePath ?? LoadWowExePath();

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    return false;

                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(path)
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static LauncherSettings LoadSettings()
        {
            try
            {
                if (!File.Exists(_settingsFile))
                    return new LauncherSettings();

                string json = File.ReadAllText(_settingsFile);
                return JsonSerializer.Deserialize<LauncherSettings>(json) ?? new LauncherSettings();
            }
            catch
            {
                return new LauncherSettings();
            }
        }

        private static void SaveSettings(LauncherSettings settings)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_settingsFile)!);
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsFile, json);
        }

        private class LauncherSettings
        {
            public string? WowExePath { get; set; }
            public bool LaunchEnabled { get; set; }
        }
    }
}