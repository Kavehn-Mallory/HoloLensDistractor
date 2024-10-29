using System;
using DistractorProject.Core;
using Unity.Collections;

namespace DistractorProject.Transport.DataContainer
{
    [Serializable]
    public struct ConfirmationData : ISerializer
    {

        public int confirmationNumber;
        
        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteInt(confirmationNumber);
        }

        public void Deserialize(ref DataStreamReader dataStreamReader)
        {
            confirmationNumber = dataStreamReader.ReadInt();
        }
    }
}