using LabApi.Features.Console;
using Mirror;
using System;
using System.Reflection;

namespace HolographicDisplays
{
    public static class MirrorWriterExtension
    {
        public static bool Write<T>(T value, NetworkWriterPooled networkWriter)
        {
            return Write(typeof(T), value, networkWriter);
        }

        public static bool Write(Type type, object value, NetworkWriterPooled networkWriter)
        {
            Type genericType = typeof(Writer<>).MakeGenericType(type);
            FieldInfo writeField = genericType.GetField("write", BindingFlags.Static | BindingFlags.Public);
            if (writeField == null)
            {
                Logger.Warn($"Tried to write type: {type} but has no NetworkWriter!");
                return false;
            }

            if (writeField.GetValue(null) is not Delegate del)
            {
                Logger.Warn($"Writer<{type}>.write is not a delegate!");
                return false;
            }
            del.DynamicInvoke(networkWriter, value);
            return true;
        }
    }
}
