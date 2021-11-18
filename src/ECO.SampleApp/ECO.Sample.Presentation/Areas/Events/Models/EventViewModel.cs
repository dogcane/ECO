using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

using AutoMapper;

using ECO.Sample.Domain;

namespace ECO.Sample.Presentation.Areas.Events.Models
{
    public class EventViewModel
    {
        public static readonly EventViewModel Empty = new EventViewModel();

        public Guid EventCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IEnumerable<SessionItemViewModel> Sessions { get; set; }
    }
}
