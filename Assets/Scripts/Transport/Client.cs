using System;
using DistractorProject.Core;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

namespace DistractorProject.Transport
{
    public class Client : Singleton<Client>, INetworkManager
    {
        [SerializeField]
        private ConnectionDataSettings settings = new()
        {
            endpointSource = NetworkEndpointSetting.LoopbackIPv4,
            port = new ConnectionPortProperty(7777)
        };
        
        private NetworkDriver _driver;
        private NetworkConnection _connection;
        private NetworkPipeline _pipeline;

        private NetworkMessageEventHandler _eventHandler;

        public void RegisterCallback<T>(Action<T> callback) where T : ISerializer, new() =>
            _eventHandler.RegisterCallback(callback);

        public void UnregisterCallback<T>(Action<T> callback) where T : ISerializer, new() =>
            _eventHandler.UnregisterCallback(callback);

        protected override void Awake()
        {
            base.Awake();
            _eventHandler = new NetworkMessageEventHandler();
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

        public bool TransmitNetworkMessage(ISerializer data)
        {
            if (!_connection.IsCreated)
            {
                return false;
            }

            _driver.BeginSend(_pipeline, _connection, out var writer);
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


        public void Connect()
        {
            if (_connection.IsCreated)
            {
                return;
            }
            _driver = NetworkDriver.Create();
            _pipeline = PipelineCreation.CreatePipeline(ref _driver);
            var endpoint = settings.NetworkEndpoint;
            _connection = _driver.Connect(endpoint);
            
        }
    }

}