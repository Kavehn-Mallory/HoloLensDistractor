using System;
using DistractorProject.Core;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

namespace DistractorProject.Transport
{
    public class Client : MonoBehaviour
    {
        [SerializeField]
        private ConnectionDataSettings settings = new()
        {
            endpointSource = NetworkEndpointSetting.LoopbackIPv4,
            port = new ConnectionPortProperty(7777)
        };
        
        private NetworkDriver _driver;
        private NetworkConnection _connection;

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

        public bool SendNetworkMessage(ISerializer data)
        {
            if (!_connection.IsCreated)
            {
                return false;
            }

            _driver.BeginSend(NetworkPipeline.Null, _connection, out var writer);
            ConnectionDataWriter.SendMessage(ref writer, data);
            _driver.EndSend(writer);
            return true;
        }

        private void ProcessData(uint value, ref DataStreamReader stream)
        {
            throw new NotImplementedException();
        }
    }

}