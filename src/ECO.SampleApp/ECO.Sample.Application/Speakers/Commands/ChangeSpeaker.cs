using ECO.Data;
using ECO.Sample.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Resulz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Speakers.Commands
{
    public static class ChangeSpeaker
    {
        public record Command(Guid SpeakerCode, string Name, string Surname, string Description, int Age) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IDataContext _DataContext;

            private readonly ISpeakerRepository _SpeakerRepository;

            private readonly ILogger<ChangeSpeaker.Handler> _Logger;

            public Handler(IDataContext dataContext, ISpeakerRepository speakerRepository, ILogger<ChangeSpeaker.Handler> logger)
            {
                _DataContext = dataContext;
                _SpeakerRepository = speakerRepository;
                _Logger = logger;
            }

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                using var transactionContext = _DataContext.BeginTransaction();
                try
                {
                    Speaker speakerEntity = _SpeakerRepository.Load(request.SpeakerCode);
                    if (speakerEntity == null)
                    {
                        return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Speaker", "SPEAKER_NOT_FOUND")));
                    }
                    var speakerResult = speakerEntity.ChangeInformation(request.Name, request.Surname, request.Description, request.Age);
                    if (speakerResult.Success)
                    {
                        _SpeakerRepository.Update(speakerEntity);
                    }
                    _DataContext.SaveChanges();
                    transactionContext.Commit();
                    return await Task.FromResult(speakerResult);
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
