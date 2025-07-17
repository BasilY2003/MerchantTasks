using FluentNHibernate.Mapping;
using CommonLib.Models;

namespace CommonLib.Mappings
{
    public class MerchantsMap : ClassMap<Merchants>
    {
        public MerchantsMap()
        {
            Table("MERCHANTS_BASIL");

            Id(x => x.Id).Column("ID").GeneratedBy.Identity();

            Map(x => x.Name).Column("NAME").Not.Nullable();
            Map(x => x.BusinessType).Column("BUSINESS_TYPE").Not.Nullable();
            Map(x => x.Status).Column("STATUS").Not.Nullable();
            Map(x => x.DeletedAt).Column("DELETED_AT").Nullable();
            Map(x => x.CreatedAt).Column("CREATED_AT").Not.Nullable();
            Map(x => x.UpdatedAt).Column("UPDATED_AT").Not.Nullable();
            Map(x => x.ManagerName)
                .Column("MANAGER_NAME")
                .Not.Nullable();

            References(x => x.MerchantsGroup)
                .Column("GROUP_ID") 
                .Not.Nullable();

            HasMany(x => x.MerchantBranches)
                .KeyColumn("MERCHANT_ID")
                .Inverse();

        }
    }
}
