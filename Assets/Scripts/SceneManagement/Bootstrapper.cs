using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DistractorProject.SceneManagement
{
    public class Bootstrapper : MonoBehaviour
    {
        private static bool _loadBootstrapper;

        [MenuItem("Tools/Toggle Bootstrapper", true)]
        private static bool SetToggleState()
        {
            Menu.SetChecked("Tools/Toggle Bootstrapper", _loadBootstrapper);
            return true;
        }

        [MenuItem("Tools/Toggle Bootstrapper")]
        public static void ToggleBootstrapper()
        {
            _loadBootstrapper = !_loadBootstrapper;
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static async void Init()
        {
            AsyncOperation bootstrapperLoadOperation = null;
            Debug.Log("Bootstrapper...");

#if UNITY_EDITOR
            //do nothing and just use the default scene?
            if (_loadBootstrapper)
            {
                bootstrapperLoadOperation = SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
            }
#elif UNITY_ANDROID
            bootstrapperLoadOperation = SceneManager.LoadSceneAsync("VR_Bootstrapper", LoadSceneMode.Single);

#else
//it's currently a little bit convoluted, but this way we can hopefully switch to the correct scene.
             bootstrapperLoadOperation = SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
#endif
            /*if (RuntimePlatform.Android == Application.platform)
            {

            }
            else
            {

            }*/


            if (bootstrapperLoadOperation == null)
            {
                return;
            }
            while (!bootstrapperLoadOperation.isDone)
            {
                await Task.Delay(100);
            }
        }
    }
}