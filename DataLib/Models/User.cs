namespace DataLib.Models
{
    public class User
    {
        public virtual long Id { get; set; }
        public virtual string Username { get; set; } = null!;
        public virtual string PasswordHash { get; set; } = null!;
        public virtual string Role { get; set; } = "User";
        public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual DateTime? DeletedAt { get; set; }
        public virtual JwtToken? Token { get; set; }

        public virtual string PublicKey { get; set; } = string.Empty;
    }
}
