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
                using var transactionContext = _DataContext.BeginTransaction();
                try
                {
                    Event @event = _EventRepository.Load(request.EventCode);
                    if (@event != null)
                    {
                        return await Task.FromResult(OperationResult<EventDetail>.MakeSuccess(_Mapper.Map<EventDetail>(@event)));
                    }
                    else
                    {
                        return await Task.FromResult(OperationResult<EventDetail>.MakeFailure());
                    }
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
