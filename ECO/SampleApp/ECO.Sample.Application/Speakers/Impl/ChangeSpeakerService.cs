using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO.Bender;

using ECO.Sample.Domain;
using ECO.Sample.Application.Speakers.DTO;

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
            return speakerEntity.ChangeInformation(speaker.Name, speaker.Surname, speaker.Description, speaker.Age);
        }

        #endregion
    }
}
