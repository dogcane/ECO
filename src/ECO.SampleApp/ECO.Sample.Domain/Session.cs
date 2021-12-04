using Resulz;
using Resulz.Validation;
using System;

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
            var result = OperationResult
                .MakeSuccess()
                .With(@event, nameof(Event)).Required("EVENT_REQUIRED")
                .With(title, nameof(Title)).Required("TITLE_REQUIRED").StringLength(50, "TITLE_TOO_LONG")
                .With(description, nameof(Description)).Required("DESCRIPTION_REQUIRED").StringLength(1000, "DESCRIPTION_TOO_LONG")
                .With(level, nameof(Level)).Into(new[] { 100, 200, 300, 400 }, "LEVEL_NOT_VALID")
                .With(speaker, nameof(Speaker)).Required("SPEAKER_REQUIRED")
                .Result;
            if (result.Success)
            {
                return new Session(@event, title, description, level, speaker);
            }
            return result;
        }

        #endregion
    }


}
