using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

namespace DistractorProject.Transport
{
    public class Server : MonoBehaviour
    {
        private NetworkDriver _driver;
        private NativeList<NetworkConnection> _connections;
        public Action<DataStreamReader> OnDataStreamReceived = delegate { };

        public ConnectionDataSettings settings = new()
        {
            port = new ConnectionPortProperty(7777),
            endpointSource = NetworkEndpointSetting.AnyIPv4
        };

        private void Start()
        {
            _driver = NetworkDriver.Create();
            _connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
            
            var endpoint = settings.NetworkEndpoint;
            if (_driver.Bind(endpoint) != 0)
            {
                Debug.LogError("Failed to bind to port 7777.");
                return;
            }
            _driver.Listen();
        }

        private void OnDestroy()
        {
            if (_driver.IsCreated)
            {
                _driver.Dispose();
                _connections.Dispose();
            }
        }

        private void Update()
        {
            _driver.ScheduleUpdate().Complete();

            // Clean up connections.
            for (int i = 0; i < _connections.Length; i++)
            {
                if (!_connections[i].IsCreated)
                {
                    _connections.RemoveAtSwapBack(i);
                    i--;
                }
            }

            // Accept new connections.
            NetworkConnection c;
            while ((c = _driver.Accept()) != default)
            {
                _connections.Add(c);
                Debug.Log("Accepted a connection.");
            }

            for (int i = 0; i < _connections.Length; i++)
            {
                NetworkEvent.Type cmd;
                while ((cmd = _driver.PopEventForConnection(_connections[i], out var stream)) != NetworkEvent.Type.Empty)
                {
                    if (cmd == NetworkEvent.Type.Data)
                    {
                        OnDataStreamReceived.Invoke(stream);
                    }
                    else if (cmd == NetworkEvent.Type.Disconnect)
                    {
                        Debug.Log("Client disconnected from the server.");
                        _connections[i] = default;
                        break;
                    }
                }
            }
        }
    }
}