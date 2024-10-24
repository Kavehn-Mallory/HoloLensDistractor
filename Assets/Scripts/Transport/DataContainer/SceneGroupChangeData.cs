using System;
using DistractorProject.Core;
using Unity.Collections;

namespace DistractorProject.Transport.DataContainer
{
    [Serializable]
    public struct SceneGroupChangeData : ISerializer
    {

        public int index;
        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteInt(index);
        }

        public void Deserialize(ref DataStreamReader dataStreamReader)
        {
            index = dataStreamReader.ReadInt();
        }
    }
}