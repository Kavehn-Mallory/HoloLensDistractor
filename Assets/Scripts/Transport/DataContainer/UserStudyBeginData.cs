﻿using System;
using DistractorProject.Core;
using Unity.Collections;

namespace DistractorProject.Transport.DataContainer
{
    [Serializable]
    public struct UserStudyBeginData : ISerializer
    {

        public int studyIndex;
        
        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteInt(studyIndex);
        }

        public void Deserialize(ref DataStreamReader reader)
        {
            studyIndex = reader.ReadInt();
        }
    }
}