using System;
using DistractorProject.Core;
using Unity.Collections;

namespace DistractorProject.Transport.DataContainer
{
    [Serializable]
    public struct MarkerCountData : ISerializer
    {
        //todo maybe replace this with just an int?
        public int markerCount;
        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteInt(markerCount);
        }

        public void Deserialize(ref DataStreamReader reader)
        {
            markerCount = reader.ReadInt();
        }
    }
}