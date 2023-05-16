using System;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using Marten;

namespace ECO.Providers.Marten.Utils
{
    public class NonPublicContractResolver : DefaultJsonTypeInfoResolver
    {
        public string FieldNamingStragegy { get; init; } = "_{0}";

        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);
            if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
            {
                //Default Non Public Ctor
                if (jsonTypeInfo.CreateObject is null && jsonTypeInfo.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length == 0)
                {
                    // The type doesn't have public constructors
                    jsonTypeInfo.CreateObject = () =>
                    {
                        return Activator.CreateInstance(jsonTypeInfo.Type, true)!;
                    };                    
                }

                //All Properties + Field;
                jsonTypeInfo.Properties.Clear();
                foreach (PropertyInfo propertyInfo in jsonTypeInfo.Type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    JsonPropertyInfo jPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(propertyInfo.PropertyType, propertyInfo.Name);
                    if (propertyInfo.CanRead)
                    {
                        jPropertyInfo.Get = obj => propertyInfo.GetValue(obj);
                    }
                    else
                    {

                    }                    
                    if (propertyInfo.CanWrite)
                    {
                        jPropertyInfo.Set = (obj, value) => propertyInfo.SetValue(obj, value);
                    }
                    else
                    {
                        FieldInfo? fieldInfo = jsonTypeInfo.Type.GetField(string.Format(FieldNamingStragegy, propertyInfo.Name), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (fieldInfo != null)
                        {
                            jPropertyInfo.Set = (obj, value) => fieldInfo.SetValue(obj, value);
                        }
                    }
                    jsonTypeInfo.Properties.Add(jPropertyInfo);
                }
            }
            return jsonTypeInfo;
        }
    }
}
