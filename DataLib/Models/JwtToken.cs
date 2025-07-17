namespace DataLib.Models
{
    public class JwtToken
    {
        public virtual long Id { get; set; }
        public virtual string Token { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual long UserId { get; } 

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? ExpiredAt { get; set; }
        public virtual bool IsRevoked { get; set; } = false;
    }
}
