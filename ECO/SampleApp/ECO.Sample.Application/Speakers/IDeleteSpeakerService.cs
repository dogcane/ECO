using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO.Bender;

namespace ECO.Sample.Application.Speakers
{
    public interface IDeleteSpeakerService
    {
        OperationResult DeleteSpeaker(Guid speakerCode);
    }
}
