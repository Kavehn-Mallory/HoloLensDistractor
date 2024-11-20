using System;
using System.Collections;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using UnityEngine;

namespace DistractorProject.UserStudy
{
    public abstract class UserStudyManager : MonoBehaviour
    {
        public StudyStageComponent[] studyStages = Array.Empty<StudyStageComponent>();

        private int _studyIndex;

        protected float SecondsToWait = 5f;

        public abstract INetworkManager Manager { get; }

        protected virtual IEnumerator Start()
        {
            yield return new WaitForSeconds(SecondsToWait);
            Manager.RegisterCallback<UserStudyBeginData>(OnStudyBegin);
            foreach (var studyStage in studyStages)
            {
                studyStage.Manager = Manager;
                if (studyStage is ReceivingStudyStageComponent receiver)
                {
                    receiver.RegisterStudyComponent(Manager);
                }
                
                studyStage.OnStudyEnd += OnStudyStageEnds;
            }

            if (Manager is Client)
            {
                Manager.TransmitNetworkMessage(new UserStudyBeginData());
            }
        }

        private void OnStudyBegin(UserStudyBeginData obj)
        {
            BeginNextStudy();
        }

        private void BeginNextStudy()
        {
            if (studyStages[_studyIndex] is SendingStudyStageComponent sender)
            {
                sender.StartStudy(Manager);
            }
            else
            {
                Manager.TransmitNetworkMessage(new UserStudyBeginData());
            }
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private void OnStudyStageEnds()
        {
            var study = studyStages[_studyIndex];
            if (study is ReceivingStudyStageComponent receiver)
            {
                receiver.UnregisterStudyComponent(Manager);
            }
            _studyIndex++;
            
            //start next study
            if (_studyIndex >= studyStages.Length)
            {
                //todo lets do something in here?
                Debug.Log("Study completed");
                //Client.Instance.UnregisterCallback<StartStudyStageData>(OnNextStudyStageStart);
                return;
            }

            BeginNextStudy();
        }

    }




}