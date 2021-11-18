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
    public static class ChangeEvent
    {
        public record Command(Guid EventCode, string Name, string Description, DateTime StartDate, DateTime EndDate) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private IEventRepository _EventRepository;

            public Handler(IEventRepository eventRepository) => _EventRepository = eventRepository;

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                Event eventEventity = _EventRepository.Load(request.EventCode);
                if (eventEventity == null) {
                    return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Event", "EVENT_NOT_FOUND")));
                }
                var eventResult = eventEventity.ChangeInformation(request.Name, request.Description, new Period(request.StartDate, request.EndDate));
                if (eventResult.Success)
                {
                    _EventRepository.Update(eventEventity);
                }
                return await Task.FromResult(
                    eventResult.Success ?
                        OperationResult.MakeSuccess() :
                        OperationResult.MakeFailure(eventResult.Errors).TranslateContext("Period.StartDate", "StartDate")
                );
            }
        }
    }
}
