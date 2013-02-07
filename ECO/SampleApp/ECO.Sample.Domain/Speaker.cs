using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Bender;

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
            return OperationResult
                .Begin()
                .CheckSpeakerName(name)
                .CheckSpeakerSurname(surname)
                .CheckSpeakerDescription(description)
                .CheckSpeakerAge(age)
                .IfSuccess<Speaker>(() =>
                {
                    return new Speaker() { Name = name, Surname = surname, Description = description, Age = age };
                });
        }

        #endregion

        #region Public_Methods

        public virtual OperationResult ChangeInformation(string name, string surname, string description, int age)
        {
            return OperationResult
                .Begin()
                .CheckSpeakerName(name)
                .CheckSpeakerSurname(surname)
                .CheckSpeakerDescription(description)
                .CheckSpeakerAge(age)
                .IfSuccess(() =>
                {
                    Name = name;
                    Surname = surname;
                    Description = description;
                    Age = age;
                });
        }

        #endregion
    }
}
