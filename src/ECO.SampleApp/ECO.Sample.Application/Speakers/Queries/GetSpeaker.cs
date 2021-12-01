using ECO.Sample.Application.Speakers.DTO;
using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Speakers.Queries
{
    public static class GetSpeaker
    {
        public record Query(Guid SpeakerCode) : IRequest<OperationResult<SpeakerDetail>>;

        public class Handler : IRequestHandler<Query, OperationResult<SpeakerDetail>>
        {
            private ISpeakerRepository _SpeakerRepository;

            public Handler(ISpeakerRepository speakerRepository) => _SpeakerRepository = speakerRepository;

            public async Task<OperationResult<SpeakerDetail>> Handle(Query request, CancellationToken cancellationToken)
            {
                Speaker speaker = _SpeakerRepository.Load(request.SpeakerCode);
                if (speaker != null)
                {
                    return await Task.FromResult(
                        OperationResult<SpeakerDetail>.MakeSuccess(
                            new SpeakerDetail(speaker.Identity, speaker.Name, speaker.Surname, speaker.Age, speaker.Description)
                    ));
                }
                else
                {
                    return await Task.FromResult(OperationResult<SpeakerDetail>.MakeFailure());
                }
            }
        }
    }
}
