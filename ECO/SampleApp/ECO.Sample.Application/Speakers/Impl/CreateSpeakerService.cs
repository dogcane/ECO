using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

using ECO.Sample.Domain;

namespace ECO.Sample.Application.Speakers.Impl
{
    public class CreateSpeakerService : ICreateSpeakerService
    {
        #region Fields

        private ISpeakerRepository _SpeakerRepository;

        #endregion

        #region Ctor

        public CreateSpeakerService(ISpeakerRepository speakerRepository)
        {
            _SpeakerRepository = speakerRepository;
        }

        #endregion

        #region Public_Methods

        public OperationResult CreateNewSpeaker(string name, string surname, int age, string description)
        {
            OperationResult<Speaker> speaker = Speaker.Create(name, surname, description, age);
            if (speaker.Success)
            {
                _SpeakerRepository.Add(speaker.Value);
            }
            return speaker;
        }

        #endregion
    }
}
