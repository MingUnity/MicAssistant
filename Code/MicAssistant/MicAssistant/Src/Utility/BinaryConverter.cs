using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MicAssistant
{
    public static class BinaryConverter
    {
        public static byte[] Serialize(object data)
        {
            byte[] buffer = null;

            if (data != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, data);

                    stream.Seek(0, SeekOrigin.Begin);

                    buffer = stream.GetBuffer();
                }
            }

            return buffer;
        }

        public static T Deserialize<T>(byte[] buffer) where T : class
        {
            T t = null;

            if (buffer != null)
            {
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    t = formatter.Deserialize(stream) as T;
                }
            }

            return t;
        }
    }
}
