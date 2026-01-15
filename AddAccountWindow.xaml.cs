using QuickLogin.Models;
using QuickLogin.Services;
using System.Windows;
using System.Windows.Controls;

namespace QuickLogin
{
    public partial class AddAccountWindow : Window
    {
        public AccountEntry? NewAccount { get; private set; }

        public AddAccountWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save button clicked — either add or edit account
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string displayName = DisplayNameText.Text.Trim();
            string username = UsernameText.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(displayName) ||
                string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("All fields are required.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string credentialKey = Guid.NewGuid().ToString();

            // Save password securely in Windows Credential Manager
            CredentialService.Save(credentialKey, username, password);

            NewAccount = new AccountEntry
            {
                DisplayName = displayName,
                Username = username,
                CredentialKey = credentialKey,
                         };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
