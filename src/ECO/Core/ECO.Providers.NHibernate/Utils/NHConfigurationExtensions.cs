namespace ECO.Providers.NHibernate.Utils;

using System;
using System.IO;
using System.Reflection;
using ECO.Providers.NHibernate.Configuration;
using Nh = global::NHibernate;
using NhCfg = global::NHibernate.Cfg;

public static class NHConfigurationExtensions
{
    /// <summary>
    /// Adds a mapping assembly to the NHibernate configuration (.hbm.xml and ModelMappers).
    /// </summary>
    /// <param name="configuration">The NHibernate configuration.</param>
    /// <param name="assemblyName">The name of the assembly containing the mappings.</param>
    public static NhCfg.Configuration AddAssemblyExtended(this NhCfg.Configuration configuration, string assemblyName)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(assemblyName);
        var assembly = Assembly.Load(assemblyName);
        return configuration.AddAssemblyExtended(assembly);
    }

    /// <summary>
    /// Adds a mapping assembly to the NHibernate configuration (.hbm.xml and ModelMappers).
    /// </summary>
    /// <param name="options">The NHibernate options.</param>
    /// <param name="assembly">The assembly containing the mappings.</param>
    public static NhCfg.Configuration AddAssemblyExtended(this NhCfg.Configuration configuration, System.Reflection.Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(assembly);

        configuration.AddAssembly(assembly);
        var mapper = new Nh.Mapping.ByCode.ModelMapper();
        mapper.AddMappings(assembly.ExportedTypes);
        NhCfg.MappingSchema.HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        configuration.AddMapping(domainMapping);
        return configuration;
    }
}
