using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Sample.Domain;

namespace ECO.Sample.Presentation.Areas.Events.Models
{
    public class SessionItemViewModel
    {
        public Guid SessionCode { get; set; }

        public string Title { get; set; }

        public int Level { get; set; }

        public string Speaker { get; set; }
    }
}
