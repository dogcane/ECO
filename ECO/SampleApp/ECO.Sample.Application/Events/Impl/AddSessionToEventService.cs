using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;
using ECO.Sample.Domain;

namespace ECO.Sample.Application.Events.Impl
{
    public class AddSessionToEventService : IAddSessionToEventService
    {
        #region Private_Fields

        private IEventRepository _EventRepository;

        private ISpeakerRepository _SpeakerRepository;

        #endregion

        #region Ctor

        public AddSessionToEventService(IEventRepository eventRepository, ISpeakerRepository speakerRepository)
        {
            _EventRepository = eventRepository;
            _SpeakerRepository = speakerRepository;
        }

        #endregion

        #region IAddSessionToEventService Membri di

        public OperationResult AddSession(Guid eventCode, string title, string description, int level, Guid speakerCode)
        {
            Event @event = _EventRepository.Load(eventCode);
            Speaker speaker = _SpeakerRepository.Load(speakerCode);
            return @event.AddSession(title, description, level, speaker);
        }

        #endregion
    }
}
