using System;
using System.Collections.Generic;
using DistractorProject.SceneManagement;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using UnityEngine;

namespace DistractorProject.UserStudy
{
    [Obsolete]
    public class UserStudyServerManager : MonoBehaviour
    {
        
        [SerializeField] private MarkerPointSetup markerPointSetup;
        [SerializeField] private SceneLoader sceneLoader;

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
            var studySceneIndices = sceneLoader.FindUserStudyScenes();
            var markerPointCount = markerPointSetup.MarkerPointCount;

            var userStudyData = new UserStudySceneData
            {
                groupings = new List<SceneGrouping>()
            };
            for (int i = 0; i < markerPointCount; i++)
            {
                for (int j = 0; j < studySceneIndices.Length; j++)
                {
                    userStudyData.groupings.Add(new SceneGrouping
                    {
                        sceneGroupId = studySceneIndices[j],
                        markerId = i
                    });
                }
            }
            Debug.Log("Sending scene data");
            Server.Instance.TransmitNetworkMessage(userStudyData);
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