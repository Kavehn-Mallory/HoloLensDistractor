using System;
using System.Linq;
using Eflatun.SceneReference;

namespace DistractorProject.SceneManagement
{
    [Serializable]
    public class SceneGroup
    {
        public string groupName = "New Scene Group";
        public SceneData[] scenes;

        public string FindSceneNameByType(SceneType sceneType)
        {
            return scenes.FirstOrDefault(scene => scene.sceneType == sceneType)?.reference.Name;
        }
    }

    
    
    public enum SceneType {ActiveScene, MainMenu, UserInterface, HUD, Cinematic, Environment, Tooling}
    [Serializable]
    public class SceneData
    {
        public SceneReference reference;
        public string Name => reference.Name;
        public SceneType sceneType;
    }
}