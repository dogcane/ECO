using ECO.Sample.Application.Speakers.DTO;
using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            public Handler(ISpeakerRepository speakerRepository) => _SpeakerRepository = speakerRepository;

            public async Task<OperationResult<IEnumerable<SpeakerItem>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _SpeakerRepository.AsQueryable();
                if (!string.IsNullOrEmpty(request.NameOrSurname))
                {
                    query = query.Where(entity => entity.Name.Contains(request.NameOrSurname) || entity.Surname.Contains(request.NameOrSurname));
                }
                var Speakers = query.Select(item => new SpeakerItem(item.Identity, item.Name, item.Surname));
                return await Task.FromResult(OperationResult<IEnumerable<SpeakerItem>>.MakeSuccess(Speakers));
            }
        }
    }
}
