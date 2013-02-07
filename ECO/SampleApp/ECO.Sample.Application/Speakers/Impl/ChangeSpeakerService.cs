using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO.Bender;

using ECO.Sample.Domain;

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

        public OperationResult ChangeInformation(Guid speakerCode, string name, string surname, int age, string description)
        {
            Speaker speaker = _SpeakerRepository.Load(speakerCode);
            return speaker.ChangeInformation(name, surname, description, age);
        }

        #endregion
    }
}
