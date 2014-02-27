using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ECO.Data;

namespace ECO.Integrations.CastleWindsor
{
    public class ECOWindsorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For(typeof(IRepository<,>))
                .UsingFactoryMethod((IKernel kernel, CreationContext ctx) =>
                {                        
                        Type T = ctx.GenericArguments[0];
                        Type K = ctx.GenericArguments[1];                       
                        IPersistenceUnit persistentUnit = ECO.Data.PersistenceUnitFactory.Instance.GetPersistenceUnit(T);
                        Type P = persistentUnit.GetType();
                        MethodInfo buildRepositoryMethod = P.GetMethod("BuildRepository").MakeGenericMethod(T, K);                                            
                        return buildRepositoryMethod.Invoke(persistentUnit, null);
                }),
                Component.For(typeof(IReadOnlyRepository<,>))
                .UsingFactoryMethod((IKernel kernel, CreationContext ctx) =>
                {
                    Type T = ctx.GenericArguments[0];
                    Type K = ctx.GenericArguments[1];
                    IPersistenceUnit persistentUnit = ECO.Data.PersistenceUnitFactory.Instance.GetPersistenceUnit(T);
                    Type P = persistentUnit.GetType();
                    MethodInfo buildRepositoryMethod = P.GetMethod("BuildReadOnlyRepository").MakeGenericMethod(T, K);
                    return buildRepositoryMethod.Invoke(persistentUnit, null);
                })
            );
        }

        #endregion
    }
}
