using DistractorProject.SceneManagement;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using UnityEngine;

namespace DistractorProject
{
    public class SceneManagementSystem : MonoBehaviour
    {
        [SerializeField]
        private SceneLoader sceneLoader;
        private void OnEnable()
        {
            sceneLoader = FindObjectOfType<SceneLoader>();
            Server.Instance.RegisterCallback<SceneGroupChangeData>(OnSceneGroupChangeRequest);
        }

        private void OnDisable()
        {
            Server.Instance.UnregisterCallback<SceneGroupChangeData>(OnSceneGroupChangeRequest);
        }

        private void OnSceneGroupChangeRequest(SceneGroupChangeData data)
        {
            sceneLoader?.LoadSceneGroup(data.index);
        }
        
    }
}