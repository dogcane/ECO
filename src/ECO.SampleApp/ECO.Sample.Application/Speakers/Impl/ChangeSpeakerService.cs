
using ECO.Sample.Application.Speakers.DTO;
using ECO.Sample.Domain;
using Resulz;

namespace ECO.Sample.Application.Speakers.Impl
{
    public class ChangeSpeakerService : IChangeSpeakerService
    {
        #region Fields

        private ISpeakerRepository _SpeakerRepository;

        #endregion

        #region Ctor

        public ChangeSpeakerService(ISpeakerRepository speakerRepository)
        {
            _SpeakerRepository = speakerRepository;
        }

        #endregion

        #region Public_Methods

        public OperationResult ChangeInformation(SpeakerDetail speaker)
        {
            Speaker speakerEntity = _SpeakerRepository.Load(speaker.SpeakerCode);
            var result = speakerEntity.ChangeInformation(speaker.Name, speaker.Surname, speaker.Description, speaker.Age);
            if (result.Success)
            {
                _SpeakerRepository.Update(speakerEntity);
            }
            return result;
        }

        #endregion
    }
}
