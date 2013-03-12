using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ECO.Bender;
using ECO.Web.MVC;

using ECO.Sample.Application.Speakers;
using ECO.Sample.Application.Speakers.DTO;

namespace ECO.Sample.Presentation.Areas.Speakers.Controllers
{
    public class SpeakerApiController : ApiController
    {
        private IShowSpeakersService _ShowSpeakersService;

        private ICreateSpeakerService _CreateSpeakerService;

        private IChangeSpeakerService _ChangeSpeakerService;

        private IDeleteSpeakerService _DeleteSpeakerService;

        public SpeakerApiController(
            IShowSpeakersService showSpeakersService,
            ICreateSpeakerService createSpeakerService,
            IChangeSpeakerService changeSpeakerService,
            IDeleteSpeakerService deleteSpeakerService
            )
        {
            _ShowSpeakersService = showSpeakersService;
            _CreateSpeakerService = createSpeakerService;
            _ChangeSpeakerService = changeSpeakerService;
            _DeleteSpeakerService = deleteSpeakerService;
        }

        // GET speakers/api
        [DataContextApiFilter]
        public IQueryable<SpeakerListItem> Get([FromBody]string nameOrSurname)
        {
            return _ShowSpeakersService.ShowSpeakers(nameOrSurname);
        }

        // POST speakers/api
        [DataContextApiFilter]
        public OperationResult<Guid> Post([FromBody]SpeakerDetail speaker)
        {
            return _CreateSpeakerService.CreateNewSpeaker(speaker);
        }

        // PUT speakers/api/{guid}
        [DataContextApiFilter]
        public OperationResult Post(Guid id, [FromBody]SpeakerDetail speaker)
        {
            speaker.SpeakerCode = id;
            return _ChangeSpeakerService.ChangeInformation(speaker);
        }

        // DELETE speakers/api/{guid}
        [DataContextApiFilter]
        public OperationResult Delete(Guid id)
        {
            return _DeleteSpeakerService.DeleteSpeaker(id);
        }
    }
}
