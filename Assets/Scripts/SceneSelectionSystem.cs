using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using Tymski;
using UnityEngine;

namespace DistractorProject
{
    /// <summary>
    /// Clientside setup for scene management 
    /// </summary>
    [RequireComponent(typeof(Client))]
    public class SceneSelectionSystem : MonoBehaviour
    {
        
        private Client _clientBehaviour;
        [SerializeField]
        private SceneReference sceneReference;

        private void Awake()
        {
            _clientBehaviour = GetComponent<Client>();
            //idea: maybe have a scriptable object that manages all the data, then we only need to send data that points to a certain scene setup? 
        }

        [ContextMenu("Send scene change")]
        public void SendSceneChange()
        {
            var messageData = new SceneChangeData
            {
                sceneReference = sceneReference
            };
            if (_clientBehaviour.SendNetworkMessage(messageData))
            {
                Debug.Log("Message sent successful");
                return;
            }
            Debug.Log("Message could not be sent");
        }
    }
}