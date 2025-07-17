using DataLib.Models;
using FluentNHibernate.Mapping;

namespace DataLib.Maps
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users_BASIL");

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Username).Not.Nullable();
            Map(x => x.PasswordHash).Not.Nullable();
            Map(x => x.Role).Not.Nullable();
            Map(x => x.CreatedAt);
            Map(x => x.DeletedAt);

            HasOne(x => x.Token)
                .PropertyRef("User")
                .Cascade.None();  
            ;
        }
    }
}