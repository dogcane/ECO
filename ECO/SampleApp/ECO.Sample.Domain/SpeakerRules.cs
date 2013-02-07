using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Bender;

namespace ECO.Sample.Domain
{
    public static class SpeakerRules
    {
        public static OperationResult CheckSpeakerName(this OperationResult result, string name)
        {
            return ValueChecker<string>.For(name, result, "Name").Required("NAME_REQUIRED").StringLength(50, "NAME_TOO_LONG");
        }

        public static OperationResult CheckSpeakerSurname(this OperationResult result, string surname)
        {
            return ValueChecker<string>.For(surname, result, "Surname").Required("SURNAME_REQUIRED").StringLength(50, "SURNAME_TOO_LONG");
        }

        public static OperationResult CheckSpeakerDescription(this OperationResult result, string description)
        {
            return ValueChecker<string>.For(description, result, "Description").StringLength(1000, "DESCRIPTION_TOO_LONG");
        }

        public static OperationResult CheckSpeakerAge(this OperationResult result, int age)
        {
            return ValueChecker<int>.For(age, result, "Age").GreaterThen(18, "AGE_MUST_BE_OVER18");
        }
    }
}
