using System;
using System.Collections;
using System.Collections.Generic;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DistractorProject.UserStudy
{
    [Obsolete]
    public class UserStudyClientManager : MonoBehaviour
    {
        //todo register to client script. maybe even initiate the connection from here
        //connect distractor placement to client -> maybe create a chain of scripts, that wait for the previous one to finish 

        [SerializeField]
        private DistractorPlacement distractorPlacement;

        private List<SceneGrouping> _groupings;

#if UNITY_EDITOR
        private IEnumerator Start()
        {
            //todo fix the in editor part
            yield return new WaitForSeconds(1f);
            Client.Instance.RegisterCallback<MarkerCountData>(distractorPlacement.OnMarkerCountDataReceived);
            Client.Instance.RegisterCallback<MarkerSetupEndData>(OnSetupCompleted);
            Client.Instance.RegisterCallback<UserStudySceneData>(OnStudySceneDataReceived);
            Client.Instance.Connect();
            yield return new WaitForSeconds(5f);
            Debug.Log("Sending UserStudyBegin message");
            Client.Instance.TransmitNetworkMessage(new UserStudyBeginData());

        }

        private void OnStudySceneDataReceived(UserStudySceneData obj)
        {
            Debug.Log("Receiving scene data");
            _groupings = obj.groupings;
            Client.Instance.TransmitNetworkMessage(new SceneGroupChangeData
            {
                index = obj.groupings[Random.Range(0, obj.groupings.Count)].sceneGroupId
            });
        }

        private void OnSetupCompleted(MarkerSetupEndData obj)
        {
            //now expect the random setup thing, I think 
        }
#else
        private void Start()
        {
            throw new NotImplementedException();
        }

#endif
    }
}