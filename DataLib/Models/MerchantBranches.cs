

using System.ComponentModel.Design;

namespace CommonLib.Models
{
    public class MerchantBranches
    {

        public virtual long Id { get; set; } 

        public virtual string BranchName { get; set; }

        public virtual Merchants Merchant { get; set; }

        public virtual long ContactPersonId { get; set; }

        public virtual long CityId { get; set; }

        public virtual long GovernateId { get; set; }

        public virtual string? AlHai {  get; set; }

        public virtual string Address { get; set; }

        public virtual int Region { get; set; }

        public virtual string? Fax { get; set; }

        public virtual string? Website { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string Phone { get; set; }

        public virtual string? Gps { get; set; }

        public virtual bool Status { get; set; }

        public virtual bool IsMainBranch { get; set; }

        public virtual DateTime? DeletedAt { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime UpdatedAt { get; set; }



    }

}
