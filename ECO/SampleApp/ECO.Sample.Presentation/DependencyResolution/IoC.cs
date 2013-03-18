// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using StructureMap;
using System.Web.Mvc;

using ECO.Sample.Domain;
using ECO.Sample.Infrastructure.Repositories;
using ECO.Sample.Application.Events;
using ECO.Sample.Application.Events.Impl;
using ECO.Sample.Application.Speakers;
using ECO.Sample.Application.Speakers.Impl;

namespace ECO.Sample.Presentation
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
#if INMEMORY
                x.For<IEventRepository>().Add<EventMemoryRepository>(); //InMemory Stuff
                x.For<ISpeakerRepository>().Add<SpeakerMemoryRepository>(); //InMemory Stuff
#elif NHIBERNATE    
                x.For<IEventRepository>().Add<EventNHRepository>(); //NHibernate Stuff
                x.For<ISpeakerRepository>().Add<SpeakerNHRepository>(); //NHibernate Stuff
#endif
                x.For<IShowEventsService>().Add<ShowEventsService>();
                x.For<ICreateEventService>().Add<CreateEventService>();
                x.For<IShowEventDetailService>().Add<ShowEventDetailService>();
                x.For<IChangeEventService>().Add<ChangeEventService>();
                x.For<IDeleteEventService>().Add<DeleteEventService>();
                x.For<IAddSessionToEventService>().Add<AddSessionToEventService>();
                x.For<IRemoveSessionFromEventService>().Add<RemoveSessionFromEventService>();
                x.For<IShowSpeakersService>().Add<ShowSpeakersService>();
                x.For<IGetSpeakerService>().Add<GetSpeakersService>();
                x.For<ICreateSpeakerService>().Add<CreateSpeakerService>();                
                x.For<IChangeSpeakerService>().Add<ChangeSpeakerService>();
                x.For<IDeleteSpeakerService>().Add<DeleteSpeakerService>();
            });
            return ObjectFactory.Container;
        }
    }
}