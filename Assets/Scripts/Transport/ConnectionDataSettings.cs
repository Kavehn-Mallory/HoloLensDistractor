using System;

namespace DistractorProject.Transport
{
    [Serializable]
    public struct ConnectionDataSettings
    {
        
        public NetworkEndpointSetting endpointSource;
        public IpAddress ipAddress;

        public int port;
    }
}