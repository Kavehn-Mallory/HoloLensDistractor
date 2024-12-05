using DistractorProject.Transport;

namespace DistractorProject.UserStudy
{
    public class ServerSideUserStudyManager : UserStudyManager
    {
        public override INetworkManager Manager => Server.Instance;

        private void Awake()
        {
            SecondsToWait = 3f;
        }
    }
}