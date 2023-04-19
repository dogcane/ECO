using Marten;
using Marten.Schema;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ECO.Providers.Marten
{
    public class ECODocumentPolicy : IDocumentPolicy
    {
        #region IDocumentPolicy Members

        public void Apply(DocumentMapping mapping)
        {

            var _idProperty = mapping.DocumentType.GetProperty("Identity", BindingFlags.Instance | BindingFlags.Public);
            if (_idProperty != null)
            {
                mapping.IdMember = _idProperty;
            }
        }

        #endregion
    }
}
