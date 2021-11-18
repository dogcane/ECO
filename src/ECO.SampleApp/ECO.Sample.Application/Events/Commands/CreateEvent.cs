using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.Commands
{
    public static class CreateEvent
    {
        public record Command(string Name, string Description, DateTime StartDate, DateTime EndDate) : IRequest<OperationResult<Guid>>;

        public class Handler : IRequestHandler<Command, OperationResult<Guid>>
        {
            private IEventRepository _EventRepository;

            public Handler(IEventRepository eventRepository) => _EventRepository = eventRepository;

            public async Task<OperationResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var eventResult = Event.Create(request.Name, request.Description, new Period(request.StartDate, request.EndDate));
                if (eventResult.Success)
                {
                    _EventRepository.Add(eventResult.Value);
                }
                return await Task.FromResult(
                    eventResult.Success ?
                        OperationResult<Guid>.MakeSuccess(eventResult.Value.Identity) :
                        OperationResult<Guid>.MakeFailure(eventResult.Errors).TranslateContext("Period.StartDate", "StartDate")
                );
            }
        }
    }
}
