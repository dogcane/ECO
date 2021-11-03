using ECO.Sample.Application.Speakers.DTO;
using Resulz;
using System;

namespace ECO.Sample.Application.Speakers
{
    public interface ICreateSpeakerService
    {
        OperationResult<Guid> CreateNewSpeaker(SpeakerDetail speaker);
    }
}
