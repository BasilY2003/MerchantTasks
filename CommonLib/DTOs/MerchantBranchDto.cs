namespace CommonLib.DTOs
{
    public class MerchantBranchDto
    {
        public long Id { get; set; }

        public string BranchName { get; set; } = string.Empty;

        public long MerchantId { get; set; }

        public string Mobile { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public bool IsMainBranch { get; set; }

        public bool Status { get; set; }

        public MerchantGroupDto? MerchantsGroup { get; set; }
    }
}
