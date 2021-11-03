using ECO.Sample.Application.Speakers.DTO;
using Resulz;
using System;

namespace ECO.Sample.Application.Speakers
{
    public interface IGetSpeakerService
    {
        OperationResult<SpeakerDetail> GetSpeaker(Guid speakerCode);
    }
}
