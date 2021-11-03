using ECO.Sample.Application.Speakers.DTO;
using ECO.Sample.Domain;
using Resulz;
using System;
using System.Linq;

namespace ECO.Sample.Application.Speakers.Impl
{
    public class GetSpeakerService : IGetSpeakerService
    {
        #region Fields

        private ISpeakerRepository _SpeakerRepository;

        #endregion

        #region Ctor

        public GetSpeakerService(ISpeakerRepository speakerRepository)
        {
            _SpeakerRepository = speakerRepository;
        }

        #endregion

        #region Public_Methods

        public OperationResult<SpeakerDetail> GetSpeaker(Guid speakerCode)
        {
            Speaker speaker = _SpeakerRepository.Load(speakerCode);
            if (speaker != null)
            {
                return new SpeakerDetail
                {
                    SpeakerCode = speaker.Identity,
                    Name = speaker.Name,
                    Surname = speaker.Surname,
                    Age = speaker.Age,
                    Description = speaker.Description
                };
            }
            else
            {
                return OperationResult<SpeakerDetail>.MakeFailure(Enumerable.Empty<ErrorMessage>());
            }
        }

        #endregion
    }
}
