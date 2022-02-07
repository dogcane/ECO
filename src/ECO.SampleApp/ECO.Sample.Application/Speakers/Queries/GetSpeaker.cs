using AutoMapper;
using ECO.Data;
using ECO.Sample.Application.Speakers.DTO;
using ECO.Sample.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
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
            private readonly IDataContext _DataContext;

            private ISpeakerRepository _SpeakerRepository;

            private IMapper _Mapper;

            private readonly ILogger<GetSpeaker.Handler> _Logger;

            public Handler(IDataContext dataContext, ISpeakerRepository speakerRepository, IMapper mapper, ILogger<GetSpeaker.Handler> logger)
            {
                _DataContext = dataContext;
                _SpeakerRepository = speakerRepository;
                _Mapper = mapper;
                _Logger = logger;
            }

            public async Task<OperationResult<SpeakerDetail>> Handle(Query request, CancellationToken cancellationToken)
            {
                using var transactionContext = await _DataContext.BeginTransactionAsync();
                try
                {
                    Speaker speaker = _SpeakerRepository.Load(request.SpeakerCode);
                    if (speaker != null)
                    {
                        SpeakerDetail speakerDetail = _Mapper.Map<SpeakerDetail>(speaker);
                        return await Task.FromResult(OperationResult<SpeakerDetail>.MakeSuccess(speakerDetail));
                    }
                    else
                    {
                        return await Task.FromResult(OperationResult<SpeakerDetail>.MakeFailure());
                    }
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
