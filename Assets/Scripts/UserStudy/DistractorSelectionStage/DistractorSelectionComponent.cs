﻿using System;
using System.Collections.Generic;
using DistractorProject.Transport.DataContainer;
using DistractorProject.UserStudy.MarkerPointStage;
using UnityEngine;

namespace DistractorProject.UserStudy.DistractorSelectionStage
{
    public class DistractorSelectionComponent : ReceivingStudyStageComponent<DistractorSelectionStageEvent>
    {
        //todo place distractor at a certain position 

        public DistractorPlacementSetupComponent distractorPlacement;
        public DistractorTaskComponent distractorTaskComponent;

        private List<Vector3> _placementPositions;
        private byte _currentLoadLevel;
        private int _currentMarker;
        private int[] _markerOrder;
        private int _selectionCount;

        protected override void OnStudyStageStart(DistractorSelectionStageEvent studyEvent)
        {
            _placementPositions = distractorPlacement.DistractorPlacementPositions;
            Manager.RegisterCallback<DistractorSelectionTrialData>(OnTrialDataReceived);
            distractorTaskComponent.OnTaskCompleted += OnTrialCompleted;
            Manager.TransmitNetworkMessage(new ConfirmationData());
        }
        
        private void OnTrialCompleted()
        {
            if (_currentMarker < _markerOrder.Length)
            {
                StartTrialRun();
                return;
            }
            Manager.TransmitNetworkMessage(new TrialCompletedData
            {
                LoadLevel = _currentLoadLevel
            });
        }


        private void OnTrialDataReceived(DistractorSelectionTrialData data)
        {
            //todo place distractor at the correct location
            
            _currentLoadLevel = data.loadLevel;
            _markerOrder = data.markers;
            _currentMarker = 0;
            _selectionCount = data.selectionCount;
            distractorTaskComponent.EnableCanvas();
            StartTrialRun();
        }

        private void StartTrialRun()
        {
            var position = _placementPositions[_markerOrder[_currentMarker]];
            _currentMarker++;
            StartTrial(position, _selectionCount, _currentLoadLevel == 1 ? 0 : 1);
        }

        private void StartTrial(Vector3 position, int selectionCount, int distractorGroup)
        {
            distractorTaskComponent.RepositionCanvas(position);
            //todo setup the correct number of iterations (we can probably just set it up in a way that the distractor task itself just informs us on completion 
            Debug.Log("Starting distractor trial");
            distractorTaskComponent.StartNewTrial(selectionCount, distractorGroup);
        }

        protected override void OnStudyStageEnd(DistractorSelectionStageEvent studyEvent)
        {
            Manager.UnregisterCallback<DistractorSelectionTrialData>(OnTrialDataReceived);
            distractorTaskComponent.DisableCanvas();
            Debug.Log("All trials completed");
        }
    }
}