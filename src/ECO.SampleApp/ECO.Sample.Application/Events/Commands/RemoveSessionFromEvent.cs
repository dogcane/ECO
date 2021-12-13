﻿using ECO.Data;
using ECO.Sample.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Resulz;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Events.Commands
{
    public static class RemoveSessionFromEvent
    {
        public record Command(Guid EventCode, Guid SessionCode) : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private readonly IDataContext _DataContext;

            private readonly IEventRepository _EventRepository;

            private readonly ILogger<RemoveSessionFromEvent.Handler> _Logger;

            public Handler(IDataContext dataContext, IEventRepository eventRepository, ILogger<RemoveSessionFromEvent.Handler> logger)
            {
                _DataContext = dataContext;
                _EventRepository = eventRepository;
                _Logger = logger;
            }

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                using var transactionContext = _DataContext.BeginTransaction();
                try
                {
                    Event eventEntity = _EventRepository.Load(request.EventCode);
                    if (eventEntity == null)
                    {
                        return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Event", "EVENT_NOT_FOUND")));
                    }
                    Session sessionEntity = eventEntity.Sessions.FirstOrDefault(s => s.Identity == request.SessionCode);
                    if (sessionEntity == null)
                    {
                        return await Task.FromResult(OperationResult.MakeFailure(ErrorMessage.Create("Session", "SESSION_NOT_FOUND")));
                    }
                    var sessionResult = eventEntity.RemoveSession(sessionEntity);
                    if (sessionResult.Success)
                    {
                        _EventRepository.Update(eventEntity);
                    }
                    _DataContext.SaveChanges();
                    transactionContext.Commit();
                    return await Task.FromResult(sessionResult);                    
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
