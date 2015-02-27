using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using SharedRef.Domain;
using System.Xml.Linq;

namespace SharedRef.Extensions.ListOfSimpleItems
{
    /// <summary>
    /// Расширения для более простого обхода дерева объектов
    /// </summary>
    public static class ListOfSimpleItems
    {
        public static string ToXml(this List<SimpleItem> list, string name)
        {
            var root = new XElement("dictionary", new XAttribute("name", name));

            var items = new XElement("items");
            foreach (var item in list)
            {
                if (item == null)
                    continue;

                items.Add(
                    new XElement("item", item.Values.Select(x => new XAttribute(x.Key, x.Value ?? ""))));

            }


            root.Add(items);

            return root.ToString();
        }

        public static string ToJson(this List<SimpleItem> list, string name)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                writer.WritePropertyName("name");
                writer.WriteValue(name);

                writer.WritePropertyName("items");
                writer.WriteStartArray();

                foreach (var item in list)
                {
                    if (item == null)
                        continue;

                    writer.WriteStartObject();

                    foreach (var val in item.Values)
                    {
                        writer.WritePropertyName(val.Key);
                        writer.WriteValue(val.Value);
                    }

                    writer.WriteEndObject();
                }

                writer.WriteEnd();

                writer.WriteEndObject();
            }

            return sb.ToString();
        }

        private static void WriteParameter(JsonWriter writer, string name, string value)
        {
            if (string.IsNullOrEmpty(value) == false)
            {
                writer.WritePropertyName(name);
                writer.WriteValue(value);
            }
        }
    }
}
