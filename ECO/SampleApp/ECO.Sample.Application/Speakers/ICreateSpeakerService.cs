using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Bender;

namespace ECO.Sample.Application.Speakers
{
    public interface ICreateSpeakerService
    {
        OperationResult CreateNewSpeaker(string name, string surname, int age, string description);
    }
}
