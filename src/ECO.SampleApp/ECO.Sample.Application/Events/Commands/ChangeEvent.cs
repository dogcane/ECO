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
    public static class ChangeEvent
    {
        public record Command(Guid EventCode, string Name, string Description, DateTime StartDate, DateTime EndDate) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IDataContext _DataContext;

            private readonly IEventRepository _EventRepository;

            private readonly ILogger<ChangeEvent.Handler> _Logger;

            public Handler(IDataContext dataContext, IEventRepository eventRepository, ILogger<ChangeEvent.Handler> logger)
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
                    var eventResult = eventEventity.ChangeInformation(request.Name, request.Description, new Period(request.StartDate, request.EndDate));
                    if (!eventResult.Success)
                    {
                        return eventResult.TranslateContext("Period.StartDate", "StartDate").TranslateContext("Period.EndDate", "EndDate");                        
                    }
                    await _EventRepository.UpdateAsync(eventEventity);
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