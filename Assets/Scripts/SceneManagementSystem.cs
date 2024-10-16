using Tymski;
using Unity.Collections;
using Unity.Networking.Transport.Samples;
using UnityEngine;

namespace DistractorProject
{
    public class SceneManagementSystem : MonoBehaviour
    {

        
        
        [SerializeField]
        private ServerBehaviour serverBehaviour;
        
        [SerializeField]
        private SceneReference[] sceneReferences;

        


        private void OnEnable()
        {
            serverBehaviour.OnDataStreamReceived += OnDataStreamReceived;
        }

        private void OnDisable()
        {
            serverBehaviour.OnDataStreamReceived -= OnDataStreamReceived;
        }

        private void OnDataStreamReceived(DataStreamReader obj)
        {
            Debug.Log("Scene managment informed");
        }
    }
}