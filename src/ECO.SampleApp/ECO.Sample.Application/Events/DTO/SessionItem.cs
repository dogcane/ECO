using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.DTO
{
    public record SessionItem(Guid SessionCode, string Title, int Level, string Speaker);
}
