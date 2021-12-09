using AutoMapper;
using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Domain;
using MediatR;
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
            private IEventRepository _EventRepository;

            private IMapper _Mapper;

            public Handler(IEventRepository eventRepository, IMapper mapper)
            {
                _EventRepository = eventRepository;
                _Mapper = mapper;
            }

            public async Task<OperationResult<IEnumerable<EventItem>>> Handle(Query request, CancellationToken cancellationToken)
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
        }
    }
}
