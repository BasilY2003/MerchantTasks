using DataLib.Models;
using FluentNHibernate.Mapping;

namespace DataLib.Maps
{
    public class JwtTokenMap : ClassMap<JwtToken>
    {
        public JwtTokenMap()
        {
            Table("JwtTokens_BASIL");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Token).Not.Nullable().Length(1000);
            Map(x => x.CreatedAt);
            Map(x => x.ExpiredAt);
            Map(x => x.IsRevoked);

            References(x => x.User)
                .Column("UserId")
                .Not.Nullable()
                .Unique();

            Map(x => x.UserId)
                .Column("UserId")
                .Not.Insert()
                .Not.Update();
        }
    }
}
