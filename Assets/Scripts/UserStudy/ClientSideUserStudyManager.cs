using System.Collections;
using DistractorProject.Transport;
using PlasticPipe.PlasticProtocol.Client;

namespace DistractorProject.UserStudy
{
    public class ClientSideUserStudyManager : UserStudyManager
    {
        public override INetworkManager Manager => Client.Instance;


        protected override IEnumerator Start()
        {
            Client.Instance.Connect();
            return base.Start();
        }
    }
}