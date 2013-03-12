using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO.Bender;
using ECO.Sample.Application.Speakers.DTO;

namespace ECO.Sample.Application.Speakers
{
    public interface IChangeSpeakerService
    {
        OperationResult ChangeInformation(SpeakerDetail speaker);
    }
}
