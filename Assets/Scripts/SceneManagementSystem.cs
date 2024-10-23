using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using Tymski;
using Unity.Collections;
using UnityEngine;

namespace DistractorProject
{
    public class SceneManagementSystem : MonoBehaviour
    {

        
        
        [SerializeField]
        private Server serverBehaviour;
        
        [SerializeField]
        private SceneReference[] sceneReferences;

        [SerializeField]
        private SceneReference sceneReference;


        private void OnEnable()
        {
            serverBehaviour.OnDataStreamReceived += OnDataStreamReceived;
        }

        private void OnDisable()
        {
            serverBehaviour.OnDataStreamReceived -= OnDataStreamReceived;
        }

        private void OnDataStreamReceived(DataStreamReader reader)
        {
            Debug.Log("Scene management informed");
            var typeIndex = reader.ReadByte();
            var type = DataSerializationIndexer.GetTypeForTypeIndex(typeIndex);

            if (type == typeof(SceneChangeData))
            {
                var sceneRef = new SceneChangeData();
                sceneRef.Deserialize(ref reader);
                sceneReference = sceneRef.sceneReference;
            }
        }
    }
}