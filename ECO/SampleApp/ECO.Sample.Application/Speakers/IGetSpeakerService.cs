using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

using ECO.Sample.Application.Speakers.DTO;

namespace ECO.Sample.Application.Speakers
{
    public interface IGetSpeakerService
    {
        OperationResult<SpeakerDetail> GetSpeaker(Guid speakerCode);
    }
}
