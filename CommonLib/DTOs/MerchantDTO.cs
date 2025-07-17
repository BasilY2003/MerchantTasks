namespace CommonLib.DTOs
{
    public class MerchantDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BusinessType { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ManagerName { get; set; } = string.Empty;

        public MerchantGroupDto? MerchantsGroup { get; set; }
        public List<MerchantBranchDto>? MerchantBranches { get; set; }
     }
    }
