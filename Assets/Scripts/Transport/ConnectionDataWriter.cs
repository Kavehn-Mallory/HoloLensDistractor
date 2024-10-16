using System;
using DistractorProject.Core;
using Unity.Collections;

namespace DistractorProject.Transport
{
    public struct ConnectionDataWriter
    {

        private DataStreamWriter _writer;
        
        public ConnectionDataWriter(ref DataStreamWriter writer)
        {
            _writer = writer;
        }
        
        public void SendMessage(ref DataStreamWriter writer, in ISerializer serializableData)
        {
            var index = GetDataTypeIndex(serializableData.GetType());
            writer.WriteByte(index);
            serializableData.Serialize(ref writer);
        }

        private static byte GetDataTypeIndex(Type type)
        {
            return DataSerializationIndexer.GetTypeIndex(type);
        }
    }
}