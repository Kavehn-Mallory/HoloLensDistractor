using System.Threading.Tasks;
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


    }
}