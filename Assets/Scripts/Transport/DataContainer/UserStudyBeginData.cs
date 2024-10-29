using System;
using DistractorProject.Core;
using Unity.Collections;

namespace DistractorProject.Transport.DataContainer
{
    [Serializable]
    public struct UserStudyBeginData : ISerializer
    {
        public void Serialize(ref DataStreamWriter writer)
        {
        }

        public void Deserialize(ref DataStreamReader dataStreamReader)
        {
        }
    }
}