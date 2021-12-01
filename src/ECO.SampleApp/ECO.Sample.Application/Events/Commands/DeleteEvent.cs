using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.Commands
{
    public static class DeleteEvent
    {
        public record Command(Guid EventCode) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private IEventRepository _EventRepository;

            public Handler(IEventRepository eventRepository) => _EventRepository = eventRepository;

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                Event eventEventity = _EventRepository.Load(request.EventCode);
                if (eventEventity == null)
                {
                    return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Event", "EVENT_NOT_FOUND")));
                }
                _EventRepository.Remove(eventEventity);
                return await Task.FromResult(OperationResult.MakeSuccess());
            }
        }
    }
}
