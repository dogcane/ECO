using Resulz;
using Resulz.Validation;
using System;

namespace ECO.Sample.Domain
{
    public static class EventRules
    {
        public static OperationResult CheckEventName(this OperationResult result, string name)
        {
            return new ValueChecker<string>(name, result, "Name").Required("NAME_REQUIRED").StringLength(50, "NAME_TOO_LONG");
        }

        public static OperationResult CheckEventDescription(this OperationResult result, string description)
        {
            return new ValueChecker<string>(description, result, "Description").Required("DESCRIPTION_REQUIRED").StringLength(200, "DESCRIPTION_TOO_LONG");
        }

        public static OperationResult CheckEventPeriod(this OperationResult result, Period period)
        {
            return new ValueChecker<DateTime>(period.StartDate, result, "Period.StartDate").LessThenOrEqual(period.EndDate, "STARTDATE_GREATER_ENDDATE");
        }
    }
}
