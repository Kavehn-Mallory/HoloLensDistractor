using DistractorProject.Transport;
using UnityEngine;

namespace DistractorProject
{
    public class ClientTestScript : MonoBehaviour
    {
        public ConnectionDataSettings settings;


        [ContextMenu("Test")]
        private void Test()
        {
            Debug.Log(settings.ipAddress);
        }
    }
}