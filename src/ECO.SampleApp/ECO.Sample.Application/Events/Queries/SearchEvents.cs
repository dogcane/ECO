using AutoMapper;
using ECO.Data;
using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Resulz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.Queries
{
    public static class SearchEvents
    {
        public record Query(DateTime? FromDate, DateTime? ToDate, string EventName) : IRequest<OperationResult<IEnumerable<EventItem>>>;

        public class Handler : IRequestHandler<Query, OperationResult<IEnumerable<EventItem>>>
        {
            private readonly IDataContext _DataContext;

            private IEventRepository _EventRepository;

            private IMapper _Mapper;

            private readonly ILogger<SearchEvents.Handler> _Logger;

            public Handler(IDataContext dataContext, IEventRepository eventRepository, IMapper mapper, ILogger<SearchEvents.Handler> logger)
            {
                _DataContext = dataContext;
                _EventRepository = eventRepository;
                _Mapper = mapper;
                _Logger = logger;
            }

            public async Task<OperationResult<IEnumerable<EventItem>>> Handle(Query request, CancellationToken cancellationToken)
            {
                using var transactionContext = await _DataContext.BeginTransactionAsync();                
                try
                {
                    var query = _EventRepository.AsQueryable();
                    if (request.FromDate.HasValue)
                    {
                        query = query.Where(entity => entity.Period.StartDate >= request.FromDate.Value);
                    }
                    if (request.ToDate.HasValue)
                    {
                        query = query.Where(entity => entity.Period.EndDate <= request.ToDate.Value);
                    }
                    if (!string.IsNullOrEmpty(request.EventName))
                    {
                        query = query.Where(entity => entity.Name.Contains(request.EventName));
                    }
                    var events = _Mapper.ProjectTo<EventItem>(query);
                    return await Task.FromResult(OperationResult<IEnumerable<EventItem>>.MakeSuccess(events));
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
