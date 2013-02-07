using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO.Bender;

namespace ECO.Sample.Application.Speakers
{
    public interface IChangeSpeakerService
    {
        OperationResult ChangeInformation(Guid speakerCode, string name, string surname, int age, string description);
    }
}
