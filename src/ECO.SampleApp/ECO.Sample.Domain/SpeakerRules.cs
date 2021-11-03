using Resulz;
using Resulz.Validation;

namespace ECO.Sample.Domain
{
    public static class SpeakerRules
    {
        public static OperationResult CheckSpeakerName(this OperationResult result, string name)
        {
            return new ValueChecker<string>(name, result, "Name").Required("NAME_REQUIRED").StringLength(50, "NAME_TOO_LONG");
        }

        public static OperationResult CheckSpeakerSurname(this OperationResult result, string surname)
        {
            return new ValueChecker<string>(surname, result, "Surname").Required("SURNAME_REQUIRED").StringLength(50, "SURNAME_TOO_LONG");
        }

        public static OperationResult CheckSpeakerDescription(this OperationResult result, string description)
        {
            return new ValueChecker<string>(description, result, "Description").StringLength(1000, "DESCRIPTION_TOO_LONG");
        }

        public static OperationResult CheckSpeakerAge(this OperationResult result, int age)
        {
            return new ValueChecker<int>(age, result, "Age").GreaterThen(18, "AGE_MUST_BE_OVER18");
        }
    }
}
