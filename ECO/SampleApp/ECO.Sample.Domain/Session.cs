using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Bender;

namespace ECO.Sample.Domain
{
    public class Session : Entity<Guid>
    {
        #region Properties

        public virtual Event Event { get; protected set; }

        public virtual string Title { get; protected set; }

        public virtual string Description { get; protected set; }

        public virtual int Level { get; protected set; }

        public virtual Speaker Speaker { get; protected set; }

        #endregion

        #region Ctor

        protected Session()
            : base(Guid.NewGuid())
        {

        }

        protected Session(Event @event, string title, string description, int level, Speaker speaker)
            : base(Guid.NewGuid())
        {
            Event = @event;
            Title = title;
            Description = description;
            Level = level;
            Speaker = speaker;
        }

        #endregion

        #region Factory_Methods

        internal static OperationResult<Session> Create(Event @event, string title, string description, int level, Speaker speaker)
        {
            return OperationResult
                .Begin()
                .CheckSessionEvent(@event)
                .CheckSessionTitle(title)
                .CheckSessionDescription(description)
                .CheckSessionLevel(level)
                .CheckSessionSpeaker(speaker)
                .IfSuccess<Session>(() =>
                {
                    return new Session(@event, title, description, level, speaker);
                });
        }

        #endregion
    }

    
}
