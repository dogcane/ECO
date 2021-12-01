using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.Commands
{
    public static class AddSessionToEvent
    {
        public record Command(Guid EventCode, string Title, string Description, int Level, Guid SpeakerCode) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private IEventRepository _EventRepository;

            private ISpeakerRepository _SpeakerRepository;

            public Handler(IEventRepository eventRepository, ISpeakerRepository speakerRepository)
            {
                _EventRepository = eventRepository;
                _SpeakerRepository = speakerRepository;
            }

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                Event eventEntity = _EventRepository.Load(request.EventCode);
                if (eventEntity == null)
                {
                    return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Event", "EVENT_NOT_FOUND")));
                }
                Speaker speakerEntity = _SpeakerRepository.Load(request.SpeakerCode);
                if (speakerEntity == null)
                {
                    return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Speaker", "SPEAKER_NOT_FOUND")));
                }
                var sessionResult = eventEntity.AddSession(request.Title, request.Description, request.Level, speakerEntity);
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
