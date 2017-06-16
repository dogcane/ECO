using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Cfg;
using NHibernate.Envers;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Configuration.Fluent;
using System.IO;


namespace ECO.Providers.NHibernate.Envers
{
    public class NHEnversPersistenceUnit : NHPersistenceUnit
    {
        #region Consts

        protected static readonly string ENVERSCONFIGFILE_ATTRIBUTE = "enversConfigFile";

        #endregion

        #region Protected_Fields

        protected string _EvenrsConfigPath;

        #endregion

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            if (extendedAttributes.ContainsKey(ENVERSCONFIGFILE_ATTRIBUTE))
            {
                _EvenrsConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, extendedAttributes[ENVERSCONFIGFILE_ATTRIBUTE]);
            }
            else
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", ENVERSCONFIGFILE_ATTRIBUTE));
            }
        }

        protected override global::NHibernate.Cfg.Configuration BuildConfiguration()
        {
            var cfg = base.BuildConfiguration();
            //*******************
            //Envers Setup
            //*******************
            var xmlConfig = XDocument.Load(_EvenrsConfigPath);
            var enversCfg = new FluentConfiguration();            
            foreach(var classElement in xmlConfig.Document.Descendants("class"))
            {
                var auditConfig = enversCfg.Audit(Type.GetType(classElement.Attribute("type").Value));
                foreach (var excludeElement in classElement.Descendants("exclude"))
                {
                    switch (excludeElement.Attribute("type").Value)
                    {
                        case "relation":
                            auditConfig.ExcludeRelationData(excludeElement.Attribute("name").Value);
                            break;
                        case "property":
                            auditConfig.Exclude(excludeElement.Attribute("name").Value);
                            break;
                    }
                }
            }
            if (xmlConfig.Document.Root.Attribute("catalogName") != null)
            {
                cfg.SetEnversProperty(ConfigurationKey.DefaultCatalog, xmlConfig.Document.Root.Attribute("catalogName").Value);
            }
            if (xmlConfig.Document.Root.Attribute("schemaName") != null)
            {
                cfg.SetEnversProperty(ConfigurationKey.DefaultSchema, xmlConfig.Document.Root.Attribute("schemaName").Value);
            }
            if (xmlConfig.Document.Root.Attribute("tablePrefix") != null)
            {
                cfg.SetEnversProperty(ConfigurationKey.AuditTablePrefix, xmlConfig.Document.Root.Attribute("tablePrefix").Value);
            }
            if (xmlConfig.Document.Root.Attribute("tableSuffix") != null)
            {
                cfg.SetEnversProperty(ConfigurationKey.AuditTableSuffix, xmlConfig.Document.Root.Attribute("tableSuffix").Value);
            }
            cfg.IntegrateWithEnvers(enversCfg);
            if (xmlConfig.Document.Root.Attribute("createTable") != null && "true".Equals(xmlConfig.Document.Root.Attribute("createTable").Value, StringComparison.CurrentCultureIgnoreCase))
            {
                cfg.DataBaseIntegration(x => x.SchemaAction = SchemaAutoAction.Update);
            }
            //*******************
            return cfg;

        }
    }
}
