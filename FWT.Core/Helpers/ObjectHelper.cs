using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FWTL.Core.Helpers
{
    public static class ObjectHelper
    {
        public static T FromByteArray<T>(byte[] data) where T : class
        {
            if (data == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                object obj = binaryFormatter.Deserialize(memoryStream);
                return (T)obj;
            }
        }

        public static byte[] ToByteArray<T>(T @object) where T : class
        {
            if (@object == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, @object);
                return memoryStream.ToArray();
            }
        }
    }
}
