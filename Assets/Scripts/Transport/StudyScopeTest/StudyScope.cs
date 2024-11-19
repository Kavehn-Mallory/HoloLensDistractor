using UnityEngine;

namespace DistractorProject.Transport.StudyScopeTest
{


    public abstract class StudyScope : ScriptableObject, IStudyData
    {
        public abstract IStudyScopeReceiver AsReceiver(INetworkManager manager);

        public abstract IStudyScopeSender AsSender(INetworkManager manager);
    }
    
    [CreateAssetMenu(menuName = "StudyScope/MarkerPointSetup", fileName = "MarkerPointSetupData")]
    public class MarkerPointSetup : StudyScope
    {
        public override IStudyScopeReceiver AsReceiver(INetworkManager manager)
        {
            throw new System.NotImplementedException();
        }

        public override IStudyScopeSender AsSender(INetworkManager manager)
        {
            throw new System.NotImplementedException();
        }
        
    }
}