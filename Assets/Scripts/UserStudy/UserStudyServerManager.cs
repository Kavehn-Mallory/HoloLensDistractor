using System;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using UnityEngine;

namespace DistractorProject.UserStudy
{
    public class UserStudyServerManager : MonoBehaviour
    {
        
        [SerializeField] private MarkerPointSetup markerPointSetup;


        private void Awake()
        {
            FetchComponents();
        }

        private void Start()
        {
            Server.Instance.RegisterCallback<UserStudyBeginData>(UserStudyBegin);
        }

        private void UserStudyBegin(UserStudyBeginData data)
        {
            DoMarkerSetup();
        }

        public void DoMarkerSetup()
        {
            Server.Instance.UnregisterCallback<UserStudyBeginData>(UserStudyBegin);
            markerPointSetup.OnMarkerSetupComplete += DoRandomOrderThingAndSendToClient;
            markerPointSetup.StartMarkerPointSetup();
        }

        

        public void DoRandomOrderThingAndSendToClient()
        {
            markerPointSetup.OnMarkerSetupComplete -= DoRandomOrderThingAndSendToClient;
        }

        public void PerformStudyWithSceneLoading()
        {
            
        }

        public void EndStudy()
        {
            
        }

        private void FetchComponents()
        {
            FetchServerComponents();
            return;
        }

        private void FetchServerComponents()
        {
            
        }
    }
}