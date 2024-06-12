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
    public static class DeleteEvent
    {
        public record Command(Guid EventCode) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IDataContext _DataContext;

            private readonly IEventRepository _EventRepository;

            private readonly ILogger<DeleteEvent.Handler> _Logger;

            public Handler(IDataContext dataContext, IEventRepository eventRepository, ILogger<DeleteEvent.Handler> logger)
            {
                _DataContext = dataContext;
                _EventRepository = eventRepository;
                _Logger = logger;
            }

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                using var transactionContext = await _DataContext.BeginTransactionAsync();
                try
                {
                    var eventEventity = _EventRepository.Load(request.EventCode);
                    if (eventEventity == null)
                    {
                        return OperationResult.MakeFailure(ErrorMessage.Create("Event", "EVENT_NOT_FOUND"));
                    }
                    await _EventRepository.RemoveAsync(eventEventity);
                    await _DataContext.SaveChangesAsync();
                    await transactionContext.CommitAsync();
                    return OperationResult.MakeSuccess();
                }
                catch (Exception ex)
                {
                    _Logger.LogError("Error during the execution of the handler : {0}", ex);
                    return OperationResult.MakeFailure(ErrorMessage.Create("Handle", ex.Message));
                }
            }
        }
    }
}
