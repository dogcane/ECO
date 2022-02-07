using ECO.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Providers.InMemory.Configuration
{
    public static class InMemoryDataContextOptionsExtensions
    {
        public static DataContextOptions UsingInMemory(this DataContextOptions dataContextOptions, Action<InMemoryOptions> optionsAction, IConfiguration configuration)
        {
            InMemoryOptions options = new InMemoryOptions { };
            optionsAction(options);
            //TODO
            return dataContextOptions;
        }
    }

    public class InMemoryOptions
    {
        public string Name { get; set; }
    }
}
