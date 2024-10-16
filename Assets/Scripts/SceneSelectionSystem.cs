using Unity.Networking.Transport.Samples;
using UnityEngine;

namespace DistractorProject
{
    /// <summary>
    /// Clientside setup for scene management 
    /// </summary>
    [RequireComponent(typeof(ClientBehaviour))]
    public class SceneSelectionSystem : MonoBehaviour
    {
        
        private ClientBehaviour _clientBehaviour;

        private void Awake()
        {
            _clientBehaviour = GetComponent<ClientBehaviour>();
            //idea: maybe have a scriptable object that manages all the data, then we only need to send data that points to a certain scene setup? 
        }
    }
}