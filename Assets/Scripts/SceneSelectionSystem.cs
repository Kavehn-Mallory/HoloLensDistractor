using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using UnityEngine;

namespace DistractorProject
{
    /// <summary>
    /// Clientside setup for scene management 
    /// </summary>
    public class SceneSelectionSystem : MonoBehaviour
    {
        
        
        [ContextMenu("Change to low scene")]
        public void ChangeToLowScene()
        {
            var messageData = new SceneGroupChangeData
            {
                index = 1
            };
            if (Client.Instance.SendNetworkMessage(messageData))
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
            if (Client.Instance.SendNetworkMessage(messageData))
            {
                Debug.Log("Message sent successful");
                return;
            }
            Debug.Log("Message could not be sent");
            
        }
    }
}