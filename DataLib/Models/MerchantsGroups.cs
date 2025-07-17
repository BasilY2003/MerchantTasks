

namespace CommonLib.Models
{
    public class MerchantsGroups 
    {
      
        public virtual long Id { get; set; }

        public virtual string Name { get; set; } = string.Empty;

        public virtual DateTime? DeletedAt { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        public virtual IList<Merchants>? Merchants { get; set; }



    }
}
