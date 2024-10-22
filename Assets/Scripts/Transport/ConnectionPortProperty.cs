using System;
using UnityEngine;

namespace DistractorProject.Transport
{
    [Serializable]
    public struct ConnectionPortProperty
    {
        public ushort Port
        {
            get
            {
                if (data)
                {
                    return data.port;
                }

                return port;
            }
        }

        private PortData data;
        [SerializeField]
        private ushort port;
    }
}