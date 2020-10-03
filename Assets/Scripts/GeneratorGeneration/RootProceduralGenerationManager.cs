using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GeneratorGeneration
{
    public class RootProceduralGenerationManager : MonoBehaviour
    {
        [SerializeField] private string _sceneToLoadPath;
        void Start()
        {
            RestartScene();
        }

        private void RestartScene()
        {
            var allScenes = Enumerable.Range(0, SceneManager.sceneCount).Select(c => SceneManager.GetSceneAt(c))
                .ToList();
            if (allScenes.Any(c => c.path.Contains(_sceneToLoadPath)))
            {
                var op = SceneManager.UnloadSceneAsync(_sceneToLoadPath);
                op.completed += operation => SceneManager.LoadScene(_sceneToLoadPath, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(_sceneToLoadPath, LoadSceneMode.Additive);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartScene();
            }
        }
    }
}