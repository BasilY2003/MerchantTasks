using FluentNHibernate.Mapping;
using CommonLib.Models;

namespace CommonLib.Mappings
{
    public class MerchantBranchesMap : ClassMap<MerchantBranches>
    {
        public MerchantBranchesMap()
        {
            Table("MERCHANT_BRANCHES_BASIL");

            Id(x => x.Id)
                .Column("ID")
                .GeneratedBy.Identity();

            Map(x => x.BranchName).Column("BRANCH_NAME").Not.Nullable();

            References(x => x.Merchant)
                .Column("MERCHANT_ID")
                .Not.Nullable();

            Map(x => x.ContactPersonId).Column("CONTACT_PERSON_ID").Not.Nullable();
            Map(x => x.CityId).Column("CITY_ID").Not.Nullable();
            Map(x => x.GovernateId).Column("GOVERNATE_ID").Not.Nullable();
            Map(x => x.AlHai).Column("AL_HAI").Nullable();
            Map(x => x.Address).Column("ADDRESS").Not.Nullable();
            Map(x => x.Region).Column("REGION").Not.Nullable();
            Map(x => x.Fax).Column("FAX").Nullable();
            Map(x => x.Website).Column("WEBSITE").Nullable();
            Map(x => x.Mobile).Column("MOBILE").Not.Nullable();
            Map(x => x.Phone).Column("PHONE").Nullable();
            Map(x => x.Gps).Column("GPS").Nullable();
            Map(x => x.Status).Column("STATUS").Not.Nullable();
            Map(x => x.IsMainBranch).Column("IS_MAIN_BRANCH").Not.Nullable();
            Map(x => x.DeletedAt).Column("DELETED_AT").Nullable();
            Map(x => x.CreatedAt).Column("CREATED_AT").Not.Nullable();
            Map(x => x.UpdatedAt).Column("UPDATED_AT").Not.Nullable();
        }
    }
}
