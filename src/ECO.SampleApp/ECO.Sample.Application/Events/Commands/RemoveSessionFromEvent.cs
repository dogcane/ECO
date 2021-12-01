using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.Commands
{
    public static class RemoveSessionFromEvent
    {
        public record Command(Guid EventCode, Guid SessionCode) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private IEventRepository _EventRepository;

            public Handler(IEventRepository eventRepository) => _EventRepository = eventRepository;

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                Event eventEntity = _EventRepository.Load(request.EventCode);
                if (eventEntity == null)
                {
                    return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Event", "EVENT_NOT_FOUND")));
                }
                Session sessionEntity = eventEntity.Sessions.FirstOrDefault(s => s.Identity == request.SessionCode);
                if (sessionEntity == null)
                {
                    return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Session", "SESSION_NOT_FOUND")));
                }
                var sessionResult = eventEntity.RemoveSession(sessionEntity);
                if (sessionResult.Success)
                {
                    _EventRepository.Update(eventEntity);
                }
                return await Task.FromResult(
                    sessionResult.Success ?
                        OperationResult.MakeSuccess() :
                        OperationResult.MakeFailure(sessionResult.Errors)
                );
            }
        }
    }
}
