using System;
using UnityEngine;

namespace DistractorProject.UserStudy
{
    public abstract class StudyStageComponent : MonoBehaviour
    {
        public event Action OnStudyStart = delegate { };

        public event Action OnStudyEnd = delegate { };

        protected void TriggerStudyStartEvent() => OnStudyStart.Invoke();
        
        protected void TriggerStudyEndEvent() => OnStudyEnd.Invoke();
        
    }
}