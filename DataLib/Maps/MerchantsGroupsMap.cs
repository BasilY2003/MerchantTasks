using FluentNHibernate.Mapping;
using CommonLib.Models;

namespace CommonLib.Mappings
{
    public class MerchantsGroupsMap : ClassMap<MerchantsGroups>
    {
        public MerchantsGroupsMap()
        {
            Table("MERCHANTS_GROUPS_BASIL"); 

            Id(x => x.Id).Column("ID").GeneratedBy.Identity();

            Map(x => x.Name).Column("NAME").Not.Nullable();
            Map(x => x.DeletedAt).Column("DELETED_AT").Nullable();
            Map(x => x.CreatedAt).Column("CREATED_AT").Not.Nullable();
            Map(x => x.UpdatedAt).Column("UPDATED_AT").Not.Nullable();

            HasMany(x => x.Merchants).KeyColumn("GROUP_ID").Inverse();
        }
    }
}
