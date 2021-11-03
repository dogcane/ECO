using ECO.Sample.Application.Speakers.DTO;
using Resulz;

namespace ECO.Sample.Application.Speakers
{
    public interface IChangeSpeakerService
    {
        OperationResult ChangeInformation(SpeakerDetail speaker);
    }
}
