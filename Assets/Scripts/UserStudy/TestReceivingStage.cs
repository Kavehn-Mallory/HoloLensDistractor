using DistractorProject.Core;
using Unity.Collections;
using UnityEngine;

namespace DistractorProject.UserStudy
{
    public class TestReceivingStage : ReceivingStudyStageComponent<TestMessage>
    {
        protected override void OnStudyStageStart(TestMessage studyEvent)
        {
            Debug.Log("Starting stage");
        }

        protected override void OnStudyStageEnd(TestMessage studyEvent)
        {
            Debug.Log("Ending stage");
        }
    }

    public struct TestMessage : IStudyStageEvent
    {
        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteByte((byte)(IsStartEvent ? 1 : 0));
        }

        public void Deserialize(ref DataStreamReader dataStreamReader)
        {
            var value = dataStreamReader.ReadByte();
            IsStartEvent = value == 1;
        }

        public bool IsStartEvent { get; set; }
    }
}