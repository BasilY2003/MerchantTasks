namespace DataLib.Models;

public class ServerKey
{
    public virtual int Id { get; set; }
    public virtual string PublicKey { get; set; } = string.Empty;
    public virtual string PrivateKey { get; set; } = string.Empty;
    public virtual DateTime CreatedAt { get; set; }
}
