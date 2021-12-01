using System;

namespace ECO.Sample.Application.Events.DTO
{
    public record SessionItem(Guid SessionCode, string Title, int Level, string Speaker);
}
