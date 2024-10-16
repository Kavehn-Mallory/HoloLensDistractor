using System;
using DistractorProject.Core;
using Unity.Collections;

namespace DistractorProject.Transport
{
    public struct ConnectionDataReader
    {
        private DataStreamReader _reader;
        
        public ConnectionDataReader(ref DataStreamReader reader)
        {
            _reader = reader;
        }
        
        
        public void OnMessageReceived(ref DataStreamReader reader)
        {
            Type dataType = GetDataType(reader.ReadByte());
            
            
        }

        public T GetDataAsSerializableType<T>() where T : ISerializer, new()
        {
            var element = new T();
            element.Deserialize(ref _reader);
            return element;
        }

        private static Type GetDataType(byte typeIndex)
        {
            return DataSerializationIndexer.GetTypeForTypeIndex(typeIndex);
        }
    }
}