using ECO.Sample.Domain;
using nh = NHibernate;
using nhmapping = NHibernate.Mapping;

namespace ECO.Sample.Infrastructure.DAL.NHibernate.Mapping
{
    public class SpeakerMapping : nhmapping.ByCode.Conformist.ClassMapping<Speaker>
    {
        public SpeakerMapping()
        {
            Table("Speakers");
            Id(ev => ev.Identity, mapper => mapper.Column("Id"));
            Property(ev => ev.Name);
            Property(ev => ev.Surname);
            Property(ev => ev.Description);
            Property(ev => ev.Age);
        }
    }
}
