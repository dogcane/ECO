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
    public static class DeleteSpeaker
    {
        public record Command(Guid SpeakerCode) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IDataContext _DataContext;

            private ISpeakerRepository _SpeakerRepository;

            private readonly ILogger<DeleteSpeaker.Handler> _Logger;

            public Handler(IDataContext dataContext, ISpeakerRepository speakerRepository, ILogger<DeleteSpeaker.Handler> logger)
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
                    _SpeakerRepository.Remove(speakerEntity);
                    _DataContext.SaveChanges();
                    transactionContext.Commit();
                    return await Task.FromResult(OperationResult.MakeSuccess());
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
