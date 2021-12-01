using AutoMapper;
using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Domain;
using MediatR;
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
            private IEventRepository _EventRepository;

            private IMapper _Mapper;

            public Handler(IEventRepository eventRepository, IMapper mapper)
            {
                _EventRepository = eventRepository;
                _Mapper = mapper;
            }

            public async Task<OperationResult<EventDetail>> Handle(Query request, CancellationToken cancellationToken)
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
        }
    }
}
