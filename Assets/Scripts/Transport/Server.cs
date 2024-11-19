using System;
using DistractorProject.Core;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

namespace DistractorProject.Transport
{
    public class Server : Singleton<Server>, INetworkManager
    {
        private NetworkDriver _driver;
        private NativeList<NetworkConnection> _connections;

        public ConnectionDataSettings settings = new()
        {
            port = new ConnectionPortProperty(7777),
            endpointSource = NetworkEndpointSetting.AnyIPv4
        };
        
        private NetworkMessageEventHandler _eventHandler;
        private NetworkPipeline _pipeline;

        protected override void Awake()
        {
            base.Awake();
            _eventHandler = new NetworkMessageEventHandler();
        }

        private void Start()
        {
            _driver = NetworkDriver.Create();
            _pipeline = PipelineCreation.CreatePipeline(ref _driver);
            _connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
            
            var endpoint = settings.NetworkEndpoint;
            if (_driver.Bind(endpoint) != 0)
            {
                Debug.LogError("Failed to bind to port 7777.");
                return;
            }
            _driver.Listen();
        }
        
        public void RegisterCallback<T>(Action<T> callback) where T : ISerializer, new() =>
            _eventHandler.RegisterCallback(callback);

        public void UnregisterCallback<T>(Action<T> callback) where T : ISerializer, new() =>
            _eventHandler.UnregisterCallback(callback);

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
                //todo -> turn this into an event 
                Debug.Log("Accepted a connection.");
            }

            for (int i = 0; i < _connections.Length; i++)
            {
                NetworkEvent.Type cmd;
                while ((cmd = _driver.PopEventForConnection(_connections[i], out var stream)) != NetworkEvent.Type.Empty)
                {
                    if (cmd == NetworkEvent.Type.Data)
                    {
                        ProcessData(ref stream);
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
        
        public bool TransmitNetworkMessage(ISerializer data)
        {
            bool success = true;
            foreach (var connection in _connections)
            {
                if (!connection.IsCreated)
                {
                    success = false;
                    continue;
                }
                _driver.BeginSend(_pipeline, connection, out var writer);
                ConnectionDataWriter.SendMessage(ref writer, data);
                _driver.EndSend(writer);
            }
            return success;
        }
        
        private void ProcessData(ref DataStreamReader stream)
        {
            var typeIndex = stream.ReadByte();
            var type = DataSerializationIndexer.GetTypeForTypeIndex(typeIndex);

            if (!_eventHandler.TriggerCallback(type, ref stream))
            {
                Debug.LogError($"Type {type} is not handled yet by {nameof(NetworkMessageEventHandler)}. This either means that {type} does not implement {nameof(ISerializer)} or that the type does not have a default constructor");
            }
        }
    }
}