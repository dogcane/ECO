using Resulz;
using Resulz.Validation;

namespace ECO.Sample.Domain
{
    public static class SessionRules
    {
        public static OperationResult CheckSessionEvent(this OperationResult result, Event @event)
        {
            return new ValueChecker<Event>(@event, result, "Event").Required("EVENT_REQUIRED");
        }

        public static OperationResult CheckSessionTitle(this OperationResult result, string title)
        {
            return new ValueChecker<string>(title, result, "Title").Required("TITLE_REQUIRED").StringLength(50, "TITLE_TOO_LONG");
        }

        public static OperationResult CheckSessionDescription(this OperationResult result, string description)
        {
            return new ValueChecker<string>(description, result, "Description").Required("DESCRIPTION_REQUIRED").StringLength(1000, "DESCRIPTION_TOO_LONG");
        }

        public static OperationResult CheckSessionLevel(this OperationResult result, int level)
        {
            return new ValueChecker<int>(level, result, "Level").Into(new[] { 100, 200, 300, 400 }, "LEVEL_NOT_VALID");
        }

        public static OperationResult CheckSessionSpeaker(this OperationResult result, Speaker speaker)
        {
            return new ValueChecker<Speaker>(speaker, result, "Speaker").Required("SPEAKER_REQUIRED");
        }
    }
}
