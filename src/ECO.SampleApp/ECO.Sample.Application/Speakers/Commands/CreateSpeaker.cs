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
    public static class CreateSpeaker
    {
        public record Command(string Name, string Surname, string Description, int Age) : IRequest<OperationResult<Guid>>;

        public class Handler : IRequestHandler<Command, OperationResult<Guid>>
        {
            private readonly IDataContext _DataContext;

            private ISpeakerRepository _SpeakerRepository;

            private readonly ILogger<CreateSpeaker.Handler> _Logger;

            public Handler(IDataContext dataContext, ISpeakerRepository speakerRepository, ILogger<CreateSpeaker.Handler> logger)
            {
                _DataContext = dataContext;
                _SpeakerRepository = speakerRepository;
                _Logger = logger;
            }

            public async Task<OperationResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                using var transactionContext = _DataContext.BeginTransaction();
                try
                {
                    var speakerResult = Speaker.Create(request.Name, request.Surname, request.Description, request.Age);
                    if (speakerResult.Success)
                    {
                        _SpeakerRepository.Add(speakerResult.Value);
                    }                    
                    _DataContext.SaveChanges();
                    transactionContext.Commit();
                    return await Task.FromResult(
                        speakerResult.Success ?
                            OperationResult<Guid>.MakeSuccess(speakerResult.Value.Identity) :
                            OperationResult<Guid>.MakeFailure(speakerResult.Errors)
                    );
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
