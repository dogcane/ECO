using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Speakers.Commands
{
    public static class DeleteSpeaker
    {
        public record Command(Guid SpeakerCode) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private ISpeakerRepository _SpeakerRepository;

            public Handler(ISpeakerRepository speakerRepository) => _SpeakerRepository = speakerRepository;

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                Speaker speakerEntity = _SpeakerRepository.Load(request.SpeakerCode);
                if (speakerEntity == null)
                {
                    return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Speaker", "SPEAKER_NOT_FOUND")));
                }
                _SpeakerRepository.Remove(speakerEntity);
                return await Task.FromResult(OperationResult.MakeSuccess());
            }
        }
    }
}
