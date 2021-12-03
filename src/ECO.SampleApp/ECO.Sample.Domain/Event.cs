using Resulz;
using Resulz.Validation;
using System;
using System.Collections.Generic;

namespace ECO.Sample.Domain
{
    public class Event : AggregateRoot<Guid>
    {
        #region Fields

        protected IList<Session> _Sessions = new List<Session>();

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
            var result = OperationResult
                .MakeSuccess()
                .With(name, nameof(Name)).Required("NAME_REQUIRED").StringLength(50, "NAME_TOO_LONG")
                .With(description, nameof(Description)).Required("DESCRIPTION_REQUIRED").StringLength(200, "DESCRIPTION_TOO_LONG")
                .With(period.StartDate, "Period.StartDate").LessThenOrEqual(period.EndDate, "STARTDATE_GREATER_ENDDATE")
                .Result;
            if (result.Success)
            {
                return new Event(name, description, period);
            }
            return result;
        }

        #endregion

        #region Public_Methods

        public virtual OperationResult ChangeInformation(string name, string description, Period period)
        {
            var result = OperationResult
                .MakeSuccess()
                .With(name, nameof(Name)).Required("NAME_REQUIRED").StringLength(50, "NAME_TOO_LONG")
                .With(description, nameof(Description)).Required("DESCRIPTION_REQUIRED").StringLength(200, "DESCRIPTION_TOO_LONG")
                .With(period.StartDate, "Period.StartDate").LessThenOrEqual(period.EndDate, "STARTDATE_GREATER_ENDDATE")
                .Result;
            if (result.Success)
            {
                Name = name;
                Description = description;
                Period = period;
            }
            return result;
        }

        public virtual OperationResult<Session> AddSession(string title, string description, int level, Speaker speaker)
        {
            var session = Session.Create(this, title, description, level, speaker);
            if (session.Success)
            {
                _Sessions.Add(session.Value);
            }
            return session;
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
