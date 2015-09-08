using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Bender;

namespace ECO.Sample.Domain
{
    public class Event : AggregateRoot<Guid>
    {
        #region Fields

        //protected IList<Session> _Sessions = new List<Session>();

        protected IList<Session> _Sessions { get; set; }

        #endregion

        #region Properties

        public virtual string Name { get; protected set; }

        public virtual string Description { get; protected set; }

        public virtual Period Period { get; protected set; }

        public virtual IEnumerable<Session> Sessions
        {
            get { return _Sessions; }
        }

        #endregion

        #region Ctor

        //x Proxy/ORM
        protected Event()
            : base(Guid.NewGuid())
        {
            _Sessions = new List<Session>();
        }

        protected Event(string name, string description, Period period)
            : base(Guid.NewGuid())
        {
            Name = name;
            Description = description;
            Period = period;
            _Sessions = new List<Session>();
        }

        #endregion

        #region Factory_Methods

        public static OperationResult<Event> Create(string name, string description, Period period)
        {
            return OperationResult
                .Begin()
                .CheckEventName(name)
                .CheckEventDescription(description)
                .CheckEventPeriod(period)
                .IfSuccess<Event>(() =>
                {
                    return new Event(name, description, period);
                });
        }

        #endregion

        #region Public_Methods

        public virtual OperationResult ChangeInformation(string name, string description, Period period)
        {
            return OperationResult
                .Begin()
                .CheckEventName(name)
                .CheckEventDescription(description)
                .CheckEventPeriod(period)
                .IfSuccess(() =>
                {
                    Name = name;
                    Description = description;
                    Period = period;
                });
        }

        public virtual OperationResult<Session> AddSession(string title, string description, int level, Speaker speaker)
        {
            return Session
                .Create(this, title, description, level, speaker)
                .IfSuccess(session =>
                {
                    _Sessions.Add(session);
                });
        }

        public virtual OperationResult RemoveSession(Session session)
        {
            if (_Sessions.Contains(session))
            {
                _Sessions.Remove(session);
            }
            return OperationResult.MakeSuccess();
        }

        #endregion
    }
}
