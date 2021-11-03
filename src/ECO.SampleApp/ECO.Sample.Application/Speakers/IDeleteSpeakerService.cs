using Resulz;
using System;

namespace ECO.Sample.Application.Speakers
{
    public interface IDeleteSpeakerService
    {
        OperationResult DeleteSpeaker(Guid speakerCode);
    }
}
