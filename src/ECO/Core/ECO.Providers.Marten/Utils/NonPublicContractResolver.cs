namespace ECO.Providers.Marten.Utils;

using System;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;

public class NonPublicContractResolver : DefaultJsonTypeInfoResolver
{
    public bool UseParameterlessCtor { get; init; } = true;

    public bool UseFields { get; init; } = true;

    public string FieldNamingStragegy { get; init; } = "_{0}";

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);
        if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
        {
            if (UseParameterlessCtor)
            {
                //Default Parameterless Ctor
                var ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, []);
                if (ctor != null)
                {
                    jsonTypeInfo.CreateObject = () => ctor.Invoke(null);
                }
            }

            //Try properties/field public/non public resolver
            foreach (var prop in jsonTypeInfo.Properties)
            {
                if (prop.Set == null)
                {
                    var propertyInfo = jsonTypeInfo.Type.GetProperty(prop.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        prop.Set = (obj, value) => propertyInfo.SetValue(obj, value);
                    }
                    else if (UseFields)
                    {
                        var fieldInfo = jsonTypeInfo.Type.GetField(string.Format(FieldNamingStragegy, prop.Name), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (fieldInfo != null)
                        {
                            prop.Set = (obj, value) => fieldInfo.SetValue(obj, value);
                        }
                    }
                }
            }
        }
        return jsonTypeInfo;
    }
}
