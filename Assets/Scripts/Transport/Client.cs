using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DistractorProject.Transport
{
    public class Client : MonoBehaviour
    {
        [SerializeField]
        private ConnectionDataSettings settings;
        
        private NetworkDriver _driver;
        private NetworkConnection _connection;
        
        public InputActionReference reference;
        public InputActionProperty reference2;
        public ConnectionPortProperty testPort;

        private void Start()
        {
            _driver = NetworkDriver.Create();
            
            var endpoint = settings.NetworkEndpoint;
            _connection = _driver.Connect(endpoint);
        }

        private void OnDestroy()
        {
            _driver.Dispose();
        }

        private void Update()
        {
            _driver.ScheduleUpdate().Complete();

            if (!_connection.IsCreated)
            {
                return;
            }

            NetworkEvent.Type cmd;
            while ((cmd = _connection.PopEvent(_driver, out var stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {
                    Debug.Log("We are now connected to the server.");
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    uint value = stream.ReadUInt();
                    
                    Debug.Log($"Got the value {value} back from the server.");
                    ProcessData(value, ref stream);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client got disconnected from server.");
                    _connection = default;
                }
            }
        }

        private void ProcessData(uint value, ref DataStreamReader stream)
        {
            throw new NotImplementedException();
        }
    }
}