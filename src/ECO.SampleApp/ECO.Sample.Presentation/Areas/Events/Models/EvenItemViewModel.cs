using System;

namespace ECO.Sample.Presentation.Areas.Events.Models
{
    public class EventItemViewModel
    {
        public Guid EventCode { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfSessions { get; set; }
    }
}
