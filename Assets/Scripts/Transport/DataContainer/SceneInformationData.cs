using System;
using DistractorProject.Core;
using Unity.Collections;

namespace DistractorProject.Transport.DataContainer
{
    [Serializable]
    public struct SceneInformationData : ISerializer
    {
        //todo fill out the data
        
        
        
        public void Serialize(ref DataStreamWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(ref DataStreamReader dataStreamReader)
        {
            throw new NotImplementedException();
        }
    }
}