﻿using System;
using Unity.Networking.Transport;

namespace DistractorProject.Transport
{
    [Serializable]
    public struct ConnectionDataSettings
    {
        
        public NetworkEndpointSetting endpointSource;
        public IpAddress ipAddress;

        public int port;

        public NetworkEndpoint NetworkEndpoint
        {
            get
            {
                var castedPort = (ushort)port;
                switch (endpointSource)
                {
                    case NetworkEndpointSetting.Custom: 
                        return NetworkEndpoint.Parse(ipAddress.ToString(), castedPort);
                    case NetworkEndpointSetting.AnyIPv4:
                        return NetworkEndpoint.AnyIpv4.WithPort(castedPort);
                    case NetworkEndpointSetting.AnyIPv6:
                        return NetworkEndpoint.AnyIpv6.WithPort(castedPort);
                    case NetworkEndpointSetting.LoopbackIPv4: return NetworkEndpoint.LoopbackIpv4.WithPort(castedPort);
                    case NetworkEndpointSetting.LoopbackIPv6: return NetworkEndpoint.LoopbackIpv6.WithPort(castedPort);
                    default:
                        return NetworkEndpoint.Parse(ipAddress.ToString(), castedPort);
                }
            }
        }
    }
}