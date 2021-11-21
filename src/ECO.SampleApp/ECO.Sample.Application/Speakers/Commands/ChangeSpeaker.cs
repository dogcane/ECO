using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Speakers.Commands
{
    public static class ChangeSpeaker
    {
        public record Command(Guid SpeakerCode, string Name, string Surname, string Description, int Age) : IRequest<OperationResult<Guid>>;

        public class Handler : IRequestHandler<Command, OperationResult<Guid>>
        {
            private ISpeakerRepository _SpeakerRepository;

            public Handler(ISpeakerRepository speakerRepository) => _SpeakerRepository = speakerRepository;

            public async Task<OperationResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                Speaker speakerEntity = _SpeakerRepository.Load(request.SpeakerCode);
                if (speakerEntity == null)
                {
                    return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Speaker", "SPEAKER_NOT_FOUND")));
                }
                var eventResult = speakerEntity.ChangeInformation(request.Name, request.Surname, request.Description, request.Age);
                if (eventResult.Success)
                {
                    _SpeakerRepository.Update(speakerEntity);
                }
                return await Task.FromResult(
                    eventResult.Success ?
                        OperationResult.MakeSuccess() :
                        OperationResult.MakeFailure(eventResult.Errors)
                );
            }
        }
    }
}
