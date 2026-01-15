using CredentialManagement;

namespace QuickLogin.Services
{
    public static class CredentialService
    {
        /// <summary>
        /// Save a credential (username + password) securely in Windows Credential Manager
        /// </summary>
        /// <param name="key">Unique key for this credential</param>
        /// <param name="username">Username to save</param>
        /// <param name="password">Password to save</param>
        public static void Save(string key, string username, string password)
        {
            using var cred = new Credential
            {
                Target = key,
                Username = username,
                Password = password,
                PersistanceType = PersistanceType.LocalComputer
            };
            cred.Save();
        }

        /// <summary>
        /// Get the password for a saved credential
        /// </summary>
        /// <param name="key">Key used when saving the credential</param>
        /// <returns>Password string or null if not found</returns>
        public static string? GetPassword(string key)
        {
            using var cred = new Credential { Target = key };
            return cred.Load() ? cred.Password : null;
        }

        /// <summary>
        /// Delete a saved credential from Windows Credential Manager
        /// </summary>
        /// <param name="key">Key used when saving the credential</param>
        public static void Delete(string key)
        {
            using var cred = new Credential { Target = key };
            cred.Delete();
        }
    }
}
