using AutoMapper;
using ECO.Sample.Application.Speakers.DTO;
using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Speakers.Queries
{
    public static class SearchSpeakers
    {
        public record Query(string NameOrSurname) : IRequest<OperationResult<IEnumerable<SpeakerItem>>>;

        public class Handler : IRequestHandler<Query, OperationResult<IEnumerable<SpeakerItem>>>
        {
            private ISpeakerRepository _SpeakerRepository;

            private IMapper _Mapper;

            public Handler(ISpeakerRepository speakerRepository, IMapper mapper)
            {
                _SpeakerRepository = speakerRepository;
                _Mapper = mapper;
            }

            public async Task<OperationResult<IEnumerable<SpeakerItem>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _SpeakerRepository.AsQueryable();
                if (!string.IsNullOrEmpty(request.NameOrSurname))
                {
                    query = query.Where(entity => entity.Name.Contains(request.NameOrSurname) || entity.Surname.Contains(request.NameOrSurname));
                }
                var speakers = _Mapper.ProjectTo<SpeakerItem>(query);
                return await Task.FromResult(OperationResult<IEnumerable<SpeakerItem>>.MakeSuccess(speakers));
            }
        }
    }
}
