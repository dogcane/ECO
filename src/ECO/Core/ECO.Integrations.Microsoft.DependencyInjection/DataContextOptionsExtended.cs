using ECO.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Integrations.Microsoft.DependencyInjection
{
    public sealed class DataContextOptionsExtended : DataContextOptions
    {
        public IServiceCollection Services { get; private set; }

        public DataContextOptionsExtended(IServiceCollection services) : base()
        {
            Services = services;
        }
    }
}
