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
                using var transactionContext = await _DataContext.BeginTransactionAsync();
                try
                {
                    var speakerResult = Speaker.Create(request.Name, request.Surname, request.Description, request.Age);
                    if (!speakerResult.Success)
                    {
                        return OperationResult<Guid>.MakeFailure(speakerResult.Errors);
                    }
                    await _SpeakerRepository.AddAsync(speakerResult.Value!);
                    await _DataContext.SaveChangesAsync();                    
                    await transactionContext.CommitAsync();
                    return OperationResult<Guid>.MakeSuccess(speakerResult.Value!.Identity);
                }
                catch (Exception ex)
                {
                    _Logger.LogError("Error during the execution of the handler : {0}", ex);
                    return OperationResult.MakeFailure(ErrorMessage.Create("Handle", ex.Message));
                }
                
            }
        }
    }
}
