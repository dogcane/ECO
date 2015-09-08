﻿using System;
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
