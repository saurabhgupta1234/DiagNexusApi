namespace DiagNexusApi.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; } // e.g., "Admin", "User", etc.
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Additional properties can be added as needed
    }
}
