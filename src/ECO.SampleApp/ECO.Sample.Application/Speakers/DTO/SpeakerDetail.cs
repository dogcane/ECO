using System;

namespace ECO.Sample.Application.Speakers.DTO
{
    public record SpeakerDetail(Guid SpeakerCode, string Name, string Surname, int Age, string Description);
}
