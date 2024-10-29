using System;
using System.Linq;
using DistractorProject.Core;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace DistractorProject.Transport
{
    public class NetworkMessageEventHandler
    {
        
        
        private readonly IInvoker[] _invocationHelper;

        public NetworkMessageEventHandler()
        {
            //todo make this work in the build. It currently won't due to the fact that it relies on editor code 
            //todo maybe look if we can cache this just into strings or something and then lookup inside the build? would be fine by me tbh 
            var serializableData = TypeCache.GetTypesDerivedFrom<ISerializer>().Where(s => s.IsValueType || s.GetConstructor(Type.EmptyTypes) != null).ToList();
            _invocationHelper = new IInvoker[serializableData.Count];
            Debug.Log(_invocationHelper.Length);
            for (var i = 0; i < serializableData.Count; i++)
            {
                var data = serializableData[i];
                var invoker = typeof(InvocationHelper<>).MakeGenericType(data);

                _invocationHelper[i] = (IInvoker)Activator.CreateInstance(invoker);
            }
        }


        
        

        public bool TriggerCallback(Type type, ref DataStreamReader stream)
        {
            foreach (var invoker in _invocationHelper)
            {
               
                if (type == invoker.InvocationType)
                {
                    invoker.Invoke(ref stream);
                    return true;
                }
            }

            return false;
        }

        public void RegisterCallback<T>(Action<T> test) where T : ISerializer, new()
        {
            foreach (var invoker in _invocationHelper)
            {
                if (invoker is InvocationHelper<T> helper)
                {
                    helper.RegisterCallback(test);
                    return;
                }
            }
        }
        
        public void UnregisterCallback<T>(Action<T> test) where T : ISerializer, new()
        {
            foreach (var invoker in _invocationHelper)
            {
                if (invoker is InvocationHelper<T> helper)
                {
                    helper.UnregisterCallback(test);
                    return;
                }
            }
        }
        
        
        public interface IInvoker
        {
            public Type InvocationType { get; }
            
            public void Invoke(ref DataStreamReader stream);
        }
        
        public interface IInvoker<T> : IInvoker where T : ISerializer
        {

            public void RegisterCallback(Action<T> callback);

            public void UnregisterCallback(Action<T> callback);
        }
        
        public class InvocationHelper<T> : IInvoker<T> where T : ISerializer, new()
        {

            public Type InvocationType => typeof(T);
            
            private Action<T> _actionToInvoke = delegate {};

            public void Invoke(ref DataStreamReader stream)
            {
                var data = new T();
                data.Deserialize(ref stream);
                _actionToInvoke.Invoke(data);
            }

            public void Invoke(T data)
            {
                _actionToInvoke.Invoke(data);
            }

            public void RegisterCallback(Action<T> callback)
            {
                _actionToInvoke += callback;
            }

            public void UnregisterCallback(Action<T> callback)
            {
                _actionToInvoke -= callback;
            }
        }
    }
}