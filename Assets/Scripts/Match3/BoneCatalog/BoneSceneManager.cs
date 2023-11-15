using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3
{
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

        public void loadBoneScene(MeshDataObj toLoad)
        {
            if (toLoad == null)
                return;
            curObj = toLoad;
            wasLoaded = true;
            MainSceneDataSaver.mainSceneDataSaver.prepareForUnload();
            SceneManager.LoadScene(4);
        }
    }
}
