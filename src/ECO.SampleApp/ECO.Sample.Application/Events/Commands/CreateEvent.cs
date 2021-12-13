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
    public static class CreateEvent
    {
        public record Command(string Name, string Description, DateTime StartDate, DateTime EndDate) : IRequest<OperationResult<Guid>>;

        public class Handler : IRequestHandler<Command, OperationResult<Guid>>
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

            public async Task<OperationResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                using var transactionContext = _DataContext.BeginTransaction();
                try
                {
                    var eventResult = Event.Create(request.Name, request.Description, new Period(request.StartDate, request.EndDate));
                    if (eventResult.Success)
                    {
                        _EventRepository.Add(eventResult.Value);
                    }
                    _DataContext.SaveChanges();
                    transactionContext.Commit();
                    return await Task.FromResult(
                        eventResult.Success ?
                            OperationResult<Guid>.MakeSuccess(eventResult.Value.Identity) :
                            OperationResult<Guid>.MakeFailure(eventResult.Errors).TranslateContext("Period.StartDate", "StartDate")
                    );
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
