namespace CommonLib.Models
{
    public class Merchants
    {   
        public virtual long Id { get; set; }
        public virtual string Name { get; set; } = string.Empty;
        public virtual int BusinessType { get; set; }
        public virtual bool Status { get; set; }
        public virtual DateTime? DeletedAt { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual string ManagerName { get; set; } = string.Empty;

        public required virtual MerchantsGroups MerchantsGroup { get; set; }

        public virtual IList<MerchantBranches>? MerchantBranches { get; set; }

    }
}
