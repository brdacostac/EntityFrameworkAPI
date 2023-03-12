
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.ComponentModel;

namespace DTOLol.Factory
{
    public class JsonConverterMessage : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var objectType = value.GetType();
            var contract = new JsonObjectContract(objectType);

            foreach (var propertyInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!propertyInfo.CanRead || !propertyInfo.GetGetMethod().IsPublic)
                {
                    continue;
                }

                var property = CreateProperty(propertyInfo, DefaultValueHandling.Ignore);
                if (property != null)
                {
                    contract.Properties.Add((JsonProperty)property);
                }
            }

            writer.WriteStartObject();

            foreach (var property in contract.Properties)
            {
                var propertyValue = property.ValueProvider.GetValue(value);
                if (propertyValue != null)
                {
                    writer.WritePropertyName(property.PropertyName);
                    serializer.Serialize(writer, propertyValue);
                }
            }

            writer.WriteEndObject();
        }

        private object CreateProperty(System.Reflection.PropertyInfo propertyInfo, DefaultValueHandling ignore)
        {
            // Ignore the property if it has a [JsonIgnore] attribute.
            if (propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>() != null)
            {
                return null;
            }

            var property = new JsonProperty
            {
                PropertyName = propertyInfo.Name,
                PropertyType = propertyInfo.PropertyType,
                Readable = true,
                Writable = false // Only deserialize from JSON, don't serialize to JSON.
            };

            // If the property has a [DefaultValue] attribute and the DefaultValueHandling is not Ignore,
            // set the DefaultValueHandling property on the JsonProperty.
            var defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultValueAttribute != null && ignore != DefaultValueHandling.Ignore)
            {
                property.DefaultValueHandling = ignore;
            }

            return property;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
