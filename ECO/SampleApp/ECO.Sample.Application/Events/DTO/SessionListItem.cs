﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Sample.Domain;

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
