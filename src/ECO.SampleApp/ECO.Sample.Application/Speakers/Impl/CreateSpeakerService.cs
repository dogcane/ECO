using ECO.Sample.Application.Speakers.DTO;
using ECO.Sample.Domain;
using Resulz;
using System;

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

        public OperationResult<Guid> CreateNewSpeaker(SpeakerDetail speaker)
        {
            var speakerResult = Speaker.Create(speaker.Name, speaker.Surname, speaker.Description, speaker.Age);
            if (speakerResult.Success)
            {
                _SpeakerRepository.Add(speakerResult.Value);
                return OperationResult<Guid>.MakeSuccess(speakerResult.Value.Identity);
            }
            else
            {
                return OperationResult<Guid>.MakeFailure(speakerResult.Errors);
            }
        }

        #endregion
    }
}
