using ECO.Sample.Domain;
using NhMapping = NHibernate.Mapping;

namespace ECO.Sample.Infrastructure.DAL.NHibernate.Mapping
{
    public class EventMapping : NhMapping.ByCode.Conformist.ClassMapping<Event>
    {
        public EventMapping()
        {
            Table("Events");
            Id(ev => ev.Identity, mapper => mapper.Column("Id"));
            Property(ev => ev.Name);
            Property(ev => ev.Description);
            Component(ev => ev.Period, aPeriod =>
            {
                aPeriod.Property(per => per.StartDate);
                aPeriod.Property(per => per.EndDate);
            });
            Bag(ev => ev.Sessions,
                sessions => {
                    sessions.Table("Sessions");
                    sessions.Key(ses => ses.Column("FK_Event"));
                    sessions.Access(NhMapping.ByCode.Accessor.NoSetter);
                    sessions.Inverse(true);
                    sessions.Cascade(NhMapping.ByCode.Cascade.DeleteOrphans | NhMapping.ByCode.Cascade.All);
                }, mapping => mapping.OneToMany());
        }
    }
}
