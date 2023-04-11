using AutoMapper;
using ECO.Data;
using ECO.Sample.Application.Events.DTO;
using ECO.Sample.Application.Speakers.DTO;
using ECO.Sample.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Resulz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Speakers.Queries
{
    public static class SearchSpeakers
    {
        public record Query(string NameOrSurname) : IRequest<OperationResult<IEnumerable<SpeakerItem>>>;

        public class Handler : IRequestHandler<Query, OperationResult<IEnumerable<SpeakerItem>>>
        {
            private readonly IDataContext _DataContext;

            private ISpeakerRepository _SpeakerRepository;

            private IMapper _Mapper;

            private readonly ILogger<SearchSpeakers.Handler> _Logger;

            public Handler(IDataContext dataContext, ISpeakerRepository speakerRepository, IMapper mapper, ILogger<SearchSpeakers.Handler> logger)
            {
                _DataContext = dataContext;
                _SpeakerRepository = speakerRepository;
                _Mapper = mapper;
                _Logger = logger;
            }

            public async Task<OperationResult<IEnumerable<SpeakerItem>>> Handle(Query request, CancellationToken cancellationToken)
            {
                using var transactionContext = await _DataContext.BeginTransactionAsync();
                try
                {
                    var query = _SpeakerRepository.AsQueryable();
                    if (!string.IsNullOrEmpty(request.NameOrSurname))
                    {
                        query = query.Where(entity => entity.Name.Contains(request.NameOrSurname) || entity.Surname.Contains(request.NameOrSurname));
                    }
#if !MONGODB && !MARTEN && !INMEMORY && !EFMEMORY
                    var speakers = _Mapper.ProjectTo<SpeakerItem>(query);
#else
                    var speakers = _Mapper.Map<IEnumerable<SpeakerItem>>(query);
#endif                    
                    return await Task.FromResult(OperationResult<IEnumerable<SpeakerItem>>.MakeSuccess(speakers));
                }
                catch (Exception ex)
                {
                    _Logger.LogError("Error during the execution of the handler : {0}", ex);
                    return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Handle", ex.Message)));
                }
            }
        }
    }
}
