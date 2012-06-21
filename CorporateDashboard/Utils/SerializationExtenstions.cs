using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CorporateDashboard.Utils
{
    public static class SerializationExtensions
    {
        public static string SerializeJson<T>(this T objectToSerialize)
        {
            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(T));

            serializer.WriteObject(stream, objectToSerialize);

            stream.Position = 0;
            var sr = new StreamReader(stream);

            return sr.ReadToEnd();
        }

        public static T DeserializeJson<T>(this string json)
        {
            Stream stream = new MemoryStream(Encoding.Default.GetBytes(json));
            var serializer = new DataContractJsonSerializer(typeof(T));
            var deserializedObject = (T)serializer.ReadObject((stream));

            return deserializedObject;
        }
    }
}