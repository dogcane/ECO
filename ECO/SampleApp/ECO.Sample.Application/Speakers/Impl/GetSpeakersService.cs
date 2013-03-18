using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Bender;
using ECO.Linq;

using ECO.Sample.Domain;
using ECO.Sample.Application.Speakers.DTO;

namespace ECO.Sample.Application.Speakers.Impl
{
    public class GetSpeakersService : IGetSpeakerService
    {
        #region Fields

        private ISpeakerRepository _SpeakerRepository;

        #endregion

        #region Ctor

        public GetSpeakersService(ISpeakerRepository speakerRepository)
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
                return SpeakerDetail.From(speaker);
            }
            else
            {
                return OperationResult<SpeakerDetail>.MakeFailure(Enumerable.Empty<ErrorMessage>());
            }
        }

        #endregion
    }
}
