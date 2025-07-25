﻿using ECO.Sample.Domain;
using NhMapping = NHibernate.Mapping;

namespace ECO.Sample.Infrastructure.DAL.NHibernate.Mapping
{
    public class SessionMapping : NhMapping.ByCode.Conformist.ClassMapping<Session>
    {
        public SessionMapping()
        {
            Table("Sessions");
            Id(ev => ev.Identity, mapper => mapper.Column("Id"));
            Property(ev => ev.Title);
            Property(ev => ev.Level);
            Property(ev => ev.Description);
            ManyToOne(ev => ev.Event, mapper => mapper.Column("FK_Event"));
            ManyToOne(ev => ev.Speaker, mapper => mapper.Column("FK_Speaker"));
        }
    }
}
