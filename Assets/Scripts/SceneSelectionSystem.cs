using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using Eflatun.SceneReference;
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

        private void Awake()
        {
            _clientBehaviour = GetComponent<Client>();
            //idea: maybe have a scriptable object that manages all the data, then we only need to send data that points to a certain scene setup? 
        }
        
        [ContextMenu("Change to low scene")]
        public void ChangeToLowScene()
        {
            var messageData = new SceneGroupChangeData
            {
                index = 1
            };
            if (_clientBehaviour.SendNetworkMessage(messageData))
            {
                Debug.Log("Message sent successful");
                return;
            }
            Debug.Log("Message could not be sent");
            
        }
        
        [ContextMenu("Change to high scene")]
        public void ChangeToHighScene()
        {
            var messageData = new SceneGroupChangeData
            {
                index = 2
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