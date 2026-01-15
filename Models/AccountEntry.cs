namespace QuickLogin.Models
{
    public class AccountEntry
    {
        public string DisplayName { get; set; } = "";
        public string CredentialKey { get; set; } = "";
        public string Username { get; set; } = "";

        // Temporary for editing only; never saved to JSON
        [System.Text.Json.Serialization.JsonIgnore]
        public string Password { get; set; } = "";
    }
}
