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
    [RoutePrefix("speakers/api")]
    public class SpeakerApiController : ApiController
    {
        private IShowSpeakersService _ShowSpeakersService;

        private IGetSpeakerService _GetSpeakerService;

        private ICreateSpeakerService _CreateSpeakerService;

        private IChangeSpeakerService _ChangeSpeakerService;

        private IDeleteSpeakerService _DeleteSpeakerService;

        public SpeakerApiController(
            IShowSpeakersService showSpeakersService,
            IGetSpeakerService getSpeakerService,
            ICreateSpeakerService createSpeakerService,
            IChangeSpeakerService changeSpeakerService,
            IDeleteSpeakerService deleteSpeakerService
            )
        {
            _ShowSpeakersService = showSpeakersService;
            _GetSpeakerService = getSpeakerService;
            _CreateSpeakerService = createSpeakerService;
            _ChangeSpeakerService = changeSpeakerService;
            _DeleteSpeakerService = deleteSpeakerService;
        }

        [DataContextApiFilter]        
        [HttpGet]        
        [Route()]
        public IQueryable<SpeakerListItem> GetSpeakers(string nameOrSurname)
        {
            return _ShowSpeakersService.ShowSpeakers(nameOrSurname);
        }

        [DataContextApiFilter]
        [HttpGet]
        [Route("{id:guid}")]
        public OperationResult<SpeakerDetail> GetSpeakerById(Guid id)
        {
            var result = _GetSpeakerService.GetSpeaker(id);
            if (result.Success)
            {
                return result;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        [DataContextApiFilter]
        [HttpPost]
        [Route()]
        public OperationResult<Guid> CreateSpeaker([FromBody]SpeakerDetail speaker)
        {
            return _CreateSpeakerService.CreateNewSpeaker(speaker);
        }

        [DataContextApiFilter]
        [HttpPut]
        [Route()]
        public OperationResult UpdateSpeaker([FromBody]SpeakerDetail speaker)
        {
            var result = _GetSpeakerService.GetSpeaker(speaker.SpeakerCode);
            if (!result.Success)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return _ChangeSpeakerService.ChangeInformation(speaker);
        }

        [DataContextApiFilter]
        [HttpDelete]
        [Route("{id:guid}")]
        public OperationResult DeleteSpeaker(Guid id)
        {
            var result = _GetSpeakerService.GetSpeaker(id);
            if (!result.Success)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return _DeleteSpeakerService.DeleteSpeaker(id);
        }
    }
}
