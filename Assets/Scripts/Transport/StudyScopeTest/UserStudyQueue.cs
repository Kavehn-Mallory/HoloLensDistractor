using System.Collections.Generic;

namespace DistractorProject.Transport.StudyScopeTest
{
    public class UserStudyQueue
    {
        private List<IStudyScopeSystemThing> _studyScopes = new();
        private readonly List<IStudyScope> _studyScopes2 = new();

        public void SetupStudy(INetworkManager manager, SenderSide side)
        {
            
            _studyScopes2.Clear();
            foreach (var study in _studyScopes)
            {
                if (study.SendingSide == side)
                {
                    //todo connect the scope to the next, when ending?
                    var scope = study.Data.AsSender(manager);

                    _studyScopes2[^1].OnScopeEnd += scope.StartTestCase;
                    _studyScopes2.Add(scope);
                }
                else
                {
                    _studyScopes2.Add(study.Data.AsReceiver(manager));
                }
            }
        }
        
    }

    public static class UserStudyQueueBuilder
    {

    }
}