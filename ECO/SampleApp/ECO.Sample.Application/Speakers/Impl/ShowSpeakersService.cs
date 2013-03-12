using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Linq;

using ECO.Sample.Domain;
using ECO.Sample.Application.Speakers.DTO;

namespace ECO.Sample.Application.Speakers.Impl
{
    public class ShowSpeakersService : IShowSpeakersService
    {
        #region Fields

        private ISpeakerRepository _SpeakerRepository;

        #endregion

        #region Ctor

        public ShowSpeakersService(ISpeakerRepository speakerRepository)
        {
            _SpeakerRepository = speakerRepository;
        }

        #endregion

        #region Public_Methods

        public IQueryable<SpeakerListItem> ShowSpeakers(string nameOrSurname)
        {
            var query = _SpeakerRepository.AsQueryable();
            if (!string.IsNullOrEmpty(nameOrSurname))
            {
                query = query.Where(entity => entity.Name.Contains(nameOrSurname) || entity.Surname.Contains(nameOrSurname));
            }
            return query.Select(item => SpeakerListItem.From(item));
        }

        #endregion
    }
}
