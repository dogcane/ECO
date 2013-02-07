using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Bender;

namespace ECO.Sample.Domain
{
    public static class EventRules
    {
        public static OperationResult CheckEventName(this OperationResult result, string name)
        {
            return ValueChecker<string>.For(name, result, "Name").Required("NAME_REQUIRED").StringLength(50, "NAME_TOO_LONG");
        }

        public static OperationResult CheckEventDescription(this OperationResult result, string description)
        {
            return ValueChecker<string>.For(description, result, "Description").Required("DESCRIPTION_REQUIRED").StringLength(200, "DESCRIPTION_TOO_LONG");
        }

        public static OperationResult CheckEventPeriod(this OperationResult result, Period period)
        {
            return ValueChecker<DateTime>.For(period.StartDate, result, "Period.StartDate").LessThenOrEqual(period.EndDate, "STARTDATE_GREATER_ENDDATE");
        }
    }
}
