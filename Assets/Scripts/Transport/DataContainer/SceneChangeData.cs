using System;
using DistractorProject.Core;
using Tymski;
using Unity.Collections;
using UnityEngine;

namespace DistractorProject.Transport.DataContainer
{
    [Serializable]
    public struct SceneChangeData : ISerializer
    {

        public SceneReference sceneReference;
        
        public void Serialize(ref DataStreamWriter writer)
        {
            ConnectionDataWriter.WriteString(ref writer, sceneReference.ScenePath);
        }

        public void Deserialize(ref DataStreamReader dataStreamReader)
        {
            sceneReference = new SceneReference();
            sceneReference.ScenePath = ConnectionDataReader.ReadFixedString(ref dataStreamReader);
            
            Debug.Log(sceneReference.ScenePath);
        }
    }
}