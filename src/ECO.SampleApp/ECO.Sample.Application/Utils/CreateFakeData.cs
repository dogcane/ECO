using ECO.Sample.Domain;
using MediatR;
using Resulz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Sample.Application.Utils
{
    public static class CreateFakeData
    {
        public record Command() : IRequest<OperationResult>;

        public class Handler : IRequestHandler<Command, OperationResult>
        {
            private IEventRepository _EventRepository;

            private ISpeakerRepository _SpeakerRepository;

            public Handler(IEventRepository eventRepository, ISpeakerRepository speakerRepository)
            {
                _EventRepository = eventRepository;
                _SpeakerRepository = speakerRepository;
            }

            public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                //Speakers
                var speaker01 = Speaker.Create("John", "Snow", ".Net & .Net Core Expert", 35).Value;
                var speaker02 = Speaker.Create("Arya", "Stark", "FrontEnd superhero", 20).Value;
                await _SpeakerRepository.AddAsync(speaker01);
                await _SpeakerRepository.AddAsync(speaker02);
                //Events
                var event01 = Event.Create(".Net Core Days", "Full immersion in .Net Core and on...", new Period(DateTime.Today, DateTime.Today.AddDays(2))).Value;
                event01.AddSession("AspNet Core", "AspNet Core full immersion", 200, speaker01);
                event01.AddSession("Blazor", "Blazor full immersion", 300, speaker01);
                event01.AddSession("Bootstrap", "Bootstrap full immersion", 100, speaker02);
                await _EventRepository.AddAsync(event01);

                return await Task.FromResult(OperationResult.MakeSuccess());
            }
        }
    }
}
