using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using UnityEngine;

namespace DistractorProject.UserStudy
{
    public class UserStudyManager : MonoBehaviour
    {
        
        [SerializeField] private MarkerPointSetup markerPointSetup;
        
        public void DoMarkerSetup()
        {
            markerPointSetup.StartMarkerPointSetup();
            //todo register progression of the markerPointSystem 
            Server.Instance.SendNetworkMessage(new MarkerCountData
            {
                markerCount = markerPointSetup.MarkerPointCount
            });
            
        }

        public void DoRandomOrderThingAndSendToClient()
        {
            
        }

        public void PerformStudyWithSceneLoading()
        {
            
        }

        public void EndStudy()
        {
            
        }
    }
}