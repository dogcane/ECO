using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Sample.Domain;

namespace ECO.Sample.Presentation.Areas.Events.Models
{
    public class EvenItemViewModel
    {
        public Guid EventCode { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfSessions { get; set; }
    }
}
