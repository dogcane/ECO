using ECO.Data;
using ECO.Sample.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
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
            private readonly IDataContext _DataContext;

            private readonly IEventRepository _EventRepository;

            private readonly ISpeakerRepository _SpeakerRepository;

            private readonly ILogger<Handler> _Logger;

            public Handler(IDataContext dataContext, IEventRepository eventRepository, ISpeakerRepository speakerRepository, ILogger<AddSessionToEvent.Handler> logger)
            {
                _DataContext = dataContext;
                _EventRepository = eventRepository;
                _SpeakerRepository = speakerRepository;
                _Logger = logger;
            }

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                using var transactionContext = _DataContext.BeginTransaction();
                try
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
                    _DataContext.SaveChanges();
                    transactionContext.Commit();
                    return await Task.FromResult(sessionResult);
                }
                catch (Exception ex)
                {
                    _Logger.LogError("Error during the execution of the handler : {0}", ex);
                    return await Task.FromResult(OperationResult.MakeFailure().AppendError("Handle", ex.Message));
                }
            }
        }
    }
}
