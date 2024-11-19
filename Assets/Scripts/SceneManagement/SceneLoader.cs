﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using Unity.Mathematics;
using UnityEngine;

namespace DistractorProject.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneGroup[] sceneGroups;
        
        [SerializeField]
        private float targetProgress;
        
        public readonly SceneGroupManager Manager = new SceneGroupManager();
        
        async void Start()
        {
            await LoadSceneGroup(0);
        }
        
        private void OnEnable()
        {
            Server.Instance.RegisterCallback<SceneGroupChangeData>(OnSceneGroupChangeRequest);
        }

        private void OnDisable()
        {
            if (Server.TryGetInstance(out var instance))
            {
                instance.UnregisterCallback<SceneGroupChangeData>(OnSceneGroupChangeRequest);
            }
            
        }

        private async void OnSceneGroupChangeRequest(SceneGroupChangeData data)
        {
            await LoadSceneGroup(data.index);
        }
        
        public async Task LoadSceneGroup(int index)
        {
            targetProgress = 1f;
            if (index < 0 || index >= sceneGroups.Length)
            {
                Debug.LogError($"Invalid scene group index {index}");
                return;
            }

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => targetProgress = math.max(target, targetProgress);

            await Manager.LoadScenes(sceneGroups[index], progress);
        }

        public int[] FindUserStudyScenes()
        {
            var results = new List<int>();
            for (var i = 0; i < sceneGroups.Length; i++)
            {
                var sceneGroup = sceneGroups[i];
                if (sceneGroup.ContainsScenesWithType(SceneType.UserStudy))
                {
                    results.Add(i);
                }
            }

            return results.ToArray();
        }


    }
}