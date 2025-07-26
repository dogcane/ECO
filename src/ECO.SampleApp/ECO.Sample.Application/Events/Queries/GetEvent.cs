using AutoMapper;
using ECO.Data;
using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Resulz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.Queries
{
    public static class GetEvent
    {
        public record Query(Guid EventCode) : IRequest<OperationResult<EventDetail>>;

        public class Handler : IRequestHandler<Query, OperationResult<EventDetail>>
        {
            private readonly IDataContext _DataContext;

            private IEventRepository _EventRepository;

            private IMapper _Mapper;

            private readonly ILogger<GetEvent.Handler> _Logger;

            public Handler(IDataContext dataContext, IEventRepository eventRepository, IMapper mapper, ILogger<GetEvent.Handler> logger)
            {
                _DataContext = dataContext;
                _EventRepository = eventRepository;
                _Mapper = mapper;
                _Logger = logger;
            }

            public async Task<OperationResult<EventDetail>> Handle(Query request, CancellationToken cancellationToken)
            {
                using var transactionContext = await _DataContext.BeginTransactionAsync(cancellationToken);
                try
                {
                    var eventEntity = await _EventRepository.LoadAsync(request.EventCode);
                    if (eventEntity == null)
                    {
                        return OperationResult.MakeFailure(ErrorMessage.Create("Event", "EVENT_NOT_FOUND"));
                    }
                    return OperationResult<EventDetail>.MakeSuccess(_Mapper.Map<EventDetail>(eventEntity));
                }
                catch (Exception ex)
                {
                    _Logger.LogError(ex, "Error during the execution of the handler");
                    return OperationResult.MakeFailure(ErrorMessage.Create("Handle", ex.Message));
                }
            }
        }
    }
}
