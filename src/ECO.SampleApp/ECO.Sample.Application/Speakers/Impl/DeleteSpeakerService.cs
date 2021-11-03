using ECO.Sample.Domain;
using Resulz;
using System;

namespace ECO.Sample.Application.Speakers.Impl
{
    public class DeleteSpeakerService : IDeleteSpeakerService
    {
        #region Fields

        private ISpeakerRepository _SpeakerRepository;

        #endregion

        #region Ctor

        public DeleteSpeakerService(ISpeakerRepository speakerRepository)
        {
            _SpeakerRepository = speakerRepository;
        }

        #endregion

        #region Public_Methods

        public OperationResult DeleteSpeaker(Guid speakerCode)
        {
            Speaker speaker = _SpeakerRepository.Load(speakerCode);
            _SpeakerRepository.Remove(speaker);
            return OperationResult.MakeSuccess();
        }

        #endregion
    }
}
