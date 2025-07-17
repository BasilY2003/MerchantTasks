namespace CommonLib.DTOs
{
    public class MerchantGroupDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<MerchantDTO> Merchants { get; set; } = new();
    }
}
