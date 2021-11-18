using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.Queries
{
    public static class GetEvent
    {
        public record Query(Guid EventCode) : IRequest<OperationResult<EventDetail>>;

        public class Handler : IRequestHandler<Query, OperationResult<EventDetail>>
        {
            private IEventRepository _EventRepository;

            public Handler(IEventRepository eventRepository) => _EventRepository = eventRepository;

            public async Task<OperationResult<EventDetail>> Handle(Query request, CancellationToken cancellationToken)
            {
                Event @event = _EventRepository.Load(request.EventCode);
                if (@event != null)
                {
                    return await Task.FromResult(
                        OperationResult<EventDetail>.MakeSuccess(
                            new EventDetail(
                                @event.Identity,
                                @event.Name,
                                @event.Description,
                                @event.Period.StartDate,
                                @event.Period.EndDate,
                                @event.Sessions.Select(item => new SessionItem(item.Identity, item.Title, item.Level, string.Concat(item.Speaker.Name, " ", item.Speaker.Surname)))
                    )));
                }
                else
                {
                    return await Task.FromResult(OperationResult<EventDetail>.MakeFailure());
                }
            }
        }
        
        

        
    }
}
