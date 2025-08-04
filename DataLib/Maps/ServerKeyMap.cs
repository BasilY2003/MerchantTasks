using DataLib.Models;
using FluentNHibernate.Mapping;

namespace DataLib.Maps
{
    public class ServerKeyMap : ClassMap<ServerKey>
    {
        public ServerKeyMap()
        {
            Table("SERVER_KEYS_BASIL");

            Id(x => x.Id);
            Map(x => x.PublicKey).Not.Nullable().Length(4096);
            Map(x => x.PrivateKey).Not.Nullable().Length(4096);
            Map(x => x.CreatedAt);
        }
    }
}
