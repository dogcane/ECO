using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Speakers.Commands
{
    public static class CreateSpeaker
    {
        public record Command(string Name, string Surname, string Description, int Age) : IRequest<OperationResult<Guid>>;

        public class Handler : IRequestHandler<Command, OperationResult<Guid>>
        {
            private ISpeakerRepository _SpeakerRepository;

            public Handler(ISpeakerRepository speakerRepository) => _SpeakerRepository = speakerRepository;

            public async Task<OperationResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var speakerResult = Speaker.Create(request.Name, request.Surname, request.Description, request.Age);
                if (speakerResult.Success)
                {
                    _SpeakerRepository.Add(speakerResult.Value);
                }
                return await Task.FromResult(
                    speakerResult.Success ?
                        OperationResult<Guid>.MakeSuccess(speakerResult.Value.Identity) :
                        OperationResult<Guid>.MakeFailure(speakerResult.Errors)
                );
            }
        }
    }
}
