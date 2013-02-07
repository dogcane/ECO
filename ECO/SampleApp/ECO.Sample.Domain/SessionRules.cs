using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Bender;

namespace ECO.Sample.Domain
{
    public static class SessionRules
    {
        public static OperationResult CheckSessionEvent(this OperationResult result, Event @event)
        {
            return ValueChecker<Event>.For(@event, result, "Event").Required("EVENT_REQUIRED");
        }

        public static OperationResult CheckSessionTitle(this OperationResult result, string title)
        {
            return ValueChecker<string>.For(title, result, "Title").Required("TITLE_REQUIRED").StringLength(50, "TITLE_TOO_LONG");
        }

        public static OperationResult CheckSessionDescription(this OperationResult result, string description)
        {
            return ValueChecker<string>.For(description, result, "Description").Required("DESCRIPTION_REQUIRED").StringLength(1000, "DESCRIPTION_TOO_LONG");
        }

        public static OperationResult CheckSessionLevel(this OperationResult result, int level)
        {
            return ValueChecker<int>.For(level, result, "Level").Into(new[] { 100, 200, 300, 400 }, "LEVEL_NOT_VALID");
        }

        public static OperationResult CheckSessionSpeaker(this OperationResult result, Speaker speaker)
        {
            return ValueChecker<Speaker>.For(speaker, result, "Speaker").Required("SPEAKER_REQUIRED");
        }
    }
}
