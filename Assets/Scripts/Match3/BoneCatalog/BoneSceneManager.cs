using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3
{
    //responsible for loading and unloading the bone viewer and carrying information to the bone viewer scene
    public class BoneSceneManager : MonoBehaviour
    {
        public static BoneSceneManager boneSceneManager;

        public MeshDataObj curObj;

        public bool wasLoaded = false;
    
        private void Awake()
        {
            if (boneSceneManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(boneSceneManager.gameObject);
            }
            boneSceneManager = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void LoadBoneScene(MeshDataObj toLoad)
        {
            if (toLoad == null)
                return;
            curObj = toLoad;
            wasLoaded = true;
            SceneLoadWrapper.sceneLoadWrapper.LoadScene("BoneViewer");
        }
    }
}
