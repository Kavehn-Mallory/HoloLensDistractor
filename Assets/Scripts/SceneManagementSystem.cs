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
        private SceneReference sceneReference;

        [SerializeField]
        private SceneLoader sceneLoader;
        private void OnEnable()
        {
            Server.Instance.OnDataStreamReceived += OnDataStreamReceived;
            sceneLoader = FindObjectOfType<SceneLoader>();
        }

        private void OnDisable()
        {
            Server.Instance.OnDataStreamReceived -= OnDataStreamReceived;
        }

        private void OnDataStreamReceived(DataStreamReader reader)
        {
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
            else if (type == typeof(ConfirmationData))
            {
                var confirmationData = new ConfirmationData();
                confirmationData.Deserialize(ref reader);
                //todo add functionality
            }
        }
    }
}