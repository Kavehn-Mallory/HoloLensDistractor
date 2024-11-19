using System;
using System.Collections.Generic;

namespace DistractorProject.Transport.StudyScopeTest
{
    public interface IStudyScopeSystemThing
    {
        public SenderSide SendingSide { get; set; }
        public IStudyData Data { get; set; }
    }

    public interface IStudyData
    {
        public IStudyScopeReceiver AsReceiver(INetworkManager manager);

        public IStudyScopeSender AsSender(INetworkManager manager);
    }

    public interface IStudyScopeReceiver : IStudyScope
    {
        
    }

    public interface IStudyScopeSender : IStudyScope
    {
        public void StartTestCase();
    }

    public interface IStudyScope
    {
        public event Action OnScopeBegin;

        public event Action OnScopeEnd;
    }

    public enum SenderSide
    {
        Server,
        Client
    }
    
   
}