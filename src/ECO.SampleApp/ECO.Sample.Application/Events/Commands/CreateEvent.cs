﻿using ECO.Data;
using ECO.Sample.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Resulz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.Commands
{
    public static class CreateEvent
    {
        public record Command(string Name, string Description, DateTime StartDate, DateTime EndDate) : IRequest<OperationResult<Guid>>;

        public class Handler : IRequestHandler<Command, OperationResult<Guid>>
        {
            private readonly IDataContext _DataContext;

            private readonly IEventRepository _EventRepository;

            private readonly ILogger<ChangeEvent.Handler> _Logger;

            public Handler(IDataContext dataContext, IEventRepository eventRepository, ILogger<ChangeEvent.Handler> logger)
            {
                _DataContext = dataContext;
                _EventRepository = eventRepository;
                _Logger = logger;
            }

            public async Task<OperationResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                using var transactionContext = await _DataContext.BeginTransactionAsync();
                try
                {
                    var eventResult = Event.Create(request.Name, request.Description, new Period(request.StartDate, request.EndDate));
                    if (!eventResult.Success)
                    {
                        return OperationResult<Guid>.MakeFailure(eventResult.TranslateContext("Period.StartDate", "StartDate").TranslateContext("Period.EndDate", "EndDate").Errors);
                    }
                    await _EventRepository.AddAsync(eventResult.Value!);
                    await _DataContext.SaveChangesAsync();
                    await transactionContext.CommitAsync();
                    return OperationResult<Guid>.MakeSuccess(eventResult.Value!.Identity);                    
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
