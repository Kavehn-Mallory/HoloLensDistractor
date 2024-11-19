using DistractorProject.Core;
using DistractorProject.Transport;

namespace DistractorProject.UserStudy
{
    public class SendingStudyStageComponent<TStudyEvent> : SendingStudyStageComponent where TStudyEvent : unmanaged, IStudyStageEvent
    {


        public override void StartStudy(INetworkManager manager)
        {
            manager.TransmitNetworkMessage(new TStudyEvent
            {
                IsStartEvent = true
            });
        }

        public override void EndStudy(INetworkManager manager)
        {
            manager.TransmitNetworkMessage(new TStudyEvent
            {
                IsStartEvent = false
            });
        }
    }
    
    public abstract class SendingStudyStageComponent : StudyStageComponent
    {
        public abstract void StartStudy(INetworkManager manager);

        public abstract void EndStudy(INetworkManager manager);
    }
}