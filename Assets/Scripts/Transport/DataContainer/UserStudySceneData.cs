using System;
using System.Collections.Generic;
using DistractorProject.Core;
using Unity.Collections;

namespace DistractorProject.Transport.DataContainer
{
    [Serializable]
    public struct UserStudySceneData : ISerializer
    {

        public List<SceneGrouping> groupings;
        
        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteInt(groupings.Count);

            foreach (var group in groupings)
            {
                writer.WriteInt(group.sceneGroupId);
                writer.WriteInt(group.markerId);
            }
        }

        public void Deserialize(ref DataStreamReader dataStreamReader)
        {
            groupings = new List<SceneGrouping>();

            var length = dataStreamReader.ReadInt();

            for (int i = 0; i < length; i++)
            {
                var sceneIndex = dataStreamReader.ReadInt();
                var markerIndex = dataStreamReader.ReadInt();
                groupings.Add(new SceneGrouping
                {
                    sceneGroupId = sceneIndex,
                    markerId = markerIndex
                });
            }
        }
    }

    [Serializable]
    public struct SceneGrouping
    {
        public int sceneGroupId;
        public int markerId;
    }
}