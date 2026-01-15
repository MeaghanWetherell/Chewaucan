#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class PlayFromAnyScene
{
    private const string BootScenePath = "Assets/Scenes/PersistentObjects.unity";

    private const string namePath = "nextSceneName";
    private const string scenePath = "nextScenePath";

    static PlayFromAnyScene()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            var active = SceneManager.GetActiveScene();
            if (active.path == BootScenePath || active.name.Equals("MainMenuUI"))
            {
                RestorePrev();
                return;
            }

            PlayerPrefs.SetString(namePath, active.name);
            PlayerPrefs.SetString(scenePath, active.path);

            // Force Play Mode to start in Bootstrap
            var bootAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(BootScenePath);
            if (bootAsset == null)
            {
                Debug.LogError($"Bootstrap scene not found at {BootScenePath}");
                return;
            }
            
            EditorSceneManager.playModeStartScene = bootAsset;
        }
        else if (state == PlayModeStateChange.EnteredPlayMode)
        {
            string sname = PlayerPrefs.GetString(namePath);
            if (!sname.Equals(""))
            {
                SceneManager.sceneLoaded += SetActiveScene;
                SceneManager.LoadScene(sname, LoadSceneMode.Additive);
            }
            
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            RestorePrev();
        }
    }

    private static void RestorePrev()
    {
        var prevAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(PlayerPrefs.GetString(scenePath));
        if (prevAsset == null)
        {
            Debug.Log("Couldn't find prev scene");
            return;
        }
        EditorSceneManager.playModeStartScene = prevAsset;
        PlayerPrefs.SetString(namePath, "");
        PlayerPrefs.SetString(scenePath, "");
    }

    private static void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        string sname = PlayerPrefs.GetString(namePath);
        if (scene.name.Equals(sname))
        {
            SceneManager.SetActiveScene(scene);
            SceneManager.sceneLoaded -= SetActiveScene;
        }
    }

}
#endif