using Resulz;
using Resulz.Validation;
using System;

namespace ECO.Sample.Domain
{
    public class Speaker : AggregateRoot<Guid>
    {
        #region Properties

        public virtual string Name { get; protected set; }

        public virtual string Surname { get; protected set; }

        public virtual string Description { get; protected set; }

        public virtual int Age { get; protected set; }

        #endregion

        #region Ctor

        protected Speaker()
            : base(Guid.NewGuid())
        {

        }

        #endregion

        #region Factory_Methods

        public static OperationResult<Speaker> Create(string name, string surname, string description, int age)
        {
            var result = OperationResult
                .MakeSuccess()
                .With(name, nameof(Name)).Required("NAME_REQUIRED").StringLength(50, "NAME_TOO_LONG")
                .With(surname, nameof(Surname)).Required("SURNAME_REQUIRED").StringLength(50, "SURNAME_TOO_LONG")
                .With(description, nameof(Description)).StringLength(1000, "DESCRIPTION_TOO_LONG")
                .With(age, nameof(Age)).GreaterThen(18, "AGE_MUST_BE_OVER18")
                .Result;
            if (result.Success)
            {
                return new Speaker() { Name = name, Surname = surname, Description = description, Age = age };
            }
            return result;
        }

        #endregion

        #region Public_Methods

        public virtual OperationResult ChangeInformation(string name, string surname, string description, int age)
        {
            var result = OperationResult
                .MakeSuccess()
                .With(name, nameof(Name)).Required("NAME_REQUIRED").StringLength(50, "NAME_TOO_LONG")
                .With(surname, nameof(Surname)).Required("SURNAME_REQUIRED").StringLength(50, "SURNAME_TOO_LONG")
                .With(description, nameof(Description)).StringLength(1000, "DESCRIPTION_TOO_LONG")
                .With(age, nameof(Age)).GreaterThen(18, "AGE_MUST_BE_OVER18")
                .Result;
            if (result.Success)
            {
                Name = name;
                Surname = surname;
                Description = description;
                Age = age;
            }
            return result;
        }

        #endregion
    }
}
