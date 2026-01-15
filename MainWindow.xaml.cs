using QuickLogin.Models;
using QuickLogin.Services;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace QuickLogin
{
    public partial class MainWindow : Window
    {
        private readonly List<AccountEntry> _accounts = new();
        private readonly string _dataFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "QuickLogin",
            "accounts.json"
        );

        private string? _wowExePath;
        private bool _launchWowEnabled;

        public MainWindow()
        {
            InitializeComponent();
            LoadAccountsFromFile();
            LoadLauncherSettings();
        }

        /// <summary>
        /// Load accounts from JSON file into memory and populate dropdown
        /// </summary>
        private void LoadAccountsFromFile()
        {
            try
            {
                if (!File.Exists(_dataFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_dataFile)!);
                    File.WriteAllText(_dataFile, "[]");
                }

                string json = File.ReadAllText(_dataFile);
                var accounts = JsonSerializer.Deserialize<List<AccountEntry>>(json);
                if (accounts != null)
                    _accounts.AddRange(accounts);

                AccountCombo.ItemsSource = _accounts;
                AccountCombo.SelectedIndex = _accounts.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load accounts: {ex.Message}");
            }
        }

        /// <summary>
        /// Load the saved WoW executable path and launch preference on startup
        /// </summary>
        private void LoadLauncherSettings()
        {
            _wowExePath = GameLauncherService.LoadWowExePath();
            _launchWowEnabled = GameLauncherService.LoadLaunchEnabled();

            // Update UI to show current settings
            if (!string.IsNullOrEmpty(_wowExePath))
            {
                ExePathText.Text = Path.GetFileName(_wowExePath);
            }

            LaunchWowCheckbox.IsChecked = _launchWowEnabled;
        }

        /// <summary>
        /// Save current account list to JSON file (metadata only)
        /// </summary>
        private void SaveAccountsToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_dataFile, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save accounts: {ex.Message}");
            }
        }

        /// <summary>
        /// Browse for WoW.exe
        /// </summary>
        private void BrowseWowExe_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Executable Files (*.exe)|*.exe",
                Title = "Select World of Warcraft Executable",
                FileName = "Wow.exe"
            };

            if (dialog.ShowDialog() == true)
            {
                _wowExePath = dialog.FileName;
                GameLauncherService.SaveWowExePath(_wowExePath);

                ExePathText.Text = Path.GetFileName(_wowExePath);

                MessageBox.Show("WoW executable path saved!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Handle checkbox change for launch preference
        /// </summary>
        private void LaunchWowCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            _launchWowEnabled = LaunchWowCheckbox.IsChecked == true;
            GameLauncherService.SaveLaunchEnabled(_launchWowEnabled);
        }

        /// <summary>
        /// Add a new account
        /// </summary>
        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddAccountWindow { Owner = this };

            if (addWindow.ShowDialog() == true && addWindow.NewAccount != null)
            {
                _accounts.Add(addWindow.NewAccount);
                AccountCombo.Items.Refresh();
                AccountCombo.SelectedItem = addWindow.NewAccount;

                SaveAccountsToFile();
            }
        }

        /// <summary>
        /// Edit the selected account
        /// </summary>
        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            if (AccountCombo.SelectedItem is not AccountEntry account)
                return;

            var editWindow = new AddAccountWindow
            {
                Owner = this,
                DisplayNameText = { Text = account.DisplayName },
                UsernameText = { Text = account.Username },
                PasswordBox = { Password = CredentialService.GetPassword(account.CredentialKey) ?? "" }
            };

            if (editWindow.ShowDialog() == true && editWindow.NewAccount != null)
            {
                account.DisplayName = editWindow.NewAccount.DisplayName;
                account.Username = editWindow.NewAccount.Username;

                // Update password in Credential Manager
                CredentialService.Save(account.CredentialKey, account.Username, editWindow.NewAccount.Password);

                AccountCombo.Items.Refresh();
                SaveAccountsToFile();
            }
        }

        /// <summary>
        /// Remove the selected account
        /// </summary>
        private void RemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (AccountCombo.SelectedItem is not AccountEntry account)
                return;

            if (MessageBox.Show($"Are you sure you want to delete account '{account.DisplayName}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            // Remove from list
            _accounts.Remove(account);
            AccountCombo.Items.Refresh();

            // Delete password from Credential Manager
            CredentialService.Delete(account.CredentialKey);

            // Save updated list
            SaveAccountsToFile();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Open the Help window
        /// </summary>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow { Owner = this };
            helpWindow.ShowDialog();
        }

        /// <summary>
        /// Focus World of Warcraft, type credentials, then close app
        /// </summary>
        private async void TypeCredentials_Click(object sender, RoutedEventArgs e)
        {
            await PerformLogin(shouldClose: true);
        }

        /// <summary>
        /// Focus World of Warcraft, type credentials, do not close app
        /// </summary>
        private async void TypeCredentials_ClickStay(object sender, RoutedEventArgs e)
        {
            await PerformLogin(shouldClose: false);
        }

        /// <summary>
        /// Main login logic - launches WoW if enabled, then types credentials
        /// </summary>
        private async Task PerformLogin(bool shouldClose)
        {
            if (AccountCombo.SelectedItem is not AccountEntry account)
                return;

            var password = CredentialService.GetPassword(account.CredentialKey);
            if (password == null)
            {
                MessageBox.Show("Password not found in Credential Manager.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            const string targetWindow = "World of Warcraft";

            // Launch WoW if checkbox is enabled
            if (_launchWowEnabled)
            {
                if (string.IsNullOrEmpty(_wowExePath) || !File.Exists(_wowExePath))
                {
                    MessageBox.Show("WoW executable path not set. Please browse for Wow.exe first.",
                        "Path Not Set", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool launched = GameLauncherService.LaunchWow(_wowExePath);

                if (!launched)
                {
                    MessageBox.Show("Failed to launch World of Warcraft.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Wait 10 seconds for game to start
                await Task.Delay(10000);
            }

            // Try to focus WoW window
            bool focused = WindowHelper.FocusWindowByTitle(targetWindow);
            if (!focused)
            {
                MessageBox.Show($"Could not find window: {targetWindow}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Wait for window to be ready
            await Task.Delay(500);

            // Type credentials with delay
            await TypingService.TypeLoginAsync(account.Username, password);

            // Extra delay to ensure TAB/ENTER registers
            await Task.Delay(200);

            // Refocus WoW just in case
            WindowHelper.FocusWindowByTitle(targetWindow);

            // Close app if requested
            if (shouldClose)
                Application.Current.Shutdown();
        }

        private void AccountCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Can be used to enable/disable buttons based on selection
        }
    }
}