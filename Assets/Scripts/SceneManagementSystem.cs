using DistractorProject.SceneManagement;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using Eflatun.SceneReference;
using Unity.Collections;
using UnityEngine;

namespace DistractorProject
{
    public class SceneManagementSystem : MonoBehaviour
    {
        [SerializeField]
        private Server serverBehaviour;
        
        [SerializeField]
        private SceneReference sceneReference;

        [SerializeField]
        private SceneLoader sceneLoader;
        private void OnEnable()
        {
            serverBehaviour.OnDataStreamReceived += OnDataStreamReceived;
            sceneLoader = FindObjectOfType<SceneLoader>();
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
            else if (type == typeof(SceneGroupChangeData))
            {
                var sceneGroup = new SceneGroupChangeData();
                sceneGroup.Deserialize(ref reader);
                sceneLoader?.LoadSceneGroup(sceneGroup.index);
            }
        }
    }
}