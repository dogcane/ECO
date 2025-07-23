namespace ECO.Providers.Marten;

using System.Reflection;
using global::Marten;
using global::Marten.Schema;

public sealed class ECODocumentPolicy : IDocumentPolicy
{
    #region IDocumentPolicy Members

    public void Apply(DocumentMapping mapping)
    {
        var idProperty = mapping.DocumentType.GetProperty("Identity", BindingFlags.Instance | BindingFlags.Public);
        if (idProperty is not null)
        {
            mapping.IdMember = idProperty;
        }
    }

    #endregion
}
