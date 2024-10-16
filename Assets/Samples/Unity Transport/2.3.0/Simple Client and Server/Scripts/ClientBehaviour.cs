using UnityEngine;
using TMPro;

namespace Unity.Networking.Transport.Samples
{
    public class ClientBehaviour : MonoBehaviour
    {
        NetworkDriver m_Driver;
        NetworkConnection m_Connection;

        [SerializeField]
        private string ipAddress;
        [SerializeField]
        private ushort port;

        public TMP_Text debugText;

        void Start()
        {
            m_Driver = NetworkDriver.Create();
            
            var endpoint = NetworkEndpoint.Parse(ipAddress, port);
            m_Connection = m_Driver.Connect(endpoint);
            debugText.text = "Trying to connect";
        }

        void OnDestroy()
        {
            m_Driver.Dispose();
        }

        void Update()
        {
            m_Driver.ScheduleUpdate().Complete();

            if (!m_Connection.IsCreated)
            {
                return;
            }

            Unity.Collections.DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {
                    debugText.text = "We are now connected to the server.";
                    Debug.Log("We are now connected to the server.");
                    
                    uint value = 1;
                    m_Driver.BeginSend(m_Connection, out var writer);
                    writer.WriteUInt(value);
                    m_Driver.EndSend(writer);
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    uint value = stream.ReadUInt();
                    Debug.Log($"Got the value {value} back from the server.");

                    m_Connection.Disconnect(m_Driver);
                    m_Connection = default;
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    debugText.text = "Client got disconnected.";
                    Debug.Log("Client got disconnected from server.");
                    m_Connection = default;
                }
            }
        }
    }
}