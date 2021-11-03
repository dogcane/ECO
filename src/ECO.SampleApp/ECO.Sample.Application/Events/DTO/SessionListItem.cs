using System;

namespace ECO.Sample.Application.Events.DTO
{
    public class SessionListItem
    {
        public Guid SessionCode { get; set; }

        public string Title { get; set; }

        public int Level { get; set; }

        public string Speaker { get; set; }
    }
}
