using UnityEngine;

namespace DistractorProject.Transport
{
    [CreateAssetMenu(fileName = "Port", menuName = "Transport/ConnectionData", order = 0)]
    public class PortData : ScriptableObject
    {
        public ushort port;
    }
}