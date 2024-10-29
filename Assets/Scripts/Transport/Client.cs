using System;
using DistractorProject.Core;
using DistractorProject.Transport.DataContainer;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

namespace DistractorProject.Transport
{
    public class Client : Singleton<Client>
    {
        [SerializeField]
        private ConnectionDataSettings settings = new()
        {
            endpointSource = NetworkEndpointSetting.LoopbackIPv4,
            port = new ConnectionPortProperty(7777)
        };
        
        private NetworkDriver _driver;
        private NetworkConnection _connection;

        private NetworkMessageEventHandler _eventHandler;

        public void RegisterCallback<T>(Action<T> callback) where T : ISerializer, new() =>
            _eventHandler.RegisterCallback(callback);

        public void UnregisterCallback<T>(Action<T> callback) where T : ISerializer, new() =>
            _eventHandler.UnregisterCallback(callback);

        private void Start()
        {
            _eventHandler = new NetworkMessageEventHandler();
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
                    ProcessData(ref stream);
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