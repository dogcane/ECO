using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Integrations.MassTransit
{
    public static class ECOMassTransitConfigurator
    {
        public static bool RemoveSagaAfterCompleted = true;
    }

    public static class ECOMassTransitConfiguratorExtensions
    {
        /// <summary>
        /// Specify if you want to remove a saga after is completed
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="value"></param>
        public static void RemoveSagaAfterCompleted(this IBusFactoryConfigurator configurator, bool value = true)
        {
            ECOMassTransitConfigurator.RemoveSagaAfterCompleted = value;
        }
    }    
}
